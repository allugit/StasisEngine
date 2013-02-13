using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;
using StasisCore;
using StasisCore.Models;
using StasisCore.Controllers;
using StasisGame.Systems;
using StasisGame.Components;
using StasisGame.Managers;
using Poly2Tri;

namespace StasisGame
{
    public class EntityFactory
    {
        public struct RopeRaycastResult
        {
            public Fixture fixture;
            public Vector2 worldPoint;
            public bool success;
            public RopeRaycastResult(Fixture fixture, Vector2 worldPoint, bool success)
            {
                this.fixture = fixture;
                this.worldPoint = worldPoint;
                this.success = success;
            }
        };

        private SystemManager _systemManager;
        private EntityManager _entityManager;

        public EntityFactory(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        private BodyRenderComponent createBodyRenderComponent(XElement data)
        {
            RenderSystem renderSystem = (RenderSystem)_systemManager.getSystem(SystemType.Render);
            Material material = new Material(ResourceController.getResource(data.Attribute("material_uid").Value));
            List<Vector2> polygonPoints = new List<Vector2>();
            Texture2D texture = null;
            int primitiveCount;
            CustomVertexFormat[] vertices;
            int vertexCount;
            List<PolygonPoint> P2TPoints = new List<PolygonPoint>();
            Polygon polygon = null;
            Vector2 topLeft;
            Vector2 bottomRight;
            float layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0.1f);
            bool fixVerticeRotation = false;
            float angle = Loader.loadFloat(data.Attribute("angle"), 0f);

            switch (data.Attribute("type").Value)
            {
                case "Box":
                    Matrix transform = Matrix.CreateRotationZ(angle);
                    float halfWidth = Loader.loadFloat(data.Attribute("half_width"), 1f);
                    float halfHeight = Loader.loadFloat(data.Attribute("half_height"), 1f);
                    Vector2 p1 = new Vector2(-halfWidth, -halfHeight);
                    Vector2 p2 = new Vector2(-halfWidth, halfHeight);
                    Vector2 p3 = new Vector2(halfWidth, halfHeight);
                    Vector2 p4 = new Vector2(halfWidth, -halfHeight);
                    polygonPoints.Add(Vector2.Transform(p1, transform));
                    polygonPoints.Add(Vector2.Transform(p2, transform));
                    polygonPoints.Add(Vector2.Transform(p3, transform));
                    polygonPoints.Add(Vector2.Transform(p4, transform));
                    fixVerticeRotation = true;
                    break;

                case "Circle":
                    float segments = 64f;
                    float increment = StasisMathHelper.pi2 / segments;
                    float radius = Loader.loadFloat(data.Attribute("radius"), 1f);
                    for (float t = StasisMathHelper.pi; t > -StasisMathHelper.pi; t -= increment)
                    {
                        polygonPoints.Add(new Vector2((float)Math.Cos(t), (float)Math.Sin(t)) * radius);
                    }
                    break;

                case "Terrain":
                    Vector2 center = Vector2.Zero;
                    foreach (XElement pointData in data.Elements("Point"))
                        polygonPoints.Add(Loader.loadVector2(pointData, Vector2.Zero));
                    foreach (Vector2 point in polygonPoints)
                        center += point / polygonPoints.Count;
                    for (int i = 0; i < polygonPoints.Count; i++)
                        polygonPoints[i] -= center;
                    break;
            }
            texture = renderSystem.materialRenderer.renderMaterial(material, polygonPoints, 1f);

            topLeft = polygonPoints[0];
            bottomRight = polygonPoints[0];
            foreach (Vector2 point in polygonPoints)
            {
                P2TPoints.Add(new PolygonPoint(point.X, point.Y));
                topLeft = Vector2.Min(topLeft, point);
                bottomRight = Vector2.Max(bottomRight, point);
            }
            polygon = new Polygon(P2TPoints);
            if (Loader.loadBool(data.Attribute("destructible"), false))
            {
                float chunkSpacingX = Math.Max(0.05f, Loader.loadFloat(data.Attribute("chunk_spacing_x"), 1f));
                float chunkSpacingY = Math.Max(0.05f, Loader.loadFloat(data.Attribute("chunk_spacing_y"), 1f));
                int seed = Loader.loadInt(data.Attribute("destructible_seed"), 12345);
                Random random = new Random(seed);

                for (float i = topLeft.X; i < bottomRight.X; i += chunkSpacingX)
                {
                    for (float j = topLeft.Y; j < bottomRight.Y; j += chunkSpacingY)
                    {
                        float jitterX = (float)(random.NextDouble() * 2 - 1) * chunkSpacingX * 0.25f;
                        float jitterY = (float)(random.NextDouble() * 2 - 1) * chunkSpacingY * 0.25f;
                        Vector2 point = new Vector2(i, j) + new Vector2(jitterX, jitterY);
                        if (polygon.IsPointInside(new TriangulationPoint(point.X, point.Y)))
                        {
                            polygon.AddSteinerPoint(new TriangulationPoint(point.X, point.Y));
                        }
                    }
                }
            }
            P2T.Triangulate(polygon);

            primitiveCount = polygon.Triangles.Count;
            vertices = new CustomVertexFormat[primitiveCount * 3];
            vertexCount = 0;
            foreach (DelaunayTriangle triangle in polygon.Triangles)
            {
                Vector2 p1 = new Vector2(triangle.Points[0].Xf, triangle.Points[0].Yf);
                Vector2 p2 = new Vector2(triangle.Points[1].Xf, triangle.Points[1].Yf);
                Vector2 p3 = new Vector2(triangle.Points[2].Xf, triangle.Points[2].Yf);
                Vector2 uv1 = p1;
                Vector2 uv2 = p2;
                Vector2 uv3 = p3;

                // Fix rotation of vertices if necessary (boxes need this)
                if (fixVerticeRotation)
                {
                    p1 = Vector2.Transform(p1, Matrix.CreateRotationZ(-angle));
                    p2 = Vector2.Transform(p2, Matrix.CreateRotationZ(-angle));
                    p3 = Vector2.Transform(p3, Matrix.CreateRotationZ(-angle));
                }

                vertices[vertexCount++] = new CustomVertexFormat(
                    new Vector3(p1, 0),
                    (uv1 - topLeft) / (bottomRight - topLeft),
                    Vector3.One);
                vertices[vertexCount++] = new CustomVertexFormat(
                    new Vector3(p2, 0),
                    (uv2 - topLeft) / (bottomRight - topLeft),
                    Vector3.One);
                vertices[vertexCount++] = new CustomVertexFormat(
                    new Vector3(p3, 0),
                    (uv3 - topLeft) / (bottomRight - topLeft),
                    Vector3.One);
            }
            Console.WriteLine("vertex count: {0}", vertexCount);

            return new BodyRenderComponent(texture, vertices, Matrix.Identity, primitiveCount, layerDepth);
        }

        public void createBox(XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId = _entityManager.createEntity();
            _entityManager.addComponent(entityId, new PhysicsComponent(world, data));
            _entityManager.addComponent(entityId, createBodyRenderComponent(data));
            Console.WriteLine("Box created");
        }

        public void createCircle(XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId = _entityManager.createEntity();
            _entityManager.addComponent(entityId, new PhysicsComponent(world, data));
            _entityManager.addComponent(entityId, createBodyRenderComponent(data));
            Console.WriteLine("Circle created");
        }

        public void createItem(XElement data)
        {
        }

        // Process of creating a rope
        // 1) Raycast pointA to pointB
        // 2) Raycast pointB to pointA
        // 3) Ensure at least one raycast was successful
        // 4) Ensure raycast to pointB was a success if doubleAnchor is true
        // 5) Ensure total length between point A and B is longer than 1 rope segment
        // 6) Create rope
        // 7) Create entity with rope component
        public void createRope(XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId;
            float segmentLength = 0.5f;
            float segmentHalfLength = segmentLength * 0.5f;
            bool doubleAnchor = Loader.loadBool(data.Attribute("double_anchor"), false);
            Vector2 initialPointA = Loader.loadVector2(data.Attribute("point_a"), Vector2.Zero);
            Vector2 initialPointB = Loader.loadVector2(data.Attribute("point_b"), Vector2.Zero);
            RopeRaycastResult abResult = new RopeRaycastResult();
            RopeRaycastResult baResult = new RopeRaycastResult();
            Vector2 finalPointA = Vector2.Zero;
            Vector2 finalPointB = Vector2.Zero;
            Vector2 finalRelativeLine = Vector2.Zero;
            Vector2 ropeNormal = Vector2.Zero;
            float finalLength;
            float angle;
            int ropeNodeLimit;
            RopeNode head = null;
            RopeNode lastNode = null;
            
            world.RayCast((fixture, point, normal, fraction) =>
                {
                    abResult.fixture = fixture;
                    abResult.worldPoint = point;
                    abResult.success = true;
                    return fraction;
                },
                initialPointA,
                initialPointB);

            world.RayCast((fixture, point, normal, fraction) =>
                {
                    baResult.fixture = fixture;
                    baResult.worldPoint = point;
                    baResult.success = true;
                    return fraction;
                },
                abResult.success ? abResult.worldPoint : initialPointB,
                initialPointA);

            if (!(abResult.success || baResult.success))
                return;

            finalPointA = baResult.success ? baResult.worldPoint : initialPointA;
            finalPointB = abResult.success ? abResult.worldPoint : initialPointB;
            finalRelativeLine = finalPointB - finalPointA;
            finalLength = finalRelativeLine.Length();

            if (doubleAnchor && !abResult.success)
                return;
            else if (finalLength < segmentLength)
                return;

            angle = (float)Math.Atan2(finalRelativeLine.Y, finalRelativeLine.X);
            ropeNormal = finalRelativeLine;
            ropeNormal.Normalize();
            ropeNodeLimit = (int)Math.Ceiling(finalLength / segmentLength);
            for (int i = 0; i < ropeNodeLimit; i++)
            {
                BodyDef bodyDef = new BodyDef();
                PolygonShape shape = new PolygonShape();
                FixtureDef fixtureDef = new FixtureDef();
                Body body;
                RopeNode ropeNode;
                RevoluteJointDef jointDef = new RevoluteJointDef();

                bodyDef.angle = angle;
                bodyDef.position = finalPointA + ropeNormal * (segmentHalfLength + i * segmentLength);
                bodyDef.type = BodyType.Dynamic;

                shape.SetAsBox(segmentHalfLength, 0.1f);

                fixtureDef.density = 0.5f;
                fixtureDef.friction = 0.5f;
                fixtureDef.restitution = 0f;
                fixtureDef.shape = shape;

                body = world.CreateBody(bodyDef);
                body.CreateFixture(fixtureDef);

                if (lastNode != null)
                {
                    jointDef.bodyA = lastNode.body;
                    jointDef.bodyB = body;
                    jointDef.localAnchorA = new Vector2(-segmentHalfLength, 0);
                    jointDef.localAnchorB = new Vector2(segmentHalfLength, 0);
                    world.CreateJoint(jointDef);
                }

                ropeNode = new RopeNode(body);
                if (head == null)
                    head = ropeNode;
                if (!(lastNode == null))
                    lastNode.insert(ropeNode);
                lastNode = ropeNode;
            }

            entityId = _entityManager.createEntity();
            _entityManager.addComponent(entityId, new RopePhysicsComponent(head));
            _entityManager.addComponent(entityId, new RopeRenderComponent());
            Console.WriteLine("Rope created");
        }

        public void createTerrain(XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId = _entityManager.createEntity();
            _entityManager.addComponent(entityId, new PhysicsComponent(world, data));
            _entityManager.addComponent(entityId, createBodyRenderComponent(data));
            Console.WriteLine("Terrain created");
        }

        public void createTree(XElement data)
        {
        }
    }
}
