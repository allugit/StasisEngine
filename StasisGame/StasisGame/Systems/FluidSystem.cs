using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class FluidSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private PhysicsSystem _physicsSystem;
        private RenderSystem _renderSystem;
        public Dictionary<int, Dictionary<int, List<int>>> fluidGrid;
        public const int MAX_PARTICLES = 10000;
        public const int MAX_NEIGHBORS = 75;
        public static Particle[] liquid;
        public int[] activeParticles;
        public int numActiveParticles = 0;
        private int initializedParticleCount = 0;
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
        private Vector2 liquidGravity = new Vector2(0, 9.8f) / 3000;
        public static Vector2 LIQUID_ORIGIN = new Vector2(999999999f);
        private Dictionary<int, List<int>> collisionGridX;
        private List<int> collisionGridY;
        private Vector2[] simPositions;
        private Vector2[] simVelocities;
        private Vector2[] simDelta;
        private object applyForcesLock = new object();
        public bool debug = false;
        public bool drawCells = false;
        public bool drawVelocities = true;
        private float dt = 1f / 60f;
        //private Renderer renderer;
        public AABB simulationAABB;

        public int defaultPriority { get { return 10; } }
        public SystemType systemType { get { return SystemType.Fluid; } }

        public FluidSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _physicsSystem = (PhysicsSystem)_systemManager.getSystem(SystemType.Physics);
            _renderSystem = (RenderSystem)_systemManager.getSystem(SystemType.Render);
            liquid = new Particle[MAX_PARTICLES];
            activeParticles = new int[MAX_PARTICLES];
            simPositions = new Vector2[MAX_PARTICLES];
            simVelocities = new Vector2[MAX_PARTICLES];
            simDelta = new Vector2[MAX_PARTICLES];
            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                liquid[i] = new Particle(this, i, LIQUID_ORIGIN);
            }
        }

        public int getFluidGridX(float x) { return (int)Math.Floor(x / CELL_SPACING); }
        public int getFluidGridY(float y) { return (int)Math.Floor(y / CELL_SPACING); }

        // createParticle
        public void createParticle(Vector2 position)
        {
            Particle particle = liquid[initializedParticleCount];
            particle.position = position;
            particle.oldPosition = position;
            particle.velocity = Vector2.Zero;
            particle.alive = true;
            initializedParticleCount++;
        }

        // prepareSimulation
        private void prepareSimulation(int index)
        {
            // Neighbor search
            //liquid[index].findNeighbors();
            findNeighbors(index);

            // Reset influences
            liquid[index].actorInfluenceCount = 0;

            // Reset collision flags
            liquid[index].skipMovementUpdate = false;
            liquid[index].numFixturesToTest = 0;

            // Reset pressures
            liquid[index].p = 0;
            liquid[index].pnear = 0;

            // Simulation-specifc values
            simPositions[index] = liquid[index].position * MULTIPLIER;
            simVelocities[index] = liquid[index].velocity * MULTIPLIER;
            simDelta[index] = Vector2.Zero;

            // Store old position
            liquid[index].oldPosition = liquid[index].position;
        }

        // calculateForces
        private void calculateForces(int index)
        {
            // Current particle
            Particle particle = liquid[index];

            // Particle pressure calculated by particle proximity
            // Pressures = 0 if all particles within range are idealRad distance away
            for (int a = particle.neighborCount - 1; a >= 0; a--)
            {
                int neighborIndex = particle.neighbors[a];
                Particle neighbor = liquid[neighborIndex];
                particle.relativePosition[a] = simPositions[neighborIndex] - simPositions[particle.index];

                float vlensqr = particle.relativePosition[a].LengthSquared();
                //within idealRad check
                if (vlensqr < IDEAL_RADIUS_SQ)
                {
                    particle.distances[a] = (float)Math.Sqrt(vlensqr);
                    if (particle.distances[a] < Settings.b2_epsilon) particle.distances[a] = IDEAL_RADIUS - .01f;
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
        }

        // applyForces
        private Vector2[] applyForces(int index, Vector2[] accumulatedDelta)
        {
            // Current particle
            Particle particle = liquid[index];

            // Now actually apply the forces
            float pressure = (particle.p - GAS_CONSTANT) / 2.0F; //normal pressure term
            float pressureNear = particle.pnear / 2.0F; //near particles term
            pressure = pressure > MAX_PRESSURE ? MAX_PRESSURE : pressure;
            pressureNear = pressureNear > MAX_PRESSURE_NEAR ? MAX_PRESSURE_NEAR : pressureNear;
            for (int a = particle.neighborCount - 1; a >= 0; a--)
            {
                int neighborIndex = particle.neighbors[a];
                Particle neighbor = liquid[neighborIndex];

                if (particle.distances[a] < IDEAL_RADIUS)
                {
                    float factor = particle.oneminusq[a] * (pressure + pressureNear * particle.oneminusq[a]) / (2.0F * particle.distances[a]);
                    Vector2 d = particle.relativePosition[a] * factor;
                    Vector2 relativeVelocity = simVelocities[neighborIndex] - simVelocities[particle.index];
                    factor = VISCOSITY * particle.oneminusq[a] * dt;
                    d -= relativeVelocity * factor;

                    accumulatedDelta[neighborIndex] += d;
                    accumulatedDelta[particle.index] -= d;
                }
            }

            return accumulatedDelta;
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
                    particle.onScreen =
                        particle.position.X > simulationAABB.lowerBound.X &&
                        particle.position.X < simulationAABB.upperBound.X &&
                        particle.position.Y > simulationAABB.lowerBound.Y &&
                        particle.position.Y < simulationAABB.upperBound.Y;
                    if (particle.isActive())
                    {
                        activeParticles[numActiveParticles] = i;
                        numActiveParticles++;
                    }
                }
            }
        }

        // prepareCollisions
        private void prepareCollisions()
        {
            // Query the world using the screen's AABB
            _physicsSystem.world.QueryAABB((FixtureProxy fixtureProxy) =>
            {
                // Skip certain collisions
                //UserData data = fixtureProxy.fixture.GetBody().GetUserData() as UserData;
                //if (data.actorType == ActorType.WALL_GROUP || data.actorType == ActorType.GROUND)
                //    return true;

                AABB aabb = fixtureProxy.aabb;
                int Ax = getFluidGridX(aabb.lowerBound.X);
                int Ay = getFluidGridY(aabb.lowerBound.Y);
                int Bx = getFluidGridX(aabb.upperBound.X) + 1;
                int By = getFluidGridY(aabb.upperBound.Y) + 1;
                for (int i = Ax; i < Bx; i++)
                {
                    for (int j = Ay; j < By; j++)
                    {
                        if (fluidGrid.TryGetValue(i, out collisionGridX) && collisionGridX.TryGetValue(j, out collisionGridY))
                        {
                            for (int n = 0; n < collisionGridY.Count; n++)
                            {
                                Particle particle = liquid[collisionGridY[n]];
                                if (particle.numFixturesToTest < Particle.MAX_FIXTURES_TO_TEST)
                                {
                                    particle.fixturesToTest[particle.numFixturesToTest] = fixtureProxy.fixture;
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
        private void resolveCollision(int activeParticleIndex)
        {
            Particle particle = liquid[activeParticles[activeParticleIndex]];
            for (int i = 0; i < particle.numFixturesToTest; i++)
            {
                Fixture fixture = particle.fixturesToTest[i];
                if (fixture.GetShape() == null)     // fixtures can be destroyed before they're tested
                    continue;

                Vector2 newPosition = particle.position + particle.velocity + particle.delta;
                if (fixture.TestPoint(newPosition, 0.01f))
                {
                    Body body = fixture.GetBody();
                    //UserData data = body.GetUserData() as UserData;
                    //Actor actor = data.parent as Actor;
                    //Debug.Assert(actor != null);
                    bool influenceActor = true;

                    // Don't check for collisions against certain actors
                    //if (!(data.actorType == ActorType.WALL_GROUP || data.actorType == ActorType.GROUND ||
                    //    data.actorType == ActorType.GRENADE || data.actorType == ActorType.PLAYER ||
                    //    data.actorType == ActorType.ROPE_SEGMENT))
                    //{
                        // Continue with normal collisions
                        Vector2 closestPoint = Vector2.Zero;
                        Vector2 normal = Vector2.Zero;
                        //if (data.actorType == ActorType.GRAVITY_WELL)
                        //{
                            /*
                            // Gravity well
                            GravityWellActor gravityWellActor = actor as GravityWellActor;
                            Vector2 delta = gravityWellActor.body.GetPosition() - particle.position;
                            float distanceSq = delta.LengthSquared();
                            if (distanceSq != 0)
                            {
                                float scale = (gravityWellActor.radiusSq - distanceSq) / distanceSq;
                                scale = Math.Min(scale, gravityWellActor.strength);
                                Vector2 force = delta * gravityWellActor.strength * scale;

                                // Well force
                                particle.velocity += force * gravityWellActor.fluidForceScale;
                            }
                            influenceActor = false;
                            */
                        //}
                        //else if (data.actorType == ActorType.EXPLOSION)
                        //{
                            /*
                            // Explosion
                            ExplosionActor explosionActor = actor as ExplosionActor;
                            Vector2 delta = particle.position - explosionActor.body.GetPosition();
                            float distanceSq = delta.LengthSquared();
                            if (distanceSq != 0)
                            {
                                float scale = (explosionActor.radiusSq - distanceSq) / distanceSq;
                                Vector2 force = delta * Math.Min(explosionActor.strength * scale, explosionActor.strength);

                                // Explosion force
                                particle.velocity += force * explosionActor.fluidForceScale;
                            }
                            influenceActor = false;
                            */
                        //}
                        //else if (fixture.GetShape().ShapeType == ShapeType.Polygon)
                        if (fixture.GetShape().ShapeType == ShapeType.Polygon)
                        {
                            // Polygons
                            PolygonShape shape = fixture.GetShape() as PolygonShape;
                            body.GetTransform(out particle.collisionXF);

                            for (int v = 0; v < shape.GetVertexCount(); v++)
                            {
                                particle.collisionVertices[v] = MathUtils.Multiply(ref particle.collisionXF, shape.GetVertex(v));
                                particle.collisionNormals[v] = MathUtils.Multiply(ref particle.collisionXF.R, shape.GetNormal(v));
                            }

                            // Find closest edge
                            float shortestDistance = 9999999f;
                            for (int v = 0; v < shape.GetVertexCount(); v++)
                            {
                                float distance = Vector2.Dot(particle.collisionNormals[v], particle.collisionVertices[v] - particle.position);
                                if (distance < shortestDistance)
                                {
                                    shortestDistance = distance;
                                    closestPoint = particle.collisionNormals[v] * (distance) + particle.position;
                                    normal = particle.collisionNormals[v];
                                }
                            }
                            //Vector2 averagedNormal = (normal + (particle.position - newPosition) / 2);
                            //averagedNormal.Normalize();
                            particle.position = closestPoint + 0.05f * normal;
                            particle.skipMovementUpdate = true;
                            influenceActor = body.GetType() == BodyType.Dynamic;
                            //doAnotherCollisionCheck = true;
                        }
                        else if (fixture.GetShape().ShapeType == ShapeType.Circle)
                        {
                            // Circles
                            CircleShape shape = fixture.GetShape() as CircleShape;
                            Vector2 center = shape._p + body.GetPosition();
                            Vector2 difference = particle.position - center;
                            normal = difference;
                            normal.Normalize();
                            closestPoint = center + difference * (shape._radius / difference.Length());
                            particle.position = closestPoint + 0.05f * normal;
                            particle.skipMovementUpdate = true;
                            influenceActor = body.GetType() == BodyType.Dynamic;
                        }

                        // Update velocity
                        particle.oldVelocity = particle.velocity;
                        particle.velocity = (particle.velocity - 1.2f * Vector2.Dot(particle.velocity, normal) * normal) * 0.85f;

                        // Handle fast moving bodies
                        if (body.GetLinearVelocity().LengthSquared() > 50f)
                        {
                            Vector2 bodyVelocity = body.GetLinearVelocityFromWorldPoint(particle.oldPosition);
                            body.SetLinearVelocity(body.GetLinearVelocity() * 0.995f);
                            body.SetAngularVelocity(body.GetAngularVelocity() * 0.95f);
                            particle.velocity += bodyVelocity * 0.005f;
                        }
                    //}

                    /*
                    // Add actor to list of actors being influenced by this particle
                    if (influenceActor)
                    {
                        particle.actorsToInfluence[particle.actorInfluenceCount] = actor;
                        particle.actorInfluenceCount++;
                    }*/
                }
            }

            /*
            // Test for metamers
            int mx = GameEnvironment.getPlantGridX(particle.position.X);
            int my = GameEnvironment.getPlantGridY(particle.position.Y);
            Dictionary<int, List<Metamer>> plantGridX;
            List<Metamer> plantGridY;
            if (GameEnvironment.metamerGrid.TryGetValue(mx, out plantGridX) && plantGridX.TryGetValue(my, out plantGridY))
            {
                for (int i = 0; i < plantGridY.Count; i++)
                {
                    if (particle.actorInfluenceCount < Particle.MAX_INFLUENCES)
                    {
                        particle.actorsToInfluence[particle.actorInfluenceCount] = plantGridY[i];
                        particle.actorInfluenceCount++;
                        particle.velocity *= 0.95f;
                    }
                }
            }
            */
        }

        // killParticle
        private void killParticle(Particle particle)
        {
            particle.position = LIQUID_ORIGIN;
            particle.alive = false;
            particle.onScreen = false;
            numActiveParticles--;
        }

        // resetLiquid
        public void resetLiquid()
        {
            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                liquid[i].position = LIQUID_ORIGIN;
                liquid[i].oldPosition = LIQUID_ORIGIN;
                liquid[i].velocity = Vector2.Zero;
                liquid[i].oldVelocity = Vector2.Zero;
                liquid[i].alive = false;
                liquid[i].onScreen = false;
                liquid[i].actorInfluenceCount = 0;
                //liquid[i].update();
                updateParticle(i);
            }
            numActiveParticles = 0;
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

        // update
        public void updateParticle(int index)
        {
            Particle particle = liquid[index];

            /*
            // Influence actors
            for (int i = 0; i < actorInfluenceCount; i++)
                actorsToInfluence[i].handleParticleInfluence(this);
            */

            // Revert movement if off screen
            if (particle.position.X < simulationAABB.lowerBound.X ||
                particle.position.X > simulationAABB.upperBound.X ||
                particle.position.Y < simulationAABB.lowerBound.Y ||
                particle.position.Y > simulationAABB.upperBound.Y)
            {
                particle.position = particle.oldPosition;
                particle.velocity = Vector2.Zero;
            }

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
            // Update simulation AABB
            //if (!GameEnvironment.paused)
            //{
                Vector2 screenCenter = _renderSystem.screenCenter;
                float width = (_renderSystem.screenWidth / _renderSystem.scale) * 0.75f;
                float height = (_renderSystem.screenHeight / _renderSystem.scale);
                simulationAABB.lowerBound.X = -screenCenter.X - width;
                simulationAABB.upperBound.X = -screenCenter.X + width;
                simulationAABB.lowerBound.Y = -screenCenter.Y - height;
                simulationAABB.upperBound.Y = -screenCenter.Y + height;
            //}

            // Flag active particles
            flagActive();

            // Prepare simulation
            Parallel.For(0, numActiveParticles, i => { prepareSimulation(activeParticles[i]); });

            // Prepare collisions
            prepareCollisions();

            // Calculate liquid forces
            Parallel.For(0, numActiveParticles, i => { calculateForces(activeParticles[i]); });

            // Apply liquid forces
            Parallel.For(
                0,
                numActiveParticles,
                () => new Vector2[MAX_PARTICLES],
                (i, state, accumulatedDelta) => applyForces(activeParticles[i], accumulatedDelta),
                (accumulatedDelta) =>
                {
                    lock (applyForcesLock)
                    {
                        for (int i = numActiveParticles - 1; i >= 0; i--)
                        {
                            simDelta[activeParticles[i]] += accumulatedDelta[activeParticles[i]];
                            liquid[activeParticles[i]].delta = simDelta[activeParticles[i]] / MULTIPLIER;
                        }
                    }
                }
            );

            // Resolve collisions
            Parallel.For(0, numActiveParticles, i => resolveCollision(i));

            // Move particles
            Parallel.For(0, numActiveParticles, i =>
            {
                // Move particle
                Particle particle = liquid[activeParticles[i]];

                if (!particle.skipMovementUpdate)
                {
                    // Update velocity
                    particle.velocity += particle.delta + liquidGravity;

                    // Update position
                    particle.position += particle.delta;
                    particle.position += particle.velocity;
                }
            });

            // Update particles
            for (int i = numActiveParticles - 1; i >= 0; i--)
                updateParticle(activeParticles[i]);
                //liquid[activeParticles[i]].update();
        }
    }
}
