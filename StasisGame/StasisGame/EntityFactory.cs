using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using StasisCore;
using StasisCore.Models;
using StasisGame.Systems;
using StasisGame.Components;
using StasisGame.Managers;
using Poly2Tri;

namespace StasisGame
{
    public class EntityFactory
    {
        // RopeTarget -- Structure used in rope creation
        private struct RopeTarget
        {
            public Fixture fixture;
            public Vector2 localPoint;
            public bool success;
            public RopeTarget(Fixture fixture, Vector2 localPoint, bool success)
            {
                this.fixture = fixture;
                this.localPoint = localPoint;
                this.success = success;
            }
        };

        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private Random _ropeTextureRNG;
        private Dictionary<string, Dictionary<int, Dictionary<int, GateOutputComponent>>> _levelUidActorIdEntityIdGateComponentMap;     // key 1) level uid
                                                                                                                                        // key 2) actor id needing to be listened to
                                                                                                                                        // key 3) output gate's entity id
        private Dictionary<string, Dictionary<int, Dictionary<int, GateOutputComponent>>> _levelUidCircuitIdGateIdGateComponentMap;     // key 1) level uid
                                                                                                                                        // key 2) circuit actor id
                                                                                                                                        // key 3) gate id

        // Constructor
        public EntityFactory(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _ropeTextureRNG = new Random();
            _levelUidActorIdEntityIdGateComponentMap = new Dictionary<string, Dictionary<int, Dictionary<int, GateOutputComponent>>>();
            _levelUidCircuitIdGateIdGateComponentMap = new Dictionary<string, Dictionary<int, Dictionary<int, GateOutputComponent>>>();
        }

        // convertPolygonToCCW -- Take a list of polygon points and make sure they're wound in a counter clockwise direction
        private List<Vector2> convertPolygonToCCW(List<Vector2> points)
        {
            if (StasisMathHelper.isPolygonClockwise(points))
            {
                return points;
            }
            else
            {
                points.Reverse();
                return points;
            }
        }

        // reset -- Clears information used to create entities from the level's xml data. Used between level loads.
        public void reset()
        {
            _levelUidActorIdEntityIdGateComponentMap.Clear();
            _levelUidCircuitIdGateIdGateComponentMap.Clear();
        }

        // expandLevelBoundary
        private void expandLevelBoundary(Vector2 point)
        {
            ((LevelSystem)_systemManager.getSystem(SystemType.Level)).expandBoundary(point);
        }

        // createOutputGate
        public void createOutputGates(string levelUid, XElement data)
        {
            _levelUidActorIdEntityIdGateComponentMap.Clear();
            _levelUidCircuitIdGateIdGateComponentMap.Clear();

            if (!_levelUidActorIdEntityIdGateComponentMap.ContainsKey(levelUid))
            {
                _levelUidActorIdEntityIdGateComponentMap.Add(levelUid, new Dictionary<int, Dictionary<int, GateOutputComponent>>());
            }
            if (!_levelUidCircuitIdGateIdGateComponentMap.ContainsKey(levelUid))
            {
                _levelUidCircuitIdGateIdGateComponentMap.Add(levelUid, new Dictionary<int, Dictionary<int, GateOutputComponent>>());
            }

            foreach (XElement circuitActorData in (from element in data.Elements("Actor") where element.Attribute("type").Value == "Circuit" select element))
            {
                int circuitId = int.Parse(circuitActorData.Attribute("id").Value);

                foreach (XElement connectionData in (from element in circuitActorData.Elements("CircuitConnection") where element.Attribute("type").Value == "output" select element))
                {
                    int actorIdToListenTo = int.Parse(connectionData.Attribute("actor_id").Value);
                    int gateId = int.Parse(connectionData.Attribute("gate_id").Value);
                    GameEventType onEnabledEvent = (GameEventType)Loader.loadEnum(typeof(GameEventType), connectionData.Attribute("on_enabled_event"), 0);
                    GameEventType onDisabledEvent = (GameEventType)Loader.loadEnum(typeof(GameEventType), connectionData.Attribute("on_disabled_event"), 0);
                    int entityId = _entityManager.createEntity(levelUid);
                    GateOutputComponent gateOutputComponent = new GateOutputComponent();

                    gateOutputComponent.onEnabledEvent = onEnabledEvent;
                    gateOutputComponent.onDisabledEvent = onDisabledEvent;
                    gateOutputComponent.entityId = entityId;

                    if (!_levelUidActorIdEntityIdGateComponentMap[levelUid].ContainsKey(actorIdToListenTo))
                        _levelUidActorIdEntityIdGateComponentMap[levelUid].Add(actorIdToListenTo, new Dictionary<int, GateOutputComponent>());
                    if (!_levelUidActorIdEntityIdGateComponentMap[levelUid][actorIdToListenTo].ContainsKey(entityId))
                        _levelUidActorIdEntityIdGateComponentMap[levelUid][actorIdToListenTo].Add(entityId, gateOutputComponent);

                    if (!_levelUidCircuitIdGateIdGateComponentMap[levelUid].ContainsKey(circuitId))
                        _levelUidCircuitIdGateIdGateComponentMap[levelUid].Add(circuitId, new Dictionary<int, GateOutputComponent>());
                    if (!_levelUidCircuitIdGateIdGateComponentMap[levelUid][circuitId].ContainsKey(gateId))
                        _levelUidCircuitIdGateIdGateComponentMap[levelUid][circuitId][gateId] = gateOutputComponent;

                    _entityManager.addComponent(levelUid, entityId, gateOutputComponent);
                }
            }
        }

        // createGroundBody -- Creates a body that is used throughout the game as an anchor for joints (TODO: Might not be necessary with Farseer)
        public Body createGroundBody(string levelUid, World world)
        {
            int groundId = _entityManager.createEntity(levelUid, 10000);
            Body groundBody = BodyFactory.CreateBody(world, groundId);
            Fixture fixture;

            groundBody.BodyType = BodyType.Static;
            fixture = groundBody.CreateFixture(new CircleShape(0.1f, 1f));
            fixture.IsSensor = true;

            _entityManager.addComponent(levelUid, groundId, new GroundBodyComponent(groundBody));
            _entityManager.addComponent(levelUid, groundId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(levelUid, groundId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(levelUid, groundId, new SkipFluidResolutionComponent());

            return groundBody;
        }

        // createBoxTexture -- Creates a texture for boxes from a body and a material
        private Texture2D createBoxTexture(Body body, XElement data)
        {
            RenderSystem renderSystem = (RenderSystem)_systemManager.getSystem(SystemType.Render);
            Material material = new Material(ResourceManager.getResource(data.Attribute("material_uid").Value));
            Matrix angleTransform = Matrix.CreateRotationZ(body.Rotation);
            List<Vector2> materialVertices = new List<Vector2>();
            PolygonShape boxShape = body.FixtureList[0].Shape as PolygonShape;

            for (int i = 0; i < 4; i++)
                materialVertices.Add(Vector2.Transform(boxShape.Vertices[i], angleTransform));

            return renderSystem.materialRenderer.renderMaterial(material, materialVertices, 1f, false);
        }

        // createBoxRenderableTriangles -- Creates a list of renderable triangles for boxes
        private List<RenderableTriangle> createBoxRenderableTriangles(Body body)
        {
            List<RenderableTriangle> renderableTriangles = new List<RenderableTriangle>();
            PolygonShape polygonShape = body.FixtureList[0].Shape as PolygonShape;
            Matrix transform = Matrix.CreateRotationZ(body.Rotation);
            List<Vector2> points = new List<Vector2>();
            Vector2 topLeft;
            Vector2 bottomRight;
            List<PolygonPoint> P2TPoints = new List<PolygonPoint>();
            Polygon polygon;

            // Get rotated vertices
            for (int i = 0; i < 4; i++)
                points.Add(Vector2.Transform(polygonShape.Vertices[i], transform));

            // Find boundary
            topLeft = points[0];
            bottomRight = points[0];
            for (int i = 0; i < 4; i++)
            {
                topLeft = Vector2.Min(topLeft, points[i]);
                bottomRight = Vector2.Max(bottomRight, points[i]);
            }

            // Convert to Poly2Tri polygon
            for (int i = 0; i < 4; i++)
                P2TPoints.Add(new PolygonPoint(points[i].X, points[i].Y));
            polygon = new Polygon(P2TPoints);
            P2T.Triangulate(polygon);

            // Create renderable triangles
            foreach (DelaunayTriangle triangle in polygon.Triangles)
            {
                renderableTriangles.Add(new RenderableTriangle(triangle, topLeft, bottomRight, true, body.Rotation));
            }

            return renderableTriangles;
        }

        // createBox -- Creates a box from supplied data
        public void createBox(string levelUid, XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(levelUid, actorId);
            float layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0.1f);
            Body body;
            Fixture fixture;
            float density = Loader.loadFloat(data.Attribute("density"), 1f);
            PolygonShape boxShape = new PolygonShape(density);
            BodyType bodyType = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Static);
            Transform xf;
            PrimitivesRenderComponent bodyRenderComponent;
            Texture2D texture;
            List<Vector2> materialVertices = new List<Vector2>();
            List<RenderableTriangle> renderableTriangles;

            body = new Body(world, entityId);
            body.BodyType = bodyType;
            body.Position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            boxShape.SetAsBox(Loader.loadFloat(data.Attribute("half_width"), 1f), Loader.loadFloat(data.Attribute("half_height"), 1f));
            fixture = body.CreateFixture(boxShape);
            fixture.Friction = Loader.loadFloat(data.Attribute("friction"), 1f);
            fixture.Restitution = Loader.loadFloat(data.Attribute("restitution"), 1f);
            fixture.CollisionCategories = bodyType == BodyType.Dynamic ? (ushort)CollisionCategory.DynamicGeometry : (ushort)CollisionCategory.StaticGeometry;
            fixture.CollidesWith =
                (ushort)CollisionCategory.DynamicGeometry |
                (ushort)CollisionCategory.Player |
                (ushort)CollisionCategory.Rope |
                (ushort)CollisionCategory.StaticGeometry |
                (ushort)CollisionCategory.Item;

            // Create render component
            texture = createBoxTexture(body, data);
            renderableTriangles = createBoxRenderableTriangles(body);
            bodyRenderComponent = new PrimitivesRenderComponent(new PrimitiveRenderObject(texture, renderableTriangles, layerDepth));

            // Add components
            if (bodyType != BodyType.Static)
                _entityManager.addComponent(levelUid, entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Physical));
            _entityManager.addComponent(levelUid, entityId, bodyRenderComponent);
            _entityManager.addComponent(levelUid, entityId, new PhysicsComponent(body));
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(body.Position));

            // Expand level boundary
            body.GetTransform(out xf);
            for (int i = 0; i < boxShape.Vertices.Count; i++)
            {
                Vector2 point = MathUtils.Multiply(ref xf, boxShape.Vertices[i]);
                expandLevelBoundary(point);
            }
        }

        // createCircleTexture -- Creates a texture for a circle from a body and a material
        private Texture2D createCircleTexture(Body body, XElement data)
        {
            RenderSystem renderSystem = (RenderSystem)_systemManager.getSystem(SystemType.Render);
            List<Vector2> polygonPoints = new List<Vector2>();
            float segments = 64f;
            float increment = StasisMathHelper.pi2 / segments;
            float radius = body.FixtureList[0].Shape.Radius;
            Material material = new Material(ResourceManager.getResource(data.Attribute("material_uid").Value));

            for (float t = StasisMathHelper.pi; t > -StasisMathHelper.pi; t -= increment)
                polygonPoints.Add(new Vector2((float)Math.Cos(t), (float)Math.Sin(t)) * radius);

            return renderSystem.materialRenderer.renderMaterial(material, polygonPoints, 1f, false);
        }

        // createCircleRenderableTriangles -- Creates a list of renderable triangles from a circle
        private List<RenderableTriangle> createCircleRenderableTriangles(Body body)
        {
            float segments = 64f;
            float increment = StasisMathHelper.pi2 / segments;
            float radius = body.FixtureList[0].Shape.Radius;
            List<PolygonPoint> polygonPoints = new List<PolygonPoint>();
            Polygon polygon;
            List<RenderableTriangle> renderableTriangles = new List<RenderableTriangle>();
            Vector2 topLeft = new Vector2(-radius, -radius);
            Vector2 bottomRight = new Vector2(radius, radius);

            for (float t = StasisMathHelper.pi; t > -StasisMathHelper.pi; t -= increment)
            {
                Vector2 p = new Vector2((float)Math.Cos(t), (float)Math.Sin(t)) * radius;
                polygonPoints.Add(new PolygonPoint(p.X, p.Y));
            }
            polygon = new Polygon(polygonPoints);
            P2T.Triangulate(polygon);

            for (int i = 0; i < polygon.Triangles.Count; i++)
                renderableTriangles.Add(new RenderableTriangle(polygon.Triangles[i], topLeft, bottomRight));

            return renderableTriangles;
        }

        // createCircle -- Creates a circle entity
        public void createCircle(string levelUid, XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(levelUid, actorId);
            Body body;
            Fixture fixture;
            //BodyDef bodyDef = new BodyDef();
            //FixtureDef circleFixtureDef = new FixtureDef();
            CircleShape circleShape = new CircleShape(Loader.loadFloat(data.Attribute("radius"), 1f), Loader.loadFloat(data.Attribute("density"), 1f));
            BodyType bodyType = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Static);
            Texture2D texture;
            float layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0.1f);
            PrimitivesRenderComponent bodyRenderComponent;
            List<RenderableTriangle> renderableTriangles = new List<RenderableTriangle>();

            body = BodyFactory.CreateBody(world, Loader.loadVector2(data.Attribute("position"), Vector2.Zero), entityId);
            body.BodyType = bodyType;
            fixture = body.CreateFixture(circleShape);
            fixture.Friction = Loader.loadFloat(data.Attribute("friction"), 1f);
            fixture.Restitution = Loader.loadFloat(data.Attribute("restitution"), 1f);
            fixture.CollisionCategories = bodyType == BodyType.Dynamic ? (ushort)CollisionCategory.DynamicGeometry : (ushort)CollisionCategory.StaticGeometry;
            fixture.CollidesWith =
                (ushort)CollisionCategory.DynamicGeometry |
                (ushort)CollisionCategory.Player |
                (ushort)CollisionCategory.Rope |
                (ushort)CollisionCategory.StaticGeometry |
                (ushort)CollisionCategory.Item;

            // Create body render component
            texture = createCircleTexture(body, data);
            renderableTriangles = createCircleRenderableTriangles(body);
            bodyRenderComponent = new PrimitivesRenderComponent(new PrimitiveRenderObject(texture, renderableTriangles, layerDepth));

            // Add components
            if (bodyType != BodyType.Static)
                _entityManager.addComponent(levelUid, entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Physical));
            _entityManager.addComponent(levelUid, entityId, new PhysicsComponent(body));
            _entityManager.addComponent(levelUid, entityId, bodyRenderComponent);
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(body.Position));

            // Expand level boundary
            expandLevelBoundary(body.Position + new Vector2(-circleShape.Radius, -circleShape.Radius));
            expandLevelBoundary(body.Position + new Vector2(circleShape.Radius, circleShape.Radius));
        }

        public void createFluid(string levelUid, XElement data)
        {
            FluidSystem fluidSystem = (FluidSystem)_systemManager.getSystem(SystemType.Fluid);
            List<Vector2> polygonPoints = new List<Vector2>();

            foreach (XElement pointData in data.Elements("Point"))
                polygonPoints.Add(Loader.loadVector2(pointData, Vector2.Zero));
            fluidSystem.createFluidBody(polygonPoints);

            // Expand level boundary
            foreach (Vector2 point in polygonPoints)
                expandLevelBoundary(point);
        }

        // createWorldItem -- Create an item that exists in the world (as opposed to an item in the inventory)
        public void createWorldItem(string levelUid, XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            RenderSystem renderSystem = _systemManager.getSystem(SystemType.Render) as RenderSystem;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(levelUid, actorId);
            string itemUID = data.Attribute("item_uid").Value;
            Body body;
            Fixture fixture;
            PolygonShape shape = new PolygonShape(Loader.loadFloat(data.Attribute("density"), 1f));
            XElement itemData = ResourceManager.getResource(itemUID);
            Texture2D worldTexture = ResourceManager.getTexture(Loader.loadString(itemData.Attribute("world_texture_uid"), "default_item"));
            Texture2D inventoryTexture = ResourceManager.getTexture(Loader.loadString(itemData.Attribute("inventory_texture_uid"), "default_item"));
            float layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0.1f);
            ItemDefinition itemDefinition = DataManager.itemManager.getItemDefinition(itemUID);
            ItemState itemState = new ItemState(int.Parse(data.Attribute("quantity").Value), float.Parse(data.Attribute("current_range_limit").Value), true);

            body = BodyFactory.CreateBody(world, Loader.loadVector2(data.Attribute("position"), Vector2.Zero), entityId);
            body.BodyType = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Dynamic);
            body.Rotation = Loader.loadFloat(data.Attribute("angle"), 0f);
            shape.SetAsBox(Loader.loadFloat(data.Attribute("half_width"), 0.4f), Loader.loadFloat(data.Attribute("half_height"), 0.4f));
            fixture = body.CreateFixture(shape);
            fixture.Friction = Loader.loadFloat(data.Attribute("friction"), 1f);
            fixture.Restitution = Loader.loadFloat(data.Attribute("restitution"), 0f);
            fixture.CollisionCategories = (ushort)CollisionCategory.Item;
            fixture.CollidesWith =
                (ushort)CollisionCategory.DynamicGeometry |
                (ushort)CollisionCategory.Player |
                (ushort)CollisionCategory.Rope |
                (ushort)CollisionCategory.StaticGeometry |
                (ushort)CollisionCategory.Explosion;

            // Add components
            _entityManager.addComponent(levelUid, entityId, new ItemComponent(itemDefinition, itemState, ResourceManager.getTexture(itemDefinition.inventoryTextureUid)));
            _entityManager.addComponent(levelUid, entityId, new PhysicsComponent(body));
            _entityManager.addComponent(levelUid, entityId, new PrimitivesRenderComponent(renderSystem.createSpritePrimitiveObject(worldTexture, body.Position, new Vector2(worldTexture.Width, worldTexture.Height) / 2f, body.Rotation, 1f, layerDepth)));
            _entityManager.addComponent(levelUid, entityId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(body.Position));
        }

        // findRopeTarget -- Tries to find a rope target by raycasting
        private RopeTarget findRopeTarget(Vector2 a, Vector2 b)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            RopeTarget ropeTarget = new RopeTarget();
            float lowestFraction = 999999f;

            world.RayCast((fixture, point, normal, fraction) =>
                {
                    int fixtureEntityId = (int)fixture.Body.UserData;
                    if (_entityManager.getComponent(fixtureEntityId, ComponentType.IgnoreRopeRaycast) != null)
                        return -1;
                    else if (_entityManager.getComponent(fixtureEntityId, ComponentType.Tree) != null)
                        return -1;  // the only bodies that exist on a tree are already supporting a rope, and will be destroyed along with the rope that created it

                    if (fraction < lowestFraction)
                    {
                        lowestFraction = fraction;
                        ropeTarget.fixture = fixture;
                        ropeTarget.localPoint = fixture.Body.GetLocalPoint(ref point);
                        ropeTarget.success = true;
                    }
                    return fraction;
                },
                a,
                b);
            return ropeTarget;
        }

        // findWall -- Tries to find a wall rope target using QueryAABB
        private Fixture findWall(Vector2 position)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            AABB aabb = new AABB();
            Fixture wallFixture = null;

            aabb.LowerBound = position;
            aabb.UpperBound = position;
            world.QueryAABB((fixture) =>
                {
                    if (fixture.TestPoint(ref position, 0f))
                    {
                        int fixtureEntityId = (int)fixture.Body.UserData;
                        WallComponent wallComponent = (WallComponent)_entityManager.getComponent(fixtureEntityId, ComponentType.Wall);

                        if (wallComponent != null)
                        {
                            wallFixture = fixture;
                            return false;   // Don't need to find all wall fixtures, one is enough.
                        }
                    }
                    return true;
                },
                ref aabb);
            return wallFixture;
        }

        // createRopeNodes -- Creates a linked list of physical bodies and joints that make up the rope
        private RopeNode createRopeNodes(Vector2 a, Vector2 b, bool collidesWithPlayer)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            Vector2 relative = b - a;
            Vector2 ropeNormal = Vector2.Normalize(relative);
            float angle = (float)Math.Atan2(relative.Y, relative.X);
            float segmentLength = 0.5f;
            float segmentHalfLength = segmentLength * 0.5f;
            int ropeNodeLimit = (int)Math.Ceiling(relative.Length() / segmentLength);
            RopeNode head = null;
            RopeNode previousNode = null;

            if (ropeNodeLimit == 0)
                return null;

            for (int i = 0; i < ropeNodeLimit; i++)
            {
                PolygonShape shape = new PolygonShape(0.5f);
                Body body = BodyFactory.CreateBody(world);
                Fixture fixture;
                RopeNode ropeNode;
                RevoluteJoint joint = null;

                // Create body
                body.Rotation = angle + StasisMathHelper.pi; // Adding pi fixes a problem where rope segments are created backwards, and then snap into the correct positions
                body.Position = a + ropeNormal * (segmentHalfLength + i * segmentLength);
                body.BodyType = BodyType.Dynamic;
                shape.SetAsBox(segmentHalfLength, 0.15f);
                fixture = body.CreateFixture(shape);
                fixture.Friction = 0.5f;
                fixture.Restitution = 0f;
                fixture.CollisionCategories = (ushort)CollisionCategory.Rope;
                fixture.CollidesWith =
                    (ushort)CollisionCategory.DynamicGeometry |
                    (ushort)CollisionCategory.Item |
                    (ushort)CollisionCategory.StaticGeometry |
                    (ushort)CollisionCategory.Explosion;
                if (collidesWithPlayer)
                    fixture.CollidesWith |= (ushort)CollisionCategory.Player;
                fixture.UserData = i;

                // Create joints
                if (previousNode != null)
                    joint = JointFactory.CreateRevoluteJoint(world, previousNode.body, body, new Vector2(-segmentHalfLength, 0), new Vector2(segmentHalfLength, 0));

                // Create node
                ropeNode = new RopeNode(body, joint, segmentHalfLength);

                // Store references to head and tail nodes, and insert the node into the linked list
                if (head == null)
                    head = ropeNode;
                if (!(previousNode == null))
                    previousNode.insert(ropeNode);
                previousNode = ropeNode;
            }
            return head;
        }

        // createAnchor -- Creates a revolute joint between a node and its anchoring fixture
        // TODO: Move this to RopeSystem, since player will be able to pick ropes up and attach them to things?
        private void createAnchor(RopeNode ropeNode, Vector2 ropeNodeLocalAnchor, Fixture fixture, Vector2 fixtureLocalAnchor)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;

            ropeNode.anchorJoint = JointFactory.CreateRevoluteJoint(world, ropeNode.body, fixture.Body, ropeNodeLocalAnchor, fixtureLocalAnchor);
        }

        // initializeRopeMaterial -- Loops through the rope nodes and initializes all of the information required to render the rope nodes
        private void initializeRopeMaterial(RopeNode head, RopeMaterial ropeMaterial)
        {
            RopeNode currentNode = head;

            while (currentNode != null)
            {
                currentNode.ropeNodeTextures = new RopeNodeTexture[ropeMaterial.interpolationCount];

                if (ropeMaterial.ropeTextureStyle == RopeTextureStyle.Random)
                {
                    for (int j = 0; j < ropeMaterial.interpolationCount; j++)
                    {
                        RopeMaterialTexture randomRopeMaterialTexture = ropeMaterial.textures[_ropeTextureRNG.Next(ropeMaterial.textures.Count)];
                        currentNode.ropeNodeTextures[j] = new RopeNodeTexture(randomRopeMaterialTexture.texture, randomRopeMaterialTexture.center, randomRopeMaterialTexture.angleOffset);
                    }
                }
                else if (ropeMaterial.ropeTextureStyle == RopeTextureStyle.Sequential)
                {
                    for (int j = 0; j < ropeMaterial.interpolationCount; j++)
                    {
                        RopeMaterialTexture sequentialRopeMaterialTexture = ropeMaterial.textures[Math.Min(j, ropeMaterial.textures.Count - 1)];
                        currentNode.ropeNodeTextures[j] = new RopeNodeTexture(sequentialRopeMaterialTexture.texture, sequentialRopeMaterialTexture.center, sequentialRopeMaterialTexture.angleOffset);
                    }
                }

                currentNode = currentNode.next;
            }
        }

        // finalizeRopeNodes -- Give rope nodes a reference to a ropeComponent and their bodies an entityId
        private void finalizeRopeNodes(RopeNode head, int entityId, RopeComponent ropeComponent)
        {
            RopeNode current = head;
            while (current != null)
            {
                current.body.UserData = entityId;
                current.ropeComponent = ropeComponent;
                current = current.next;
            }
        }

        // createRope -- Creates a rope from xml information loaded from level data.
        public int createRope(string levelUid, XElement data)
        {
            RopeMaterial ropeMaterial = new RopeMaterial(ResourceManager.getResource(data.Attribute("rope_material_uid").Value));
            bool doubleAnchor = Loader.loadBool(data.Attribute("double_anchor"), false);
            Vector2 a = Loader.loadVector2(data.Attribute("point_b"), Vector2.Zero);    // For single-anchor ropes, the anchor is created at point B, so I'm swapping the values here because the editor assumes point A is the anchor.. TODO: fix this?
            Vector2 b = Loader.loadVector2(data.Attribute("point_a"), Vector2.Zero);
            int entityId = Loader.loadInt(data.Attribute("id"), -1);

            if (doubleAnchor)
                entityId = createDoubleAnchorRope(levelUid, a, b, ropeMaterial, entityId);
            else
                entityId = createSingleAnchorRope(levelUid, a, b, ropeMaterial, false, entityId);

            return entityId;
        }

        // createSingleAnchorRope -- Creates a rope with one anchor
        public int createSingleAnchorRope(string levelUid, Vector2 a, Vector2 b, RopeMaterial ropeMaterial, bool destroyAfterRelease, int entityId = -1)
        {
            /*
             * - Raycast from A to B
             *   - If valid fixture found, store result
             *   - Otherwise, test metamers
             *     - If metamer found, create limb body and store result
             *     - Otherwise, test walls
             *       - If wall found, store result
             *       - Otherwise, abort rope creation
             *   - If found a valid rope target, create rope
             *   - Otherwise, abort creation
             */
            RopeComponent ropeComponent;
            RopeTarget ropeTarget = findRopeTarget(a, b);
            RopeNode head;
            RopeNode tail;

            if (!ropeTarget.success)
            {
                // Test for metamers
                TreeSystem treeSystem = (_systemManager.getSystem(SystemType.Tree) as TreeSystem);
                Metamer metamer = treeSystem.findMetamer(b);

                if (metamer != null)
                {
                    // Create limb body
                    metamer.createLimbBody();
                    metamer.anchorCount++;

                    // Store result
                    ropeTarget.fixture = metamer.body.FixtureList[0];
                    ropeTarget.localPoint = Vector2.Zero;
                    ropeTarget.success = true;
                }
                else
                {
                    // Test for walls
                    Fixture fixture = findWall(b);

                    if (fixture != null)
                    {
                        // Store results
                        ropeTarget.fixture = fixture;
                        ropeTarget.localPoint = fixture.Body.GetLocalPoint(ref b);
                        ropeTarget.success = true;
                    }
                }
            }

            // Abort creation if no valid targets found
            if (!ropeTarget.success)
                return -1;

            // Create rope
            head = createRopeNodes(a, ropeTarget.fixture.Body.GetWorldPoint(ref ropeTarget.localPoint), false);
            if (head == null)
                return -1;
            tail = head.tail;
            createAnchor(tail, new Vector2(-tail.halfLength, 0), ropeTarget.fixture, ropeTarget.localPoint);
            initializeRopeMaterial(head, ropeMaterial);
            entityId = entityId == -1 ? _entityManager.createEntity(levelUid) : _entityManager.createEntity(levelUid, entityId);

            // Add components
            ropeComponent = new RopeComponent(head, ropeMaterial.interpolationCount, destroyAfterRelease, false, false);
            _entityManager.addComponent(levelUid, entityId, ropeComponent);
            _entityManager.addComponent(levelUid, entityId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(levelUid, entityId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(levelUid, entityId, new SkipFluidResolutionComponent());
            _entityManager.addComponent(levelUid, entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Rope));

            // Finalize rope properties by giving bodies an entityId and a reference to the ropeComponent
            finalizeRopeNodes(head, entityId, ropeComponent);

            return entityId;
        }

        // createDoubleAnchorRope -- Creates a rope with two anchors
        public int createDoubleAnchorRope(string levelUid, Vector2 a, Vector2 b, RopeMaterial ropeMaterial, int entityId = -1)
        {
            /*
             * - Create midpoint 'c'
             * - Raycast from C to A
             *   - If a valid fixture is found, store result
             *   - Otherwise, test metamers
             *     - If a valid metamer is found, create limb and store result
             *     - Otherwise, test walls
             *       - If a valid wall fixture is found, store result
             *       - Otherwise, abort
             * - If no result so far, abort
             * - Otherwise, repeat process above from C to B
             * - ...
             * - If less than two results, abort
             * - Otherwise, create rope
             */
            RopeTarget ropeTargetA;
            RopeTarget ropeTargetB;
            RopeNode head;
            RopeNode tail;
            Vector2 c = (a + b) / 2f;
            RopeComponent ropeComponent;

            // C to A
            ropeTargetA = findRopeTarget(c, a);
            if (!ropeTargetA.success)
            {
                // Test metamers
                Metamer metamer = (_systemManager.getSystem(SystemType.Tree) as TreeSystem).findMetamer(a);

                if (metamer != null)
                {
                    // Create metamer limb
                    metamer.createLimbBody();
                    metamer.anchorCount++;

                    // Store result
                    ropeTargetA.fixture = metamer.body.FixtureList[0];
                    ropeTargetA.localPoint = Vector2.Zero;
                    ropeTargetA.success = true;
                }
                else
                {
                    // Test walls
                    Fixture wallFixture = findWall(a);

                    if (wallFixture != null)
                    {
                        ropeTargetA.fixture = wallFixture;
                        ropeTargetA.localPoint = wallFixture.Body.GetLocalPoint(ref a);
                        ropeTargetA.success = true;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }

            // C to B
            ropeTargetB = findRopeTarget(c, b);
            if (!ropeTargetB.success)
            {
                // Test metamers
                Metamer metamer = (_systemManager.getSystem(SystemType.Tree) as TreeSystem).findMetamer(b);

                if (metamer != null)
                {
                    // Create metamer limb
                    metamer.createLimbBody();
                    metamer.anchorCount++;

                    // Store result
                    ropeTargetB.fixture = metamer.body.FixtureList[0];
                    ropeTargetB.localPoint = Vector2.Zero;
                    ropeTargetB.success = true;
                }
                else
                {
                    // Test walls
                    Fixture wallFixture = findWall(b);

                    if (wallFixture != null)
                    {
                        ropeTargetB.fixture = wallFixture;
                        ropeTargetB.localPoint = wallFixture.Body.GetLocalPoint(ref b);
                        ropeTargetB.success = true;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }

            // Create rope
            head = createRopeNodes(
                ropeTargetA.fixture.Body.GetWorldPoint(ref ropeTargetA.localPoint),
                ropeTargetB.fixture.Body.GetWorldPoint(ref ropeTargetB.localPoint),
                true);
            if (head == null)
                return -1;
            tail = head.tail;
            createAnchor(head, new Vector2(head.halfLength, 0), ropeTargetA.fixture, ropeTargetA.localPoint);
            createAnchor(tail, new Vector2(-tail.halfLength, 0), ropeTargetB.fixture, ropeTargetB.localPoint);
            initializeRopeMaterial(head, ropeMaterial);
            entityId = entityId == -1 ? _entityManager.createEntity(levelUid) : _entityManager.createEntity(levelUid, entityId);

            // Add components
            ropeComponent = new RopeComponent(head, ropeMaterial.interpolationCount, false, false, true);
            _entityManager.addComponent(levelUid, entityId, ropeComponent);
            _entityManager.addComponent(levelUid, entityId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(levelUid, entityId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(levelUid, entityId, new SkipFluidResolutionComponent());
            _entityManager.addComponent(levelUid, entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Rope));

            // Finalize rope properties by giving bodies an entityId and a reference to the ropeComponent
            finalizeRopeNodes(head, entityId, ropeComponent);

            return entityId;
        }

        // recreateRope -- Creates a new rope entity from an existing segment of rope nodes
        public int recreateRope(string levelUid, RopeNode head, int interpolationCount)
        {
            int entityId = _entityManager.createEntity(levelUid);
            RopeComponent ropeComponent = new RopeComponent(head, interpolationCount, head.ropeComponent.destroyAfterRelease, head.ropeComponent.reverseClimbDirection, head.ropeComponent.doubleAnchor);
            RopeNode current = head;

            // Update rope nodes settings
            finalizeRopeNodes(head, entityId, ropeComponent);

            // Add components
            _entityManager.addComponent(levelUid, entityId, ropeComponent);
            _entityManager.addComponent(levelUid, entityId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(levelUid, entityId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(levelUid, entityId, new SkipFluidResolutionComponent());
            _entityManager.addComponent(levelUid, entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Rope));

            return entityId;
        }

        // createTerrainTexture -- Creates a terrain texture
        private Texture2D createTerrainTexture(List<Vector2> points, XElement data)
        {
            Material material = new Material(ResourceManager.getResource(data.Attribute("material_uid").Value));
            RenderSystem renderSystem = (RenderSystem)_systemManager.getSystem(SystemType.Render);

            return renderSystem.materialRenderer.renderMaterial(material, points, 1f, false);
        }

        // createTerrainRenderableTriangles -- Creates a list of renderable triangles from terrain fixtures
        private List<RenderableTriangle> createTerrainRenderableTriangles(List<Vector2> points, Body body)
        {
            List<RenderableTriangle> renderableTriangles = new List<RenderableTriangle>();
            Vector2 topLeft = points[0];
            Vector2 bottomRight = points[0];

            for (int i = 0; i < points.Count; i++)
            {
                topLeft = Vector2.Min(topLeft, points[i]);
                bottomRight = Vector2.Max(bottomRight, points[i]);
            }

            for (int i = 0; i < body.FixtureList.Count; i++)
            {
                Fixture fixture = body.FixtureList[i];
                RenderableTriangle renderableTriangle = new RenderableTriangle(fixture, topLeft, bottomRight);

                renderableTriangle.fixture = fixture;
                renderableTriangles.Add(renderableTriangle);
            }

            return renderableTriangles;
        }

        // createTerrain -- Creates a terrain entity
        public void createTerrain(string levelUid, XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(levelUid, actorId);
            bool isWall = Loader.loadBool(data.Attribute("wall"), false);
            float layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0.1f);
            List<Vector2> points = new List<Vector2>();
            List<PolygonPoint> P2TPoints = new List<PolygonPoint>();
            Polygon polygon;
            Vector2 center = Vector2.Zero;
            Body body = BodyFactory.CreateBody(world, entityId);
            BodyType bodyType = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Static);
            bool isDestructible = Loader.loadBool(data.Attribute("destructible"), false);
            float density = Loader.loadFloat(data.Attribute("density"), 1f);
            float chunkSpacingX = Loader.loadFloat(data.Attribute("chunk_spacing_x"), 0.5f);
            float chunkSpacingY = Loader.loadFloat(data.Attribute("chunk_spacing_y"), 0.5f);
            float chunkJitterX = Loader.loadFloat(data.Attribute("chunk_jitter_x"), 0.2f);
            float chunkJitterY = Loader.loadFloat(data.Attribute("chunk_jitter_y"), 0.2f);
            float averageChunkSize = (chunkSpacingX + chunkSpacingY) / 2f;
            Random rng = new Random(Loader.loadInt(data.Attribute("destructible_seed"), 12345));
            Texture2D texture;
            List<RenderableTriangle> renderableTriangles;
            PrimitivesRenderComponent bodyRenderComponent;

            body.BodyType = bodyType;
            body.UserData = entityId;

            // Load points
            foreach (XElement pointData in data.Elements("Point"))
                points.Add(Loader.loadVector2(pointData, Vector2.Zero));

            // Convert to counter clockwise polygon
            points = convertPolygonToCCW(points);

            // Calculate center (average)
            foreach (Vector2 point in points)
                center += point / points.Count;

            // Adjust points to account for body position
            for (int i = 0; i < points.Count; i++)
                points[i] -= center;

            // Convert edge points to PolygonPoints
            for (int i = 0; i < points.Count; i++)
            {
                // Edge lines are broken into multiple smaller lines to prevent shards from forming
                if (i > 0)
                {
                    float distance = Vector2.Distance(points[i], points[i - 1]);
                    int lengthSegments = isDestructible ? (int)Math.Ceiling(distance / averageChunkSize) : 1;
                    if (lengthSegments > 0)
                    {
                        for (int x = 0; x < lengthSegments - 1; x++)
                        {
                            float factor = ((float)x + 1f) / (float)lengthSegments;
                            Vector2 lerp = Vector2.Lerp(points[i - 1], points[i], factor);
                            P2TPoints.Add(new PolygonPoint(lerp.X, lerp.Y));
                        }
                    }
                }
                P2TPoints.Add(new PolygonPoint(points[i].X, points[i].Y));
            }

            // Close loop (line is also broken into multiple smaller segments)
            float gapDistance = Vector2.Distance(points[0], points[points.Count - 1]);
            int gapLengthSegments = isDestructible ? (int)Math.Ceiling(gapDistance / averageChunkSize) : 1;
            if (gapLengthSegments > 0)
            {
                for (int x = 0; x < gapLengthSegments - 1; x++)
                {
                    float factor = ((float)x + 1f) / (float)gapLengthSegments;
                    Vector2 lerp = Vector2.Lerp(points[points.Count - 1], points[0], factor);
                    P2TPoints.Add(new PolygonPoint(lerp.X, lerp.Y));
                }
            }

            // Create polygon
            polygon = new Polygon(P2TPoints);

            // Create grid points
            if (isDestructible)
            {
                float width = (float)polygon.BoundingBox.Width;
                float height = (float)polygon.BoundingBox.Height;
                List<TriangulationPoint> newPoints = new List<TriangulationPoint>();
                for (float i = 0f; i < width; i += chunkSpacingX)
                {
                    for (float j = 0f; j < height; j += chunkSpacingY)
                    {
                        Vector2 jitter = new Vector2(
                            StasisMathHelper.floatBetween(-chunkJitterX, chunkJitterX, rng),
                            StasisMathHelper.floatBetween(-chunkJitterY, chunkJitterY, rng));
                        PolygonPoint point = new PolygonPoint(polygon.BoundingBox.Left + i + jitter.X, polygon.BoundingBox.Bottom + j + jitter.Y);
                        if (polygon.IsPointInside(point))
                            newPoints.Add(point);
                    }
                }
                polygon.AddSteinerPoints(newPoints);
            }

            // Triangulate polygon
            P2T.Triangulate(polygon);

            // Create fixtures out of triangles
            foreach (DelaunayTriangle triangle in polygon.Triangles)
            {
                PolygonShape shape = new PolygonShape(density);
                Vertices vertices = new Vertices(3);
                Fixture fixture;
                TriangulationPoint trianglePoint;

                vertices.Add(new Vector2(triangle.Points[0].Xf, triangle.Points[0].Yf));
                trianglePoint = triangle.PointCCWFrom(triangle.Points[0]);
                vertices.Add(new Vector2(trianglePoint.Xf, trianglePoint.Yf));
                trianglePoint = triangle.PointCCWFrom(trianglePoint);
                vertices.Add(new Vector2(trianglePoint.Xf, trianglePoint.Yf));
                shape.Set(vertices);
                fixture = body.CreateFixture(shape);
                fixture.Friction = Loader.loadFloat(data.Attribute("friction"), 1f);
                fixture.Restitution = Loader.loadFloat(data.Attribute("restitution"), 0f);
                fixture.CollisionCategories = bodyType == BodyType.Dynamic ? (ushort)CollisionCategory.DynamicGeometry : (ushort)CollisionCategory.StaticGeometry;
                if (isWall)
                {
                    fixture.CollidesWith = (ushort)CollisionCategory.Explosion;
                }
                else
                {
                    fixture.CollidesWith =
                        (ushort)CollisionCategory.DynamicGeometry |
                        (ushort)CollisionCategory.Item |
                        (ushort)CollisionCategory.Player |
                        (ushort)CollisionCategory.Rope |
                        (ushort)CollisionCategory.StaticGeometry |
                        (ushort)CollisionCategory.Explosion;
                }
            }
            body.Position = center;

            // Create render component
            texture = createTerrainTexture(points, data);
            renderableTriangles = createTerrainRenderableTriangles(points, body);
            bodyRenderComponent = new PrimitivesRenderComponent(new PrimitiveRenderObject(texture, renderableTriangles, layerDepth));

            // Add components
            if (isWall)
            {
                _entityManager.addComponent(levelUid, entityId, new IgnoreRopeRaycastComponent());
                _entityManager.addComponent(levelUid, entityId, new WallComponent());
                _entityManager.addComponent(levelUid, entityId, new SkipFluidResolutionComponent());
            }

            if (bodyType != BodyType.Static)
                _entityManager.addComponent(levelUid, entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Physical));

            if (isDestructible)
                _entityManager.addComponent(levelUid, entityId, new DestructibleGeometryComponent());
            _entityManager.addComponent(levelUid, entityId, new PhysicsComponent(body));
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(levelUid, entityId, bodyRenderComponent);
            _entityManager.addComponent(levelUid, entityId, new IgnoreTreeCollisionComponent());

            // Expand level boundary
            foreach (Vector2 point in points)
                expandLevelBoundary(point);
        }

        public void createTree(string levelUid, XElement data)
        {
            RenderSystem renderSystem = _systemManager.getSystem(SystemType.Render) as RenderSystem;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(levelUid, actorId);
            Material barkMaterial = new Material(ResourceManager.getResource(data.Attribute("bark_material_uid").Value));
            Material leafMaterial = new Material(ResourceManager.getResource(data.Attribute("leaf_material_uid").Value));
            List<Vector2> barkPoints = new List<Vector2>();
            List<Vector2> maxLeafPoints = new List<Vector2>();
            Texture2D barkTexture;
            List<List<Texture2D>> leafTextures = new List<List<Texture2D>>();
            Tree tree;
            float maxBaseHalfWidth = Loader.loadFloat(data.Attribute("max_base_half_width"), 0.5f);
            float internodeHalfLength = Loader.loadFloat(data.Attribute("internode_half_length"), 0.5f);
            float leafRange = 1f / (float)TreeSystem.NUM_LEAF_GROUPS;  // 1f / numSizes

            // Bark texture
            barkPoints.Add(new Vector2(-maxBaseHalfWidth, -internodeHalfLength));
            barkPoints.Add(new Vector2(-maxBaseHalfWidth, internodeHalfLength));
            barkPoints.Add(new Vector2(maxBaseHalfWidth, internodeHalfLength));
            barkPoints.Add(new Vector2(maxBaseHalfWidth, -internodeHalfLength));
            barkTexture = renderSystem.materialRenderer.renderMaterial(barkMaterial, barkPoints, 1f, false);

            // Leaf textures
            maxLeafPoints.Add(new Vector2(-256f, -256f) / renderSystem.scale);
            maxLeafPoints.Add(new Vector2(-256f, 256f) / renderSystem.scale);
            maxLeafPoints.Add(new Vector2(256f, 256f) / renderSystem.scale);
            maxLeafPoints.Add(new Vector2(256f, -256f) / renderSystem.scale);
            for (int i = 0; i < TreeSystem.NUM_LEAF_VARIATIONS; i++)
            {
                List<Texture2D> insideList = new List<Texture2D>();
                leafTextures.Add(insideList);
                for (float r = leafRange; r < 1f; r += leafRange)
                {
                    insideList.Add(renderSystem.materialRenderer.renderMaterial(leafMaterial, maxLeafPoints, r, true));
                }
                insideList.Add(renderSystem.materialRenderer.renderMaterial(leafMaterial, maxLeafPoints, 1f, true));
            }

            tree = new Tree(_systemManager.getSystem(SystemType.Tree) as TreeSystem, barkTexture, leafTextures, data);  // also expands level boundary

            // Handle initial iterations
            while ((int)tree.age > tree.iterations)
            {
                // Iterate
                tree.iterate(1);

                // Relax if on last iteration
                if ((int)tree.age == tree.iterations)
                {
                    for (int r = 0; r < 300; r++)
                        tree.step();
                }
            }

            // Add components
            _entityManager.addComponent(levelUid, entityId, new TreeComponent(tree));
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(tree.position));
        }

        // createRevoluteJoint
        public void createRevoluteJoint(string levelUid, XElement data)
        {
            EventSystem eventSystem = _systemManager.getSystem(SystemType.Event) as EventSystem;
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId;
            GroundBodyComponent groundBodyComponent = _entityManager.getComponents<GroundBodyComponent>(levelUid, ComponentType.GroundBody)[0];
            Vector2 jointWorldPosition = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            //RevoluteJointDef jointDef = new RevoluteJointDef();
            Body bodyA = null;
            Body bodyB = null;
            float lowerLimit = float.Parse(data.Attribute("lower_angle").Value);
            float upperLimit = float.Parse(data.Attribute("upper_angle").Value);
            bool enableLimit = bool.Parse(data.Attribute("enable_limit").Value);
            float maxMotorTorque = float.Parse(data.Attribute("max_motor_torque").Value);
            float motorSpeed = float.Parse(data.Attribute("motor_speed").Value);
            bool enableMotor = bool.Parse(data.Attribute("enable_motor").Value);
            RevoluteJoint joint;
            RevoluteComponent revoluteJointComponent;
            PhysicsComponent physicsComponentA = _entityManager.getComponent(levelUid, int.Parse(data.Attribute("actor_a").Value), ComponentType.Physics) as PhysicsComponent;
            PhysicsComponent physicsComponentB = _entityManager.getComponent(levelUid, int.Parse(data.Attribute("actor_b").Value), ComponentType.Physics) as PhysicsComponent;

            if (physicsComponentA == null || physicsComponentB == null)
                return;

            bodyA = physicsComponentA.body;
            bodyB = physicsComponentB.body;

            joint = JointFactory.CreateRevoluteJoint(world, bodyA, bodyB, bodyA.GetLocalPoint(jointWorldPosition), bodyB.GetLocalPoint(jointWorldPosition));
            joint.CollideConnected = false;
            joint.LimitEnabled = enableLimit;
            joint.MotorEnabled = enableMotor;
            joint.LowerLimit = lowerLimit;
            joint.UpperLimit = upperLimit;
            joint.MaxMotorTorque = maxMotorTorque;
            joint.MotorSpeed = motorSpeed;
            revoluteJointComponent = new RevoluteComponent(joint);
            entityId = _entityManager.createEntity(levelUid, actorId);

            // Add components
            _entityManager.addComponent(levelUid, entityId, revoluteJointComponent);

            // Connect to circuit gate if necessary
            if (_levelUidActorIdEntityIdGateComponentMap[levelUid].ContainsKey(actorId))
            {
                foreach (int gateEntityId in _levelUidActorIdEntityIdGateComponentMap[levelUid][actorId].Keys)
                {
                    GateOutputComponent gateOutputComponent = _levelUidActorIdEntityIdGateComponentMap[levelUid][actorId][gateEntityId];
                    eventSystem.addHandler(gateOutputComponent.onEnabledEvent, gateEntityId, revoluteJointComponent);
                    eventSystem.addHandler(gateOutputComponent.onDisabledEvent, gateEntityId, revoluteJointComponent);
                }
            }
        }

        // createPrismaticJoint
        public void createPrismaticJoint(string levelUid, XElement data)
        {
            EventSystem eventSystem = _systemManager.getSystem(SystemType.Event) as EventSystem;
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId;
            GroundBodyComponent groundBodyComponent = _entityManager.getComponents<GroundBodyComponent>(levelUid, ComponentType.GroundBody)[0];
            Vector2 jointWorldPosition = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            Vector2 axis = Loader.loadVector2(data.Attribute("axis"), new Vector2(1, 0));
            float upperLimit = Loader.loadFloat(data.Attribute("upper_limit"), 0f);
            float lowerLimit = Loader.loadFloat(data.Attribute("lower_limit"), 0f);
            bool autoCalculateForce = Loader.loadBool(data.Attribute("auto_calculate_force"), false);
            float buttonForceDifference = Loader.loadFloat(data.Attribute("auto_force_difference"), 0f);
            float motorSpeed = Loader.loadFloat(data.Attribute("motor_speed"), 0f);
            bool motorEnabled = Loader.loadBool(data.Attribute("motor_enabled"), false);
            float maxMotorForce = Loader.loadFloat(data.Attribute("max_motor_force"), 0f);
            Body bodyA = null;
            Body bodyB = null;
            PrismaticJointComponent prismaticJointComponent;
            PrismaticJoint joint;
            PhysicsComponent physicsComponentA = _entityManager.getComponent(levelUid, int.Parse(data.Attribute("actor_a").Value), ComponentType.Physics) as PhysicsComponent;
            PhysicsComponent physicsComponentB = _entityManager.getComponent(levelUid, int.Parse(data.Attribute("actor_b").Value), ComponentType.Physics) as PhysicsComponent;

            bodyA = physicsComponentA.body;
            bodyB = physicsComponentB.body;

            joint = JointFactory.CreatePrismaticJoint(bodyA, bodyB, bodyA.WorldCenter, axis);
            joint.LowerLimit = lowerLimit;
            joint.UpperLimit = upperLimit;
            joint.LimitEnabled = lowerLimit != 0 || upperLimit != 0;
            joint.MotorEnabled = motorEnabled;
            joint.MotorSpeed = motorSpeed;
            joint.MaxMotorForce = autoCalculateForce ? bodyA.Mass * world.Gravity.Length() + buttonForceDifference : maxMotorForce;

            entityId = _entityManager.createEntity(levelUid, actorId);
            prismaticJointComponent = new PrismaticJointComponent(joint);

            // Add components
            _entityManager.addComponent(levelUid, entityId, prismaticJointComponent);

            // Connect to circuit gate if necessary
            if (_levelUidActorIdEntityIdGateComponentMap[levelUid].ContainsKey(actorId))
            {
                foreach (int gateEntityId in _levelUidActorIdEntityIdGateComponentMap[levelUid][actorId].Keys)
                {
                    GateOutputComponent gateOutputComponent = _levelUidActorIdEntityIdGateComponentMap[levelUid][actorId][gateEntityId];
                    eventSystem.addHandler(gateOutputComponent.onEnabledEvent, gateEntityId, prismaticJointComponent);
                    eventSystem.addHandler(gateOutputComponent.onDisabledEvent, gateEntityId, prismaticJointComponent);
                }
            }
        }

        // Create circuit
        public void createCircuit(string levelUid, XElement data)
        {
            EventSystem eventSystem = _systemManager.getSystem(SystemType.Event) as EventSystem;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(levelUid, actorId);
            string circuitUID = data.Attribute("circuit_uid").Value;
            XElement circuitData = ResourceManager.getResource(circuitUID);
            Circuit circuit = new Circuit(circuitData);
            CircuitComponent circuitComponent = new CircuitComponent(circuit);
            Func<int, Gate> getGateById = (id) =>
                {
                    foreach (Gate gate in circuit.gates)
                    {
                        if (gate.id == id)
                            return gate;
                    }
                    return null;
                };

            _entityManager.addComponent(levelUid, entityId, circuitComponent);

            foreach (Gate gate in circuit.gates)
            {
                if (gate.type == "output")
                {
                    OutputGate outputGate = gate as OutputGate;
                    GateOutputComponent gateOutputComponent = _levelUidCircuitIdGateIdGateComponentMap[levelUid][actorId][gate.id];
                    gateOutputComponent.outputGate = outputGate;
                    outputGate.entityId = gateOutputComponent.entityId;
                }
            }

            foreach (XElement connectionData in data.Elements("CircuitConnection"))
            {
                string connectionType = connectionData.Attribute("type").Value;
                int gateId = int.Parse(connectionData.Attribute("gate_id").Value);

                if (connectionType == "input")
                {
                    InputGate inputGate = getGateById(gateId) as InputGate;
                    GameEventType listenToEvent = (GameEventType)Loader.loadEnum(typeof(GameEventType), connectionData.Attribute("listen_to_event"), 0);
                    int gateActorId = int.Parse(connectionData.Attribute("actor_id").Value);
                    //int gateEntityId = matchEntityIdToEditorId(gateActorId);
                    int gateEntityId = gateActorId;
                    System.Diagnostics.Debug.Assert(gateEntityId != -1);

                    inputGate.listenToEvent = listenToEvent;
                    eventSystem.addHandler(inputGate.listenToEvent, gateEntityId, inputGate);
                }
            }

            circuit.updateOutput();
        }

        // Create collision filter -- Allows pairs of entities' bodies to ignore each other
        public void createCollisionFilter(string levelUid, XElement data)
        {
            int actorA = int.Parse(data.Attribute("actor_a").Value);
            int actorB = int.Parse(data.Attribute("actor_b").Value);
            int entityA = actorA;
            int entityB = actorB;
            Action<int, int> addEntityToIgnored = (ignored, ignorer) =>
                {
                    List<IComponent> components = _entityManager.getEntityComponents(levelUid, ignorer);

                    foreach (IComponent component in components)
                    {
                        switch (component.componentType)
                        {
                            case ComponentType.Physics:
                                PhysicsComponent physicsComponent = component as PhysicsComponent;
                                //Fixture currentPhysicsFixture = physicsComponent.body.FixtureList[0];
                                for (int i = 0; i < physicsComponent.body.FixtureList.Count; i++)
                                {
                                    Fixture fixture = physicsComponent.body.FixtureList[i];
                                    if (!fixture.IsIgnoredEntity(ignored))
                                        fixture.AddIgnoredEntity(ignored);
                                }
                                break;

                            case ComponentType.CharacterMovement:
                                CharacterMovementComponent characterMovementComponent = component as CharacterMovementComponent;
                                if (!characterMovementComponent.feetFixture.IsIgnoredEntity(ignored))
                                    characterMovementComponent.feetFixture.AddIgnoredEntity(ignored);
                                break;

                            case ComponentType.Rope:
                                RopeComponent ropeComponent = component as RopeComponent;
                                RopeNode currentRopeNode = ropeComponent.ropeNodeHead;
                                while (currentRopeNode != null)
                                {
                                    if (!currentRopeNode.body.FixtureList[0].IsIgnoredEntity(ignored))
                                        currentRopeNode.body.FixtureList[0].AddIgnoredEntity(ignored);
                                    currentRopeNode = currentRopeNode.next;
                                }
                                break;
                        }
                    }
                };

            addEntityToIgnored(entityA, entityB);
            addEntityToIgnored(entityB, entityA);
        }

        // createRegionGoal -- Creates a polygon that triggers an event when the player touches it
        public void createRegionGoal(string levelUid, XElement data)
        {
            LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;
            RegionGoalComponent regionGoalComponent = new RegionGoalComponent();
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(levelUid, actorId);
            List<Vector2> points = new List<Vector2>();
            List<PolygonPoint> P2TPoints = new List<PolygonPoint>();
            Polygon polygon;
            Vector2 center = Vector2.Zero;
            Body body = BodyFactory.CreateBody(world, entityId);

            body.BodyType = BodyType.Static;

            foreach (XElement pointData in data.Elements("Point"))
                points.Add(Loader.loadVector2(pointData, Vector2.Zero));

            foreach (Vector2 point in points)
                center += point / points.Count;

            foreach (Vector2 point in points)
                P2TPoints.Add(new PolygonPoint(point.X - center.X, point.Y - center.Y));

            polygon = new Polygon(P2TPoints);
            P2T.Triangulate(polygon);

            foreach (DelaunayTriangle triangle in polygon.Triangles)
            {
                PolygonShape shape = new PolygonShape(1f);
                Vertices vertices = new Vertices(3);
                Fixture fixture;
                TriangulationPoint trianglePoint;

                vertices.Add(new Vector2(triangle.Points[0].Xf, triangle.Points[0].Yf));
                trianglePoint = triangle.PointCCWFrom(triangle.Points[0]);
                vertices.Add(new Vector2(trianglePoint.Xf, trianglePoint.Yf));
                trianglePoint = triangle.PointCCWFrom(trianglePoint);
                vertices.Add(new Vector2(trianglePoint.Xf, trianglePoint.Yf));
                shape.Set(vertices);
                fixture = body.CreateFixture(shape);
                fixture.Friction = 0f;
                fixture.Restitution = 0f;
                fixture.IsSensor = true;
                fixture.CollisionCategories = (ushort)CollisionCategory.StaticGeometry;
                fixture.CollidesWith = (ushort)CollisionCategory.Player;
            }

            body.Position = center;

            // Add components
            _entityManager.addComponent(levelUid, entityId, new PhysicsComponent(body));
            _entityManager.addComponent(levelUid, entityId, regionGoalComponent);
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(levelUid, entityId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(levelUid, entityId, new SkipFluidResolutionComponent());

            // Expand level boundary
            foreach (Vector2 point in points)
                levelSystem.expandBoundary(point);
        }

        // createDynamite
        public int createDynamite(string levelUid, Vector2 position, Vector2 force)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            RenderSystem renderSystem = _systemManager.getSystem(SystemType.Render) as RenderSystem;
            Texture2D worldTexture = ResourceManager.getTexture("dynamite");
            int entityId = _entityManager.createEntity(levelUid);
            Body body = BodyFactory.CreateBody(world, entityId);
            PolygonShape shape = new PolygonShape(1f);
            Fixture fixture;
            float layerDepth = 0.1f;

            body.AngularVelocity = 60f;
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.UserData = entityId;
            shape.SetAsBox(0.19f, 0.4f);
            fixture = body.CreateFixture(shape);
            fixture.CollisionCategories = (ushort)CollisionCategory.Item;
            fixture.CollidesWith =
                (ushort)CollisionCategory.DynamicGeometry |
                (ushort)CollisionCategory.Rope |
                (ushort)CollisionCategory.StaticGeometry;
            body.ApplyForce(force, position);

            // Add components
            _entityManager.addComponent(levelUid, entityId, new SkipFluidResolutionComponent());
            _entityManager.addComponent(levelUid, entityId, new PhysicsComponent(body));
            _entityManager.addComponent(levelUid, entityId, new PrimitivesRenderComponent(renderSystem.createSpritePrimitiveObject(worldTexture, position, new Vector2(worldTexture.Width, worldTexture.Height) / 2f, body.Rotation, 1f, layerDepth)));
            _entityManager.addComponent(levelUid, entityId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(levelUid, entityId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(levelUid, entityId, new DynamiteComponent(400f, 2f, 180));
            _entityManager.addComponent(levelUid, entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Dynamite));

            return entityId;
        }

        // createExplosion
        public int createExplosion(string levelUid, Vector2 position, float strength, float radius)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId = _entityManager.createEntity(levelUid);
            Body body = BodyFactory.CreateBody(world, entityId);
            CircleShape shape = new CircleShape(radius, 1f);
            Fixture fixture = body.CreateFixture(shape);

            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.UserData = entityId;
            fixture.IsSensor = true;
            fixture.CollisionCategories = (ushort)CollisionCategory.Explosion;
            fixture.CollidesWith = 0xFFFF;

            // Add components
            _entityManager.addComponent(levelUid, entityId, new PhysicsComponent(body));
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(levelUid, entityId, new ExplosionComponent(body.Position, strength, radius));
            _entityManager.addComponent(levelUid, entityId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(levelUid, entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Explosion));

            return entityId;
        }

        // createDebris
        public int createDebris(string levelUid, Fixture sourceFixture, Vector2 force, int timeToLive, RenderableTriangle renderableTriangle, Texture2D texture, float layerDepth)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId = _entityManager.createEntity(levelUid);
            PolygonShape sourceShape = sourceFixture.Shape as PolygonShape;
            Body body = BodyFactory.CreateBody(world, entityId);
            PolygonShape shape = new PolygonShape(sourceShape.Density);
            Vertices points = new Vertices(3);
            Vector2 center = Vector2.Zero;
            Vector2 position;
            List<RenderableTriangle> renderableTriangles = new List<RenderableTriangle>();
            float restitutionIncrement = -(1f - sourceFixture.Restitution) / (float)DebrisComponent.RESTITUTION_RESTORE_COUNT;
            Fixture fixture;

            // Adjust fixture's points
            for (int i = 0; i < 3; i++)
                center += sourceShape.Vertices[i] / 3;
            for (int i = 0; i < 3; i++)
                points.Add(sourceShape.Vertices[i] - center);

            // Create body
            position = sourceFixture.Body.Position + center;
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.UserData = entityId;
            shape.Set(points);
            fixture = body.CreateFixture(shape);
            fixture.CollisionCategories = sourceFixture.CollisionCategories;
            fixture.CollidesWith = sourceFixture.CollidesWith;
            fixture.Friction = sourceFixture.Friction;
            fixture.Restitution = 1f;
            body.ApplyForce(ref force, ref position);

            // Adjust renderable triangle
            for (int i = 0; i < 3; i++)
            {
                renderableTriangle.vertices[i].Position.X -= center.X;
                renderableTriangle.vertices[i].Position.Y -= center.Y;
            }
            renderableTriangles.Add(renderableTriangle);

            // Add components
            _entityManager.addComponent(levelUid, entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Physical));
            _entityManager.addComponent(levelUid, entityId, new PhysicsComponent(body));
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(levelUid, entityId, new PrimitivesRenderComponent(new PrimitiveRenderObject(texture, renderableTriangles, layerDepth)));
            _entityManager.addComponent(levelUid, entityId, new DebrisComponent(fixture, timeToLive, restitutionIncrement));

            return entityId;
        }

        // createDecal -- Used to determine which decal to create
        public int createDecal(string levelUid, XElement data)
        {
            string decalUID = data.Attribute("decal_uid").Value;
            Vector2 position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            float angle = Loader.loadFloat(data.Attribute("angle"), 0f);
            int entityId = Loader.loadInt(data.Attribute("id"), -1);
            float layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0.11f);

            switch (decalUID)
            {
                case "blacksmith_hut":
                    entityId = createBlacksmithHut(levelUid, position, angle, layerDepth);
                    break;
                case "carpenter_hut":
                    entityId = createCarpenterHut(levelUid, position, angle, layerDepth);
                    break;
                case "door_1":
                    entityId = createDoor(levelUid, "door_1", position, angle, layerDepth);
                    break;
                case "door_2":
                    entityId = createDoor(levelUid, "door_2", position, angle, layerDepth);
                    break;
                case "rose_window_1":
                    entityId = createTreeWindow(levelUid, "rose_window_1", position, angle, layerDepth);
                    break;
                case "dagny_hut":
                    entityId = createDagnyHut(levelUid, position, angle, layerDepth);
                    break;
            }

            return entityId;
        }

        // createBlacksmithHut
        public int createBlacksmithHut(string levelUid, Vector2 position, float angle, float layerDepth)
        {
            RenderSystem renderSystem = _systemManager.getSystem(SystemType.Render) as RenderSystem;
            int entityId = _entityManager.createEntity(levelUid);
            Texture2D texture = ResourceManager.getTexture("blacksmith_hut");
            Vector2 origin = new Vector2(texture.Width, texture.Height) / 2f;

            _entityManager.addComponent(levelUid, entityId, new PrimitivesRenderComponent(renderSystem.createSpritePrimitiveObject(texture, position, new Vector2(texture.Width / 2f, texture.Height), angle, 1f, layerDepth)));

            return entityId;
        }

        // createDoor
        public int createDoor(string levelUid, string textureUID, Vector2 position, float angle, float layerDepth)
        {
            RenderSystem renderSystem = _systemManager.getSystem(SystemType.Render) as RenderSystem;
            int entityId = _entityManager.createEntity(levelUid);
            Texture2D texture = ResourceManager.getTexture(textureUID);
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height);

            _entityManager.addComponent(levelUid, entityId, new PrimitivesRenderComponent(renderSystem.createSpritePrimitiveObject(texture, position, origin, angle, 1f, layerDepth)));
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(position));

            return entityId;
        }

        // createCarpenterHut
        public int createCarpenterHut(string levelUid, Vector2 position, float angle, float layerDepth)
        {
            RenderSystem renderSystem = _systemManager.getSystem(SystemType.Render) as RenderSystem;
            int entityId = _entityManager.createEntity(levelUid);
            Texture2D texture = ResourceManager.getTexture("carpenter_hut");
            Vector2 textureOffset = new Vector2(texture.Width / 2f, texture.Height);
            Vector2 origin = new Vector2(texture.Width, texture.Height) / 2f;

            _entityManager.addComponent(levelUid, entityId, new PrimitivesRenderComponent(renderSystem.createSpritePrimitiveObject(texture, position, textureOffset, angle, 1f, layerDepth)));
            _entityManager.addComponent(levelUid, entityId, new IgnoreTreeCollisionComponent());

            return entityId;
        }

        // createTreeWindow -- Creates a window decal and attaches it to a metamer
        public int createTreeWindow(string levelUid, string textureUID, Vector2 position, float angle, float layerDepth)
        {
            RenderSystem renderSystem = _systemManager.getSystem(SystemType.Render) as RenderSystem;
            int entityId = _entityManager.createEntity(levelUid);
            Texture2D texture = ResourceManager.getTexture(textureUID);
            Vector2 origin = new Vector2(texture.Width, texture.Height) / 2f;
            Metamer metamer = (_systemManager.getSystem(SystemType.Tree) as TreeSystem).findMetamer(position);
            System.Diagnostics.Debug.Assert(metamer != null);

            _entityManager.addComponent(levelUid, entityId, new PrimitivesRenderComponent(renderSystem.createSpritePrimitiveObject(texture, position, origin, angle, 1f, layerDepth)));
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(position));
            _entityManager.addComponent(levelUid, entityId, new FollowMetamerComponent(metamer));

            return entityId;
        }

        // createDagnyHut -- Creates Dagny's hut
        public int createDagnyHut(string levelUid, Vector2 position, float angle, float layerDepth)
        {
            RenderSystem renderSystem = _systemManager.getSystem(SystemType.Render) as RenderSystem;
            int entityId = _entityManager.createEntity(levelUid);
            Texture2D texture = ResourceManager.getTexture("dagny_hut");
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height);

            _entityManager.addComponent(levelUid, entityId, new PrimitivesRenderComponent(renderSystem.createSpritePrimitiveObject(texture, position, origin, angle, 1f, layerDepth)));
            _entityManager.addComponent(levelUid, entityId, new WorldPositionComponent(position));
            _entityManager.addComponent(levelUid, entityId, new TooltipComponent("[Use] Enter Dagny's House", position, 1.2f));
            _entityManager.addComponent(levelUid, entityId, new LevelTransitionComponent("dagny_house", position, 1.2f, true));

            return entityId;
        }
    }
}
