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
        public struct RopeAnchorResult
        {
            public Fixture fixture;
            public Vector2 worldPoint;
            public bool success;
            public RopeAnchorResult(Fixture fixture, Vector2 worldPoint, bool success)
            {
                this.fixture = fixture;
                this.worldPoint = worldPoint;
                this.success = success;
            }
        };

        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private Dictionary<int, Dictionary<int, GateOutputComponent>> _actorIdEntityIdGateComponentMap;     // key 1) actor id needing to be listened to
                                                                                                            // key 2) output gate's entity id
        private Dictionary<int, Dictionary<int, GateOutputComponent>> _circuitIdGateIdGateComponentMap;     // key 1) circuit actor id
                                                                                                            // key 2) gate id

        public EntityFactory(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _actorIdEntityIdGateComponentMap = new Dictionary<int, Dictionary<int, GateOutputComponent>>();
            _circuitIdGateIdGateComponentMap = new Dictionary<int, Dictionary<int, GateOutputComponent>>();
        }

        public void reset()
        {
            _actorIdEntityIdGateComponentMap.Clear();
            _circuitIdGateIdGateComponentMap.Clear();
        }

        private void expandLevelBoundary(Vector2 point)
        {
            ((LevelSystem)_systemManager.getSystem(SystemType.Level)).expandBoundary(point);
        }

        public void createOutputGates(XElement data)
        {
            _actorIdEntityIdGateComponentMap.Clear();
            _circuitIdGateIdGateComponentMap.Clear();

            foreach (XElement circuitActorData in (from element in data.Elements("Actor") where element.Attribute("type").Value == "Circuit" select element))
            {
                int circuitId = int.Parse(circuitActorData.Attribute("id").Value);

                foreach (XElement connectionData in (from element in circuitActorData.Elements("CircuitConnection") where element.Attribute("type").Value == "output" select element))
                {
                    int actorIdToListenTo = int.Parse(connectionData.Attribute("actor_id").Value);
                    int gateId = int.Parse(connectionData.Attribute("gate_id").Value);
                    GameEventType onEnabledEvent = (GameEventType)Loader.loadEnum(typeof(GameEventType), connectionData.Attribute("on_enabled_event"), 0);
                    GameEventType onDisabledEvent = (GameEventType)Loader.loadEnum(typeof(GameEventType), connectionData.Attribute("on_disabled_event"), 0);
                    int entityId = _entityManager.createEntity();
                    GateOutputComponent gateOutputComponent = new GateOutputComponent();

                    gateOutputComponent.onEnabledEvent = onEnabledEvent;
                    gateOutputComponent.onDisabledEvent = onDisabledEvent;
                    gateOutputComponent.entityId = entityId;

                    if (!_actorIdEntityIdGateComponentMap.ContainsKey(actorIdToListenTo))
                        _actorIdEntityIdGateComponentMap.Add(actorIdToListenTo, new Dictionary<int, GateOutputComponent>());
                    if (!_actorIdEntityIdGateComponentMap[actorIdToListenTo].ContainsKey(entityId))
                        _actorIdEntityIdGateComponentMap[actorIdToListenTo].Add(entityId, gateOutputComponent);

                    if (!_circuitIdGateIdGateComponentMap.ContainsKey(circuitId))
                        _circuitIdGateIdGateComponentMap.Add(circuitId, new Dictionary<int, GateOutputComponent>());
                    if (!_circuitIdGateIdGateComponentMap[circuitId].ContainsKey(gateId))
                        _circuitIdGateIdGateComponentMap[circuitId][gateId] = gateOutputComponent;

                    _entityManager.addComponent(entityId, gateOutputComponent);
                }
            }
        }

        // createGroundBody -- Creates a body that is used throughout the game as an anchor for joints (TODO: Might not be necessary with Farseer)
        public Body createGroundBody(World world)
        {
            int groundId = _entityManager.createEntity(10000);
            Body groundBody = BodyFactory.CreateBody(world, groundId);
            Fixture fixture;

            groundBody.BodyType = BodyType.Static;
            fixture = groundBody.CreateFixture(new CircleShape(0.1f, 1f));
            fixture.IsSensor = true;
            //groundBodyDef.type = BodyType.Static;
            //groundBodyDef.userData = groundId;
            //circleShape.Radius = 0.1f;
            //fixtureDef.isSensor = true;
            //fixtureDef.shape = circleShape;
            //_groundBody = world.CreateBody(groundBodyDef);
            //_groundBody.CreateFixture(fixtureDef);

            _entityManager.addComponent(groundId, new GroundBodyComponent(groundBody));
            _entityManager.addComponent(groundId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(groundId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(groundId, new SkipFluidResolutionComponent());

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
        public void createBox(XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(actorId);
            float layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0.1f);
            Body body;
            Fixture fixture;
            //BodyDef bodyDef = new BodyDef();
            float density = Loader.loadFloat(data.Attribute("density"), 1f);
            PolygonShape boxShape = new PolygonShape(density);
            //FixtureDef boxFixtureDef = new FixtureDef();
            BodyType bodyType = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Static);
            Transform xf;
            BodyRenderComponent bodyRenderComponent;
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
            /*
            bodyDef.type = bodyType;
            bodyDef.position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            bodyDef.angle = Loader.loadFloat(data.Attribute("angle"), 0f);
            bodyDef.userData = entityId;
            boxFixtureDef.density = Loader.loadFloat(data.Attribute("density"), 1f);
            boxFixtureDef.friction = Loader.loadFloat(data.Attribute("friction"), 1f);
            boxFixtureDef.restitution = Loader.loadFloat(data.Attribute("restitution"), 1f);
            boxFixtureDef.filter.categoryBits = bodyType == BodyType.Dynamic ? (ushort)CollisionCategory.DynamicGeometry : (ushort)CollisionCategory.StaticGeometry;
            boxFixtureDef.filter.maskBits =
                (ushort)CollisionCategory.DynamicGeometry |
                (ushort)CollisionCategory.Player |
                (ushort)CollisionCategory.Rope |
                (ushort)CollisionCategory.StaticGeometry |
                (ushort)CollisionCategory.Item;
            boxShape.SetAsBox(Loader.loadFloat(data.Attribute("half_width"), 1f), Loader.loadFloat(data.Attribute("half_height"), 1f));
            boxFixtureDef.shape = boxShape;
            body = world.CreateBody(bodyDef);
            body.CreateFixture(boxFixtureDef);*/

            // Create body render component
            texture = createBoxTexture(body, data);
            renderableTriangles = createBoxRenderableTriangles(body);
            bodyRenderComponent = new BodyRenderComponent(texture, renderableTriangles, layerDepth);

            // Add components
            if (bodyType != BodyType.Static)
                _entityManager.addComponent(entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Physical));
            _entityManager.addComponent(entityId, bodyRenderComponent);
            _entityManager.addComponent(entityId, new PhysicsComponent(body));
            _entityManager.addComponent(entityId, new WorldPositionComponent(body.Position));

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
        public void createCircle(XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(actorId);
            Body body;
            Fixture fixture;
            //BodyDef bodyDef = new BodyDef();
            //FixtureDef circleFixtureDef = new FixtureDef();
            CircleShape circleShape = new CircleShape(Loader.loadFloat(data.Attribute("radius"), 1f), Loader.loadFloat(data.Attribute("density"), 1f));
            BodyType bodyType = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Static);
            Texture2D texture;
            float layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0.1f);
            BodyRenderComponent bodyRenderComponent;
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

            //bodyDef.type = bodyType;
            //bodyDef.position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            //bodyDef.userData = entityId;
            //circleFixtureDef.density = Loader.loadFloat(data.Attribute("density"), 1f);
            //circleFixtureDef.friction = Loader.loadFloat(data.Attribute("friction"), 1f);
            //circleFixtureDef.restitution = Loader.loadFloat(data.Attribute("restitution"), 1f);
            //circleFixtureDef.filter.categoryBits = bodyType == BodyType.Dynamic ? (ushort)CollisionCategory.DynamicGeometry : (ushort)CollisionCategory.StaticGeometry;
            //circleFixtureDef.filter.maskBits =
            //    (ushort)CollisionCategory.DynamicGeometry |
            //    (ushort)CollisionCategory.Player |
            //    (ushort)CollisionCategory.Rope |
            //    (ushort)CollisionCategory.StaticGeometry |
            //    (ushort)CollisionCategory.Item;
            //circleShape.Radius = Loader.loadFloat(data.Attribute("radius"), 1f);
            //circleFixtureDef.shape = circleShape;

            //body = world.CreateBody(bodyDef);
            //body.CreateFixture(circleFixtureDef);

            // Create body render component
            texture = createCircleTexture(body, data);
            renderableTriangles = createCircleRenderableTriangles(body);
            bodyRenderComponent = new BodyRenderComponent(texture, renderableTriangles, layerDepth);

            // Add components
            if (bodyType != BodyType.Static)
                _entityManager.addComponent(entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Physical));
            _entityManager.addComponent(entityId, new PhysicsComponent(body));
            _entityManager.addComponent(entityId, bodyRenderComponent);
            _entityManager.addComponent(entityId, new WorldPositionComponent(body.Position));

            // Expand level boundary
            expandLevelBoundary(body.Position + new Vector2(-circleShape.Radius, -circleShape.Radius));
            expandLevelBoundary(body.Position + new Vector2(circleShape.Radius, circleShape.Radius));
        }

        public void createFluid(XElement data)
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

        public void createWorldItem(XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(actorId);
            string itemUID = data.Attribute("item_uid").Value;
            Body body;
            Fixture fixture;
            PolygonShape shape = new PolygonShape(Loader.loadFloat(data.Attribute("density"), 1f));
            XElement itemData = ResourceManager.getResource(itemUID);
            Texture2D worldTexture = ResourceManager.getTexture(Loader.loadString(itemData.Attribute("world_texture_uid"), "default_item"));
            Texture2D inventoryTexture = ResourceManager.getTexture(Loader.loadString(itemData.Attribute("inventory_texture_uid"), "default_item"));

            body = BodyFactory.CreateBody(world, Loader.loadVector2(data.Attribute("position"), Vector2.Zero), entityId);
            body.BodyType = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Dynamic);
            body.Rotation = Loader.loadFloat(data.Attribute("angle"), 0f);
            shape.SetAsBox(Loader.loadFloat(data.Attribute("half_width"), 0.25f), Loader.loadFloat(data.Attribute("half_height"), 0.25f));
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

            //bodyDef.type = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Dynamic);
            //bodyDef.position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            //bodyDef.angle = Loader.loadFloat(data.Attribute("angle"), 0f);
            //bodyDef.userData = entityId;
            //fixtureDef.density = Loader.loadFloat(data.Attribute("density"), 1f);
            //fixtureDef.friction = Loader.loadFloat(data.Attribute("friction"), 1f);
            //fixtureDef.restitution = Loader.loadFloat(data.Attribute("restitution"), 0f);
            //fixtureDef.filter.categoryBits = (ushort)CollisionCategory.Item;
            //fixtureDef.filter.maskBits =
            //    (ushort)CollisionCategory.DynamicGeometry |
            //    (ushort)CollisionCategory.Player |
            //    (ushort)CollisionCategory.Rope |
            //    (ushort)CollisionCategory.StaticGeometry |
            //    (ushort)CollisionCategory.Explosion;
            //shape.SetAsBox(Loader.loadFloat(data.Attribute("half_width"), 0.25f), Loader.loadFloat(data.Attribute("half_height"), 0.25f));
            //fixtureDef.shape = shape;

            //body = world.CreateBody(bodyDef);
            //body.CreateFixture(fixtureDef);

            _entityManager.addComponent(entityId, new ItemComponent(
                itemUID,
                (ItemType)Loader.loadEnum(typeof(ItemType), itemData.Attribute("type"), 0),
                inventoryTexture,
                Loader.loadInt(data.Attribute("quantity"), 1),
                true,
                Loader.loadBool(itemData.Attribute("adds_reticle"), false),
                Loader.loadFloat(itemData.Attribute("range"), 1f)));

            _entityManager.addComponent(entityId, new PhysicsComponent(body));
            _entityManager.addComponent(entityId, new WorldItemRenderComponent(worldTexture));
            _entityManager.addComponent(entityId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(entityId, new WorldPositionComponent(body.Position));
        }

        // Process of creating a rope
        // 1) Raycast pointA to pointB
        // 2) Raycast pointB to pointA
        // 3) Ensure at least one raycast was successful
        // 4) Ensure raycast to pointB was a success if doubleAnchor is true
        // 5) Ensure total length between point A and B is longer than 1 rope segment
        // 6) Create rope
        // 7) Create entity with rope component
        public int createRope(XElement data)
        {
            return createRope(
                Loader.loadBool(data.Attribute("double_anchor"), false),
                false,
                Loader.loadVector2(data.Attribute("point_a"), Vector2.Zero),
                Loader.loadVector2(data.Attribute("point_b"), Vector2.Zero),
                Loader.loadInt(data.Attribute("id"), -1));
        }
        public int createRope(bool doubleAnchor, bool destroyAfterRelease, Vector2 initialPointA, Vector2 initialPointB, int actorId)
        {
            TreeSystem treeSystem = _systemManager.getSystem(SystemType.Tree) as TreeSystem;
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId = -1;
            float segmentLength = 0.5f;
            float segmentHalfLength = segmentLength * 0.5f;
            RopeAnchorResult abResult = new RopeAnchorResult();
            RopeAnchorResult baResult = new RopeAnchorResult();
            Vector2 finalPointA = Vector2.Zero;
            Vector2 finalPointB = Vector2.Zero;
            Vector2 finalRelativeLine = Vector2.Zero;
            Vector2 ropeNormal = Vector2.Zero;
            float finalLength;
            float angle;
            int ropeNodeLimit;
            RopeNode head = null;
            RopeNode lastNode = null;
            //RevoluteJointDef anchorADef = new RevoluteJointDef();
            //RevoluteJointDef anchorBDef = new RevoluteJointDef();
            
            // Raycast to find A to B result
            world.RayCast((fixture, point, normal, fraction) =>
                {
                    int fixtureEntityId = (int)fixture.Body.UserData;
                    if (_entityManager.getComponent(fixtureEntityId, ComponentType.IgnoreRopeRaycast) != null)
                        return -1;
                    else if (_entityManager.getComponent(fixtureEntityId, ComponentType.Tree) != null)
                        return -1;  // the only bodies that exist on a tree are already supporting a rope, and will be destroyed along with the rope that created it

                    abResult.fixture = fixture;
                    abResult.worldPoint = point;
                    abResult.success = true;
                    return fraction;
                },
                initialPointA,
                initialPointB);

            // Raycast to find B to A result
            world.RayCast((fixture, point, normal, fraction) =>
                {
                    int fixtureEntityId = (int)fixture.Body.UserData;
                    if (_entityManager.getComponent(fixtureEntityId, ComponentType.IgnoreRopeRaycast) != null)
                        return -1;
                    else if (_entityManager.getComponent(fixtureEntityId, ComponentType.Tree) != null)
                        return -1;  // the only bodies that exist on a tree are already supporting a rope, and will be destroyed along with the rope that created it

                    baResult.fixture = fixture;
                    baResult.worldPoint = point;
                    baResult.success = true;
                    return fraction;
                },
                abResult.success ? abResult.worldPoint : initialPointB,
                initialPointA);

            if (!abResult.success)
            {
                // If two successful results are necessary or if there are no successful results, test for metamers/walls
                if (doubleAnchor || !baResult.success)
                {
                    Metamer metamer = treeSystem.findMetamer(initialPointB);
                    Fixture wallFixture = null;

                    if (metamer != null)
                    {
                        // Metamer found at initialPointB
                        metamer.createLimbBody();
                        abResult.fixture = metamer.body.FixtureList[0];
                        abResult.success = true;
                        abResult.worldPoint = initialPointB;
                    }
                    else if (testForWall(world, initialPointB, out wallFixture))
                    {
                        // Test for a wall at initalPointB
                        abResult.fixture = wallFixture;
                        abResult.success = true;
                        abResult.worldPoint = initialPointB;
                    }
                }
            }
            if (doubleAnchor && !baResult.success)
            {
                Metamer metamer = treeSystem.findMetamer(initialPointA);
                Fixture wallFixture = null;

                if (metamer != null)
                {
                    // Metamer found at initialPointA
                    metamer.createLimbBody();
                    baResult.fixture = metamer.body.FixtureList[0];
                    baResult.success = true;
                    baResult.worldPoint = initialPointA;
                }
                else if (testForWall(world, initialPointA, out wallFixture))
                {
                    // Test for a wall at initialPointA
                    baResult.fixture = wallFixture;
                    baResult.success = true;
                    baResult.worldPoint = initialPointA;
                }
            }

            // Halt if there were not a successful combination of results (single anchor needs 1 result, double anchor needs 2)
            if ((doubleAnchor && !(abResult.success && baResult.success)) ||
                (!doubleAnchor && !(abResult.success || baResult.success)))
                return -1;

            finalPointA = baResult.success ? baResult.worldPoint : initialPointA;
            finalPointB = abResult.success ? abResult.worldPoint : initialPointB;
            finalRelativeLine = finalPointB - finalPointA;
            finalLength = finalRelativeLine.Length();

            if (doubleAnchor && !abResult.success)
                return -1;
            else if (finalLength < segmentLength)
                return -1;

            angle = (float)Math.Atan2(finalRelativeLine.Y, finalRelativeLine.X);
            ropeNormal = finalRelativeLine;
            ropeNormal.Normalize();
            ropeNodeLimit = (int)Math.Ceiling(finalLength / segmentLength);
            for (int i = 0; i < ropeNodeLimit; i++)
            {
                PolygonShape shape = new PolygonShape(0.5f);
                Body body = BodyFactory.CreateBody(world);
                Fixture fixture;
                RopeNode ropeNode;
                RevoluteJoint joint = null;

                body.Rotation = angle + StasisMathHelper.pi; // Adding pi fixes a problem where rope segments are created backwards, and then snap into the correct positions
                body.Position = finalPointA + ropeNormal * (segmentHalfLength + i * segmentLength);
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
                if (doubleAnchor)
                    fixture.CollidesWith |= (ushort)CollisionCategory.Player;
                fixture.UserData = i;

                if (lastNode != null)
                {
                    joint = JointFactory.CreateRevoluteJoint(world, lastNode.body, body, new Vector2(-segmentHalfLength, 0), new Vector2(segmentHalfLength, 0));
                    /*
                    jointDef.bodyA = lastNode.body;
                    jointDef.bodyB = body;
                    jointDef.localAnchorA = new Vector2(-segmentHalfLength, 0);
                    jointDef.localAnchorB = new Vector2(segmentHalfLength, 0);
                    joint = (RevoluteJoint)world.CreateJoint(jointDef);
                    */
                }

                ropeNode = new RopeNode(body, joint, segmentHalfLength);

                if (head == null)
                    head = ropeNode;
                if (!(lastNode == null))
                    lastNode.insert(ropeNode);
                lastNode = ropeNode;
            }

            bool resultHandled = false;
            bool reverseClimbDirection = false;

            if (baResult.success)
            {
                head.anchorJoint = JointFactory.CreateRevoluteJoint(world, baResult.fixture.Body, head.body, baResult.fixture.Body.GetLocalPoint(baResult.worldPoint), new Vector2(segmentHalfLength, 0));
                /*
                anchorADef.bodyA = baResult.fixture.GetBody();
                anchorADef.bodyB = head.body;
                anchorADef.localAnchorA = baResult.fixture.GetBody().GetLocalPoint(baResult.worldPoint);
                anchorADef.localAnchorB = new Vector2(segmentHalfLength, 0);
                head.anchorJoint = (RevoluteJoint)world.CreateJoint(anchorADef);
                */
                resultHandled = true;
                reverseClimbDirection = !doubleAnchor;
            }

            if ((!doubleAnchor && !resultHandled) ||
                (doubleAnchor && resultHandled))
            {
                if (abResult.success)
                {
                    lastNode.anchorJoint = JointFactory.CreateRevoluteJoint(world, lastNode.body, abResult.fixture.Body, new Vector2(-segmentHalfLength, 0), abResult.fixture.Body.GetLocalPoint(abResult.worldPoint));
                    /*
                    anchorBDef.bodyA = lastNode.body;
                    anchorBDef.bodyB = abResult.fixture.GetBody();
                    anchorBDef.localAnchorA = new Vector2(-segmentHalfLength, 0);
                    anchorBDef.localAnchorB = abResult.fixture.GetBody().GetLocalPoint(abResult.worldPoint);
                    lastNode.anchorJoint = (RevoluteJoint)world.CreateJoint(anchorBDef);
                    */
                }
            }

            // Only supply an actor id when creating the entity if this rope is being loaded by the level.
            // Ropes created by the rope gun during gameplay don't need to use a reserved id.
            if (actorId != -1)
                entityId = _entityManager.createEntity(actorId);
            else
                entityId = _entityManager.createEntity();

            // Add components
            _entityManager.addComponent(entityId, new RopePhysicsComponent(head, destroyAfterRelease, reverseClimbDirection, doubleAnchor));
            _entityManager.addComponent(entityId, new RopeRenderComponent());
            _entityManager.addComponent(entityId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(entityId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(entityId, new SkipFluidResolutionComponent());
            _entityManager.addComponent(entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Rope));

            RopeNode current = head;
            while (current != null)
            {
                current.body.UserData = entityId;
                current = current.next;
            }

            return entityId;
        }

        // testForWall -- Used by createRope to test points for wall entities
        private bool testForWall(World world, Vector2 point, out Fixture wallFixture)
        {
            AABB aabb = new AABB();
            bool result = false;
            Fixture resultFixture = null;

            aabb.LowerBound = point;
            aabb.UpperBound = point;
            world.QueryAABB((fixture) =>
                {
                    if (fixture.TestPoint(ref point, 0f))
                    {
                        int fixtureEntityId = (int)fixture.Body.UserData;
                        WallComponent wallComponent = (WallComponent)_entityManager.getComponent(fixtureEntityId, ComponentType.Wall);

                        if (wallComponent != null)
                        {
                            result = true;
                            resultFixture = fixture;
                            return false;
                        }
                    }
                    return true;
                },
                ref aabb);

            wallFixture = resultFixture;    // have to assign wallFixture in this roundabout way because an out param cannot be assigned in a lambda statement
            return result;
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
        public void createTerrain(XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(actorId);
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
            float chunkSpacingX = Loader.loadFloat(data.Attribute("chunk_spacing_x"), 0.2f);
            float chunkSpacingY = Loader.loadFloat(data.Attribute("chunk_spacing_y"), 0.2f);
            float averageChunkSize = (chunkSpacingX + chunkSpacingY) / 2f;
            Random rng = new Random(Loader.loadInt(data.Attribute("destructible_seed"), 12345));
            Texture2D texture;
            List<RenderableTriangle> renderableTriangles;
            BodyRenderComponent bodyRenderComponent;

            body.BodyType = bodyType;
            body.UserData = entityId;

            // Load points
            foreach (XElement pointData in data.Elements("Point"))
                points.Add(Loader.loadVector2(pointData, Vector2.Zero));

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
                            StasisMathHelper.floatBetween(-chunkSpacingX, chunkSpacingX, rng),
                            StasisMathHelper.floatBetween(-chunkSpacingY, chunkSpacingY, rng));
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

            // Create body render component
            texture = createTerrainTexture(points, data);
            renderableTriangles = createTerrainRenderableTriangles(points, body);
            bodyRenderComponent = new BodyRenderComponent(texture, renderableTriangles, layerDepth);

            // Add components
            if (isWall)
            {
                _entityManager.addComponent(entityId, new IgnoreRopeRaycastComponent());
                _entityManager.addComponent(entityId, new WallComponent());
                _entityManager.addComponent(entityId, new SkipFluidResolutionComponent());
            }

            if (bodyType != BodyType.Static)
                _entityManager.addComponent(entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Physical));

            if (isDestructible)
                _entityManager.addComponent(entityId, new DestructibleGeometryComponent());
            _entityManager.addComponent(entityId, new PhysicsComponent(body));
            _entityManager.addComponent(entityId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(entityId, bodyRenderComponent);
            _entityManager.addComponent(entityId, new IgnoreTreeCollisionComponent());

            // Expand level boundary
            foreach (Vector2 point in points)
                expandLevelBoundary(point);
        }

        public void createTree(XElement data)
        {
            RenderSystem renderSystem = _systemManager.getSystem(SystemType.Render) as RenderSystem;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(actorId);
            Material barkMaterial = new Material(ResourceManager.getResource(data.Attribute("bark_material_uid").Value));
            Material leafMaterial = new Material(ResourceManager.getResource(data.Attribute("leaf_material_uid").Value));
            List<Vector2> barkPoints = new List<Vector2>();
            List<Vector2> maxLeafPoints = new List<Vector2>();
            Texture2D barkTexture;
            List<List<Texture2D>> leafTextures = new List<List<Texture2D>>();
            int leafVariations = 3;
            Tree tree;
            float maxBaseHalfWidth = Loader.loadFloat(data.Attribute("max_base_half_width"), 0.5f);
            float internodeHalfLength = Loader.loadFloat(data.Attribute("internode_half_length"), 0.5f);
            float leafRange = 1f / 14f;  // 1f / numSizes

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
            for (int i = 0; i < leafVariations; i++)
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
            _entityManager.addComponent(entityId, new TreeComponent(tree));
            _entityManager.addComponent(entityId, new WorldPositionComponent(tree.position));
        }

        public void createRevoluteJoint(XElement data)
        {
            EventSystem eventSystem = _systemManager.getSystem(SystemType.Event) as EventSystem;
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId;
            GroundBodyComponent groundBodyComponent = _entityManager.getComponents<GroundBodyComponent>(ComponentType.GroundBody)[0];
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
            PhysicsComponent physicsComponentA = _entityManager.getComponent(int.Parse(data.Attribute("actor_a").Value), ComponentType.Physics) as PhysicsComponent;
            PhysicsComponent physicsComponentB = _entityManager.getComponent(int.Parse(data.Attribute("actor_b").Value), ComponentType.Physics) as PhysicsComponent;

            if (physicsComponentA == null || physicsComponentB == null)
                return;

            bodyA = physicsComponentA.body;
            bodyB = physicsComponentB.body;

            /*
            jointDef.bodyA = bodyA;
            jointDef.bodyB = bodyB;
            jointDef.collideConnected = false;
            jointDef.localAnchorA = bodyA.GetLocalPoint(jointWorldPosition);
            jointDef.localAnchorB = bodyB.GetLocalPoint(jointWorldPosition);
            jointDef.enableLimit = enableLimit;
            jointDef.enableMotor = enableMotor;
            jointDef.lowerAngle = lowerLimit;
            jointDef.upperAngle = upperLimit;
            jointDef.maxMotorTorque = maxMotorTorque;
            jointDef.motorSpeed = motorSpeed;
            */
            joint = JointFactory.CreateRevoluteJoint(world, bodyA, bodyB, bodyA.GetLocalPoint(jointWorldPosition), bodyB.GetLocalPoint(jointWorldPosition));
            joint.CollideConnected = false;
            joint.LimitEnabled = enableLimit;
            joint.MotorEnabled = enableMotor;
            joint.LowerLimit = lowerLimit;
            joint.UpperLimit = upperLimit;
            joint.MaxMotorTorque = maxMotorTorque;
            joint.MotorSpeed = motorSpeed;
            revoluteJointComponent = new RevoluteComponent(joint);

            entityId = _entityManager.createEntity(actorId);
            _entityManager.addComponent(entityId, revoluteJointComponent);

            if (_actorIdEntityIdGateComponentMap.ContainsKey(actorId))
            {
                foreach (int gateEntityId in _actorIdEntityIdGateComponentMap[actorId].Keys)
                {
                    GateOutputComponent gateOutputComponent = _actorIdEntityIdGateComponentMap[actorId][gateEntityId];
                    eventSystem.addHandler(gateOutputComponent.onEnabledEvent, gateEntityId, revoluteJointComponent);
                    eventSystem.addHandler(gateOutputComponent.onDisabledEvent, gateEntityId, revoluteJointComponent);
                }
            }
        }

        public void createPrismaticJoint(XElement data)
        {
            EventSystem eventSystem = _systemManager.getSystem(SystemType.Event) as EventSystem;
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId;
            GroundBodyComponent groundBodyComponent = _entityManager.getComponents<GroundBodyComponent>(ComponentType.GroundBody)[0];
            //PrismaticJointDef jointDef = new PrismaticJointDef();
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
            PhysicsComponent physicsComponentA = _entityManager.getComponent(int.Parse(data.Attribute("actor_a").Value), ComponentType.Physics) as PhysicsComponent;
            PhysicsComponent physicsComponentB = _entityManager.getComponent(int.Parse(data.Attribute("actor_b").Value), ComponentType.Physics) as PhysicsComponent;

            bodyA = physicsComponentA.body;
            bodyB = physicsComponentB.body;

            /*
            jointDef.Initialize(bodyA, bodyB, bodyA.GetWorldCenter(), axis);
            jointDef.lowerTranslation = lowerLimit;
            jointDef.upperTranslation = upperLimit;
            jointDef.enableLimit = lowerLimit != 0 || upperLimit != 0;
            jointDef.enableMotor = motorEnabled;
            jointDef.motorSpeed = motorSpeed;
            jointDef.maxMotorForce = autoCalculateForce ? bodyA.GetMass() * world.Gravity.Length() + buttonForceDifference : maxMotorForce;
            */
            joint = JointFactory.CreatePrismaticJoint(bodyA, bodyB, bodyA.WorldCenter, axis);
            joint.LowerLimit = lowerLimit;
            joint.UpperLimit = upperLimit;
            joint.LimitEnabled = lowerLimit != 0 || upperLimit != 0;
            joint.MotorEnabled = motorEnabled;
            joint.MotorSpeed = motorSpeed;
            joint.MaxMotorForce = autoCalculateForce ? bodyA.Mass * world.Gravity.Length() + buttonForceDifference : maxMotorForce;

            entityId = _entityManager.createEntity(actorId);
            prismaticJointComponent = new PrismaticJointComponent(joint);
            _entityManager.addComponent(entityId, prismaticJointComponent);

            if (_actorIdEntityIdGateComponentMap.ContainsKey(actorId))
            {
                foreach (int gateEntityId in _actorIdEntityIdGateComponentMap[actorId].Keys)
                {
                    GateOutputComponent gateOutputComponent = _actorIdEntityIdGateComponentMap[actorId][gateEntityId];
                    eventSystem.addHandler(gateOutputComponent.onEnabledEvent, gateEntityId, prismaticJointComponent);
                    eventSystem.addHandler(gateOutputComponent.onDisabledEvent, gateEntityId, prismaticJointComponent);
                }
            }
        }

        public void createCircuit(XElement data)
        {
            EventSystem eventSystem = _systemManager.getSystem(SystemType.Event) as EventSystem;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(actorId);
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

            //_actorIdToEntityId.Add(actorId, entityId);
            _entityManager.addComponent(entityId, circuitComponent);

            foreach (Gate gate in circuit.gates)
            {
                if (gate.type == "output")
                {
                    OutputGate outputGate = gate as OutputGate;
                    GateOutputComponent gateOutputComponent = _circuitIdGateIdGateComponentMap[actorId][gate.id];
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

        public void createCollisionFilter(XElement data)
        {
            int actorA = int.Parse(data.Attribute("actor_a").Value);
            int actorB = int.Parse(data.Attribute("actor_b").Value);
            //int entityA = _actorIdToEntityId[actorA];
            //int entityB = _actorIdToEntityId[actorB];
            int entityA = actorA;
            int entityB = actorB;
            Action<int, int> addEntityToIgnored = (ignored, ignorer) =>
                {
                    List<IComponent> components = _entityManager.getEntityComponents(ignorer);

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

                            case ComponentType.RopePhysics:
                                RopePhysicsComponent ropePhysicsComponent = component as RopePhysicsComponent;
                                RopeNode currentRopeNode = ropePhysicsComponent.ropeNodeHead;
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

        public void createRegionGoal(XElement data)
        {
            LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;
            RegionGoalComponent regionGoalComponent = new RegionGoalComponent();
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int actorId = int.Parse(data.Attribute("id").Value);
            int entityId = _entityManager.createEntity(actorId);
            //BodyDef bodyDef = new BodyDef();
            //List<FixtureDef> fixtureDefs = new List<FixtureDef>();
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
                //FixtureDef fixtureDef = new FixtureDef();
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
            //body = world.CreateBody(bodyDef);
            //foreach (FixtureDef fixtureDef in fixtureDefs)
            //    body.CreateFixture(fixtureDef);

            // Add components
            _entityManager.addComponent(entityId, new PhysicsComponent(body));
            _entityManager.addComponent(entityId, regionGoalComponent);
            _entityManager.addComponent(entityId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(entityId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(entityId, new SkipFluidResolutionComponent());

            // Expand level boundary
            foreach (Vector2 point in points)
                levelSystem.expandBoundary(point);
        }

        public int createDynamite(Vector2 position, Vector2 force)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            Texture2D worldTexture = ResourceManager.getTexture("dynamite");
            int entityId = _entityManager.createEntity();
            Body body = BodyFactory.CreateBody(world, entityId);
            PolygonShape shape = new PolygonShape(1f);
            Fixture fixture;

            body.AngularVelocity = 6f;
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
            _entityManager.addComponent(entityId, new SkipFluidResolutionComponent());
            _entityManager.addComponent(entityId, new PhysicsComponent(body));
            _entityManager.addComponent(entityId, new WorldItemRenderComponent(worldTexture));
            _entityManager.addComponent(entityId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(entityId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(entityId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(entityId, new DynamiteComponent(400f, 2f, 180));
            _entityManager.addComponent(entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Dynamite));

            return entityId;
        }

        public int createExplosion(Vector2 position, float strength, float radius)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId = _entityManager.createEntity();
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
            _entityManager.addComponent(entityId, new PhysicsComponent(body));
            _entityManager.addComponent(entityId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(entityId, new ExplosionComponent(body.Position, strength, radius));
            _entityManager.addComponent(entityId, new SkipFluidResolutionComponent());
            _entityManager.addComponent(entityId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Explosion));

            return entityId;
        }

        public int createDebris(Fixture sourceFixture, Vector2 force, int timeToLive, RenderableTriangle renderableTriangle, Texture2D texture, float layerDepth)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId = _entityManager.createEntity();
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
                renderableTriangle.vertices[i].position.X -= center.X;
                renderableTriangle.vertices[i].position.Y -= center.Y;
            }
            renderableTriangles.Add(renderableTriangle);

            // Add components
            _entityManager.addComponent(entityId, new ParticleInfluenceComponent(ParticleInfluenceType.Physical));
            _entityManager.addComponent(entityId, new PhysicsComponent(body));
            _entityManager.addComponent(entityId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(entityId, new BodyRenderComponent(texture, renderableTriangles, layerDepth));
            _entityManager.addComponent(entityId, new DebrisComponent(fixture, timeToLive, restitutionIncrement));

            return entityId;
        }
    }
}
