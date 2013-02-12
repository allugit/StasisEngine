using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using Poly2Tri;
using StasisCore;

namespace StasisGame.Components
{
    public class PhysicsComponent : IComponent
    {
        private Body _body;

        public Body body { get { return _body; } }

        public ComponentType componentType { get { return ComponentType.Physics; } }

        public PhysicsComponent(World world, XElement data)
        {
            BodyDef bodyDef = new BodyDef();
            bodyDef.type = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Static);

            List<FixtureDef> fixtureDefs = new List<FixtureDef>();
            switch (data.Attribute("type").Value)
            {
                case "Box":
                    PolygonShape boxShape = new PolygonShape();
                    FixtureDef boxFixtureDef = new FixtureDef();

                    bodyDef.position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
                    bodyDef.angle = Loader.loadFloat(data.Attribute("angle"), 0f);
                    boxFixtureDef.density = Loader.loadFloat(data.Attribute("density"), 1f);
                    boxFixtureDef.friction = Loader.loadFloat(data.Attribute("friction"), 1f);
                    boxFixtureDef.restitution = Loader.loadFloat(data.Attribute("restitution"), 1f);
                    boxShape.SetAsBox(Loader.loadFloat(data.Attribute("half_width"), 1f), Loader.loadFloat(data.Attribute("half_height"), 1f));
                    boxFixtureDef.shape = boxShape;
                    fixtureDefs.Add(boxFixtureDef);
                    break;

                case "Circle":
                    FixtureDef circleFixtureDef = new FixtureDef();
                    CircleShape circleShape = new CircleShape();

                    bodyDef.position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
                    circleFixtureDef.density = Loader.loadFloat(data.Attribute("density"), 1f);
                    circleFixtureDef.friction = Loader.loadFloat(data.Attribute("friction"), 1f);
                    circleFixtureDef.restitution = Loader.loadFloat(data.Attribute("restitution"), 1f);
                    circleShape._radius = Loader.loadFloat(data.Attribute("radius"), 1f);
                    circleFixtureDef.shape = circleShape;
                    fixtureDefs.Add(circleFixtureDef);
                    break;

                case "Terrain":
                    List<Vector2> points = new List<Vector2>();
                    List<PolygonPoint> P2TPoints = new List<PolygonPoint>();
                    Polygon polygon;
                    Vector2 center = Vector2.Zero;

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
                        FixtureDef fixtureDef = new FixtureDef();
                        PolygonShape shape = new PolygonShape();
                        Vector2[] vertices = new Vector2[3];
                        TriangulationPoint trianglePoint;

                        vertices[0] = new Vector2(triangle.Points[0].Xf, triangle.Points[0].Yf);
                        trianglePoint = triangle.PointCCWFrom(triangle.Points[0]);
                        vertices[1] = new Vector2(trianglePoint.Xf, trianglePoint.Yf);
                        trianglePoint = triangle.PointCCWFrom(trianglePoint);
                        vertices[2] = new Vector2(trianglePoint.Xf, trianglePoint.Yf);
                        shape.Set(vertices, 3);
                        fixtureDef.density = Loader.loadFloat(data.Attribute("density"), 1f);
                        fixtureDef.friction = Loader.loadFloat(data.Attribute("friction"), 1f);
                        fixtureDef.restitution = Loader.loadFloat(data.Attribute("restitution"), 1f);
                        fixtureDef.shape = shape;
                        fixtureDefs.Add(fixtureDef);
                    }

                    bodyDef.position = center;

                    break;
            }

            _body = world.CreateBody(bodyDef);
            foreach (FixtureDef fixtureDef in fixtureDefs)
                _body.CreateFixture(fixtureDef);
        }
    }
}
