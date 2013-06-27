using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Poly2Tri;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class FluidSystem : ISystem
    {
        public const int MAX_PARTICLES = 10000;
        public const int MAX_NEIGHBORS = 75;
        public const float RADIUS = 0.9f;
        public const float RADIUS_SQ = RADIUS * RADIUS;
        public const float IDEAL_RADIUS = 50.0f;
        public const float IDEAL_RADIUS_SQ = IDEAL_RADIUS * IDEAL_RADIUS;
        public const float MULTIPLIER = IDEAL_RADIUS / RADIUS;
        public const float VISCOSITY = 0.004f;
        public const float GAS_CONSTANT = 5f;
        public const float CELL_SPACING = 0.6f;
        public const float CELL_SPACING_SQ = CELL_SPACING * CELL_SPACING;
        public const float MAX_PRESSURE = 0.8f;
        public const float MAX_PRESSURE_NEAR = 1.6f;
        public static Vector2 SIMULATION_MARGIN = new Vector2(6f, 6f);
        public static Vector2 LIQUID_ORIGIN = new Vector2(999999999f);
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private PhysicsSystem _physicsSystem;
        private RenderSystem _renderSystem;
        public Dictionary<int, Dictionary<int, List<int>>> fluidGrid;
        public Particle[] liquid;
        public int[] activeParticles;
        public int numActiveParticles = 0;
        private int _initializedParticleCount = 0;
        private Vector2 _gravity = new Vector2(0, 9.8f) / 3000;
        private Dictionary<int, List<int>> _collisionGridX;
        private List<int> _collisionGridY;
        private Vector2[] _simPositions;
        private Vector2[] _simVelocities;
        private Vector2[] _delta;
        private object _calculateForcesLock = new object();
        public bool debug = false;
        public bool drawCells = false;
        public bool drawVelocities = true;
        private float dt = 1f / 60f;
        private bool _paused;
        private bool _singleStep;
        public AABB simulationAABB;
        private bool _skipAABBUpdate;

        public int defaultPriority { get { return 10; } }
        public SystemType systemType { get { return SystemType.Fluid; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public FluidSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;

            _physicsSystem = (PhysicsSystem)_systemManager.getSystem(SystemType.Physics);
            _renderSystem = (RenderSystem)_systemManager.getSystem(SystemType.Render);
            fluidGrid = new Dictionary<int, Dictionary<int, List<int>>>();
            liquid = new Particle[MAX_PARTICLES];
            activeParticles = new int[MAX_PARTICLES];
            _simPositions = new Vector2[MAX_PARTICLES];
            _simVelocities = new Vector2[MAX_PARTICLES];
            _delta = new Vector2[MAX_PARTICLES];
            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                liquid[i] = new Particle(this, i, LIQUID_ORIGIN);
            }
        }

        public int getFluidGridX(float x) { return (int)Math.Floor(x / CELL_SPACING); }
        public int getFluidGridY(float y) { return (int)Math.Floor(y / CELL_SPACING); }

        // createFluidBody
        public void createFluidBody(List<Vector2> polygonPoints)
        {
            List<PolygonPoint> P2TPoints = new List<PolygonPoint>();
            Polygon polygon;
            Vector2 topLeft = polygonPoints[0];
            Vector2 bottomRight = polygonPoints[0];
            float spacing = RADIUS / 3.7f;
            Random random = new Random();

            foreach (Vector2 point in polygonPoints)
            {
                topLeft = Vector2.Min(topLeft, point);
                bottomRight = Vector2.Max(bottomRight, point);
                P2TPoints.Add(new PolygonPoint(point.X, point.Y));
            }
            polygon = new Polygon(P2TPoints);

            for (float i = topLeft.X; i < bottomRight.X; i += spacing)
            {
                for (float j = topLeft.Y; j < bottomRight.Y; j += spacing)
                {
                    Vector2 jitter = new Vector2(-1 + (float)random.NextDouble() * 2, -1 + (float)random.NextDouble() * 2) * (spacing * 0.2f);
                    Vector2 point = new Vector2(i, j) + jitter;
                    if (polygon.IsPointInside(new PolygonPoint(point.X, point.Y)))
                        createParticle(point, Vector2.Zero);
                }
            }
        }

        // relaxFluid -- Runs the simulation for all particles
        public void relaxFluid()
        {
            simulationAABB.LowerBound.X = float.MinValue;
            simulationAABB.LowerBound.Y = float.MinValue;
            simulationAABB.UpperBound.X = float.MaxValue;
            simulationAABB.UpperBound.Y = float.MaxValue;
            _skipAABBUpdate = true;
            for (int i = 0; i < 300; i++)
                update();
            _skipAABBUpdate = false;
        }

        // createParticle
        public void createParticle(Vector2 position, Vector2 velocity)
        {
            Particle particle = liquid[_initializedParticleCount];
            particle.position = position;
            particle.oldPosition = position;
            particle.velocity = velocity;
            particle.alive = true;
            _initializedParticleCount++;
        }

        // prepareSimulation
        private void prepareSimulation(int index)
        {
            // Neighbor search
            //liquid[index].findNeighbors();
            findNeighbors(index);

            // Reset influences
            liquid[index].entityInfluenceCount = 0;

            // Reset collision flags
            liquid[index].skipMovementUpdate = false;
            liquid[index].numFixturesToTest = 0;

            // Reset pressures
            liquid[index].p = 0;
            liquid[index].pnear = 0;

            // Simulation-specifc values
            _simPositions[index] = liquid[index].position * MULTIPLIER;
            _simVelocities[index] = liquid[index].velocity * MULTIPLIER;
            _delta[index] = Vector2.Zero;

            // Store old position
            liquid[index].oldPosition = liquid[index].position;
        }

        // calculatePressure
        private void calculatePressure(int index)
        {
            // Current particle
            Particle particle = liquid[index];

            // Particle pressure calculated by particle proximity
            // Pressures = 0 if all particles within range are idealRad distance away
            for (int a = particle.neighborCount - 1; a >= 0; a--)
            {
                int neighborIndex = particle.neighbors[a];
                particle.relativePosition[a] = _simPositions[neighborIndex] - _simPositions[particle.index];

                float vlensqr = particle.relativePosition[a].LengthSquared();
                //within idealRad check
                if (vlensqr < IDEAL_RADIUS_SQ)
                {
                    particle.distances[a] = (float)Math.Sqrt(vlensqr);
                    if (particle.distances[a] < Settings.Epsilon) particle.distances[a] = IDEAL_RADIUS - .01f;
                    particle.oneminusq[a] = 1.0f - particle.distances[a] / IDEAL_RADIUS;
                    float oneminusqSq = particle.oneminusq[a] * particle.oneminusq[a];
                    particle.p = (particle.p + oneminusqSq);
                    particle.pnear = (particle.pnear + oneminusqSq * particle.oneminusq[a]);
                }
                else
                {
                    particle.distances[a] = float.MaxValue;
                }
            }

            particle.pressure = (particle.p - GAS_CONSTANT) / 2.0F; //normal pressure term
            particle.pressureNear = particle.pnear / 2.0F; //near particles term
            particle.pressure = particle.pressure > MAX_PRESSURE ? MAX_PRESSURE : particle.pressure;
            particle.pressureNear = particle.pressureNear > MAX_PRESSURE_NEAR ? MAX_PRESSURE_NEAR : particle.pressureNear;
        }

        // calculateForce
        private Vector2[] calculateForce(int index, Vector2[] accumulatedDelta)
        {
            // Current particle
            Particle particle = liquid[index];

            // Now actually apply the forces
            for (int a = particle.neighborCount - 1; a >= 0; a--)
            {
                int neighborIndex = particle.neighbors[a];
                Particle neighbor = liquid[neighborIndex];

                if (particle.distances[a] < IDEAL_RADIUS)
                {
                    float factor = particle.oneminusq[a] * (particle.pressure + particle.pressureNear * particle.oneminusq[a]) / (2.0F * particle.distances[a]);
                    Vector2 d = particle.relativePosition[a] * factor;
                    Vector2 relativeVelocity = _simVelocities[neighborIndex] - _simVelocities[particle.index];
                    factor = VISCOSITY * particle.oneminusq[a] * dt;
                    d -= relativeVelocity * factor;

                    // Calculate forces differently based on whether or not the neighboring particle is active (usually
                    // the change is split evenly between two particles, but if one is inactive, it won't be updated.
                    // So if one is inactive, double up on the other).
                    if (neighbor.active)
                    {
                        accumulatedDelta[neighborIndex] += d;
                        accumulatedDelta[particle.index] -= d;
                    }
                    else
                    {
                        accumulatedDelta[particle.index] -= d * 2;
                    }
                }
            }

            // Apply gravitational force
            particle.velocity += _gravity;

            return accumulatedDelta;
        }

        // isInBounds
        private bool isInBounds(ref Vector2 position)
        {
            return position.X > simulationAABB.LowerBound.X &&
                    position.X < simulationAABB.UpperBound.X &&
                    position.Y > simulationAABB.LowerBound.Y &&
                    position.Y < simulationAABB.UpperBound.Y;
        }

        // flagActive
        private void flagActive()
        {
            numActiveParticles = 0;
            for (int i = liquid.Length - 1; i >= 0; i--)
            {
                Particle particle = liquid[i];
                if (particle.alive)
                {
                    if (isInBounds(ref particle.position))
                    {
                        particle.active = true;
                        activeParticles[numActiveParticles] = i;
                        numActiveParticles++;
                    }
                    else
                    {
                        particle.active = false;
                    }
                }
            }
        }

        // prepareCollisions
        private void prepareCollisions()
        {
            // Query the world using the screen's AABB
            _physicsSystem.world.QueryAABB((Fixture fixture) =>
            {
                // Skip certain collisions
                //UserData data = fixtureProxy.fixture.GetBody().GetUserData() as UserData;
                //if (data.actorType == ActorType.WALL_GROUP || data.actorType == ActorType.GROUND)
                //    return true;

                AABB aabb;
                Transform transform;
                fixture.Body.GetTransform(out transform);
                fixture.Shape.ComputeAABB(out aabb, ref transform, 0);
                int Ax = getFluidGridX(aabb.LowerBound.X);
                int Ay = getFluidGridY(aabb.LowerBound.Y);
                int Bx = getFluidGridX(aabb.UpperBound.X) + 1;
                int By = getFluidGridY(aabb.UpperBound.Y) + 1;
                for (int i = Ax; i < Bx; i++)
                {
                    for (int j = Ay; j < By; j++)
                    {
                        if (fluidGrid.TryGetValue(i, out _collisionGridX) && _collisionGridX.TryGetValue(j, out _collisionGridY))
                        {
                            for (int n = 0; n < _collisionGridY.Count; n++)
                            {
                                Particle particle = liquid[_collisionGridY[n]];
                                if (particle.numFixturesToTest < Particle.MAX_FIXTURES_TO_TEST)
                                {
                                    particle.fixturesToTest[particle.numFixturesToTest] = fixture;
                                    particle.numFixturesToTest++;
                                }
                            }
                        }
                    }
                }

                return true;
            },
                ref simulationAABB);
        }

        // resolveCollisions
        private void resolveCollision(int index)
        {
            Particle particle = liquid[index];

            // Resolve collisions between particles and fixtures
            for (int i = 0; i < particle.numFixturesToTest; i++)
            {
                Fixture fixture = particle.fixturesToTest[i];
                if (fixture.Shape == null)     // fixtures can be destroyed before they're tested
                    continue;

                Vector2 newPosition = particle.position + particle.velocity + _delta[index];
                if (fixture.TestPoint(ref newPosition, 0.01f))
                {
                    Body body = fixture.Body;
                    int entityId = (int)body.UserData;
                    SkipFluidResolutionComponent skipFluidResolutionComponent = _entityManager.getComponent(entityId, ComponentType.SkipFluidResolution) as SkipFluidResolutionComponent;
                    ParticleInfluenceComponent particleInfluenceComponent = _entityManager.getComponent(entityId, ComponentType.ParticleInfluence) as ParticleInfluenceComponent;
                    bool influenceEntity = particleInfluenceComponent != null;

                    if (skipFluidResolutionComponent == null)
                    {
                        Vector2 closestPoint = Vector2.Zero;
                        Vector2 normal = Vector2.Zero;
                        Vector2 resolvedPosition = Vector2.Zero;

                        if (fixture.ShapeType == ShapeType.Polygon)
                        {
                            // Polygons
                            PolygonShape shape = fixture.Shape as PolygonShape;
                            Transform collisionXF;
                            body.GetTransform(out collisionXF);

                            for (int v = 0; v < shape.Vertices.Count; v++)
                            {
                                particle.collisionVertices[v] = MathUtils.Multiply(ref collisionXF, shape.Vertices[v]);
                                particle.collisionNormals[v] = MathUtils.Multiply(ref collisionXF.R, shape.Normals[v]);
                            }

                            // Find closest edge
                            float shortestDistance = 9999999f;
                            for (int v = 0; v < shape.Vertices.Count; v++)
                            {
                                float distance = Vector2.Dot(particle.collisionNormals[v], particle.collisionVertices[v] - particle.position);
                                if (distance < shortestDistance)
                                {
                                    shortestDistance = distance;
                                    closestPoint = particle.collisionNormals[v] * (distance) + particle.position;
                                    normal = particle.collisionNormals[v];
                                }
                            }
                            resolvedPosition = closestPoint + 0.025f * normal;
                            particle.skipMovementUpdate = true;
                            particle.pressure = MAX_PRESSURE;
                        }
                        else if (fixture.ShapeType == ShapeType.Circle)
                        {
                            // Circles
                            CircleShape shape = fixture.Shape as CircleShape;
                            Vector2 center = shape.Position + body.Position;
                            Vector2 difference = particle.position - center;

                            normal = difference;
                            normal.Normalize();
                            closestPoint = center + difference * (shape.Radius / difference.Length());
                            resolvedPosition = closestPoint + 0.025f * normal;
                            particle.skipMovementUpdate = true;
                            particle.pressure = MAX_PRESSURE;
                        }

                        // Update particle position and velocity
                        if (isInBounds(ref resolvedPosition))
                        {
                            particle.position = resolvedPosition;
                            particle.velocity = (particle.velocity - 1.2f * Vector2.Dot(particle.velocity, normal) * normal) * 0.85f;
                        }

                        // Handle fast moving bodies
                        if (body.LinearVelocity.LengthSquared() > 50f)
                        {
                            Vector2 bodyVelocity = body.GetLinearVelocityFromWorldPoint(particle.oldPosition);
                            body.LinearVelocity = body.LinearVelocity * 0.995f;
                            body.AngularVelocity = body.AngularVelocity * 0.95f;
                            particle.velocity += bodyVelocity * 0.005f;
                        }
                    }

                    // Particle influences
                    if (influenceEntity)
                    {
                        if (particleInfluenceComponent.type == ParticleInfluenceType.Rope)
                        {
                            // Handle rope particle-to-body influences here, since trying to find the correct body through the list of rope nodes would be a pain
                            Vector2 bodyVelocity = body.LinearVelocity;

                            body.LinearVelocity = bodyVelocity * 0.98f + particle.velocity * 1.5f;
                            particle.velocity += bodyVelocity * 0.003f;
                        }
                        else
                        {
                            // Add entityId to list of entities being influenced by this particle
                            particle.entitiesToInfluence[particle.entityInfluenceCount] = (int)body.UserData;
                            particle.entityInfluenceCount++;
                        }
                    }
                }
            }
        }

        // killParticle
        private void killParticle(Particle particle)
        {
            particle.position = LIQUID_ORIGIN;
            particle.alive = false;
            particle.active = false;
            numActiveParticles--;
        }

        // findNeighbors
        public void findNeighbors(int index)
        {
            Particle particle = liquid[index];
            bool neighborSearchDone = false;

            particle.neighborCount = 0;

            for (int nx = -1; nx < 2; nx++)
            {
                for (int ny = -1; ny < 2; ny++)
                {
                    int xc = particle.ci + nx;
                    int yc = particle.cj + ny;
                    if (fluidGrid.TryGetValue(xc, out particle.gridX) && particle.gridX.TryGetValue(yc, out particle.gridY))
                    {
                        int limit = particle.gridY.Count;
                        if (particle.neighborCount + limit > FluidSystem.MAX_NEIGHBORS)
                        {
                            limit = limit - (particle.neighborCount + limit) - FluidSystem.MAX_NEIGHBORS;
                            neighborSearchDone = true;
                        }

                        for (int a = 0; a < limit; a++)
                        {
                            particle.neighbors[particle.neighborCount] = particle.gridY[a];
                            particle.neighborCount++;
                        }

                        if (neighborSearchDone)
                            return;
                    }
                }
            }
        }

        // handleParticleInfluence -- Handles interaction between entities and particles
        public void handleParticleInfluence(Particle particle)
        {
            for (int i = 0; i < particle.entityInfluenceCount; i++)
            {
                int entityId = particle.entitiesToInfluence[i];
                ParticleInfluenceComponent particleInfluenceComponent = _entityManager.getComponent(entityId, ComponentType.ParticleInfluence) as ParticleInfluenceComponent;

                particleInfluenceComponent.particleCount++;
                if (particleInfluenceComponent.type == ParticleInfluenceType.Physical)
                {
                    // Physical body influences -- body-to-particle influences are usually already handled by resolveCollisions()
                    PhysicsComponent physicsComponent = _entityManager.getComponent(entityId, ComponentType.Physics) as PhysicsComponent;
                    
                    physicsComponent.body.ApplyLinearImpulse(particle.oldPosition - particle.position, particle.oldPosition);
                }
                else if (particleInfluenceComponent.type == ParticleInfluenceType.Dynamite)
                {
                    // Dynamite influence
                    PhysicsComponent physicsComponent = _entityManager.getComponent(entityId, ComponentType.Physics) as PhysicsComponent;
                    Vector2 bodyVelocity = physicsComponent.body.LinearVelocity;

                    physicsComponent.body.LinearVelocity = bodyVelocity * 0.98f + particle.velocity;
                    physicsComponent.body.AngularVelocity = physicsComponent.body.AngularVelocity * 0.98f;
                    particle.velocity += bodyVelocity * 0.003f;
                }
                else if (particleInfluenceComponent.type == ParticleInfluenceType.Character)
                {
                    // Character influence
                    PhysicsComponent physicsComponent = _entityManager.getComponent(entityId, ComponentType.Physics) as PhysicsComponent;
                    Vector2 bodyVelocity = physicsComponent.body.LinearVelocity;

                    physicsComponent.body.LinearVelocity = bodyVelocity * 0.95f + particle.velocity;
                    particle.velocity += bodyVelocity * 0.003f;
                }
                else if (particleInfluenceComponent.type == ParticleInfluenceType.Explosion)
                {
                    ExplosionComponent explosionComponent = _entityManager.getComponent(entityId, ComponentType.Explosion) as ExplosionComponent;
                    Vector2 relative;
                    float distanceSq;
                    Vector2 force;

                    relative = particle.position - explosionComponent.position;
                    distanceSq = relative.LengthSquared();
                    relative.Normalize();
                    force = relative * (explosionComponent.strength / Math.Max(distanceSq, 1f));
                    particle.velocity += force * 0.0055f;
                }
            }
        }


        // updateParticle
        public void updateParticle(int index)
        {
            Particle particle = liquid[index];

            // Influence actors
            handleParticleInfluence(particle);

            // Update cell
            updateParticleCell(index);
        }

        // updateCell
        private void updateParticleCell(int index)
        {
            Particle particle = liquid[index];
            int i = getFluidGridX(particle.position.X);
            int j = getFluidGridY(particle.position.Y);

            if (particle.ci == i && particle.cj == j)
                return;
            else
            {
                fluidGrid[particle.ci][particle.cj].Remove(index);

                if (fluidGrid[particle.ci][particle.cj].Count == 0)
                {
                    fluidGrid[particle.ci].Remove(particle.cj);

                    if (fluidGrid[particle.ci].Count == 0)
                    {
                        fluidGrid.Remove(particle.ci);
                    }
                }

                if (!fluidGrid.ContainsKey(i))
                    fluidGrid[i] = new Dictionary<int, List<int>>();
                if (!fluidGrid[i].ContainsKey(j))
                    fluidGrid[i][j] = new List<int>(20);

                fluidGrid[i][j].Add(index);
                particle.ci = i;
                particle.cj = j;
            }
        }

        // Update
        public void update()
        {
            if (!_paused || _singleStep)
            {
                List<ParticleInfluenceComponent> particleInfluenceComponents = _entityManager.getComponents<ParticleInfluenceComponent>(ComponentType.ParticleInfluence);

                // Reset particle influence components
                for (int i = 0; i < particleInfluenceComponents.Count; i++)
                    particleInfluenceComponents[i].particleCount = 0;

                if (!_skipAABBUpdate)
                {
                    // Update simulation AABB
                    Vector2 screenCenter = _renderSystem.screenCenter;
                    Vector2 simHalfScreen = (_renderSystem.halfScreen / _renderSystem.scale) + SIMULATION_MARGIN;
                    simulationAABB.LowerBound = screenCenter - simHalfScreen;
                    simulationAABB.UpperBound = screenCenter + simHalfScreen;
                }

                // Flag active particles
                flagActive();

                // Prepare simulation
                Parallel.For(0, numActiveParticles, i => { prepareSimulation(activeParticles[i]); });

                // Prepare collisions
                prepareCollisions();

                // Calculate pressures
                Parallel.For(0, numActiveParticles, i => { calculatePressure(activeParticles[i]); });

                // Calculate forces
                Parallel.For(
                    0,
                    numActiveParticles,
                    () => new Vector2[MAX_PARTICLES],
                    (i, state, accumulatedDelta) => calculateForce(activeParticles[i], accumulatedDelta),
                    (accumulatedDelta) =>
                    {
                        lock (_calculateForcesLock)
                        {
                            for (int i = numActiveParticles - 1; i >= 0; i--)
                            {
                                _delta[activeParticles[i]] += accumulatedDelta[activeParticles[i]] / MULTIPLIER;
                            }
                        }
                    }
                );

                // Resolve collisions
                Parallel.For(0, numActiveParticles, i => resolveCollision(activeParticles[i]));

                // Move particles
                Parallel.For(0, numActiveParticles, i =>
                {
                    int index = activeParticles[i];
                    Particle particle = liquid[index];

                    if (!particle.skipMovementUpdate)
                    {
                        Vector2 newVelocity = particle.velocity + _delta[index];
                        Vector2 newPosition = particle.position + _delta[index] + newVelocity;

                        if (isInBounds(ref newPosition))
                        {
                            particle.velocity = newVelocity;
                            particle.position = newPosition;
                        }

                        // Update velocity
                        //particle.velocity += _delta[index];

                        // Update position
                        //particle.position += _delta[index];
                        //particle.position += particle.velocity;
                    }
                });

                // Update particles
                for (int i = numActiveParticles - 1; i >= 0; i--)
                    updateParticle(activeParticles[i]);
            }
            _singleStep = false;
        }
    }
}
