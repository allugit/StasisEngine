using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Box2D.XNA;
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
            bodyDef.position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            bodyDef.type = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Static);

            FixtureDef fixtureDef = new FixtureDef();
            fixtureDef.density = Loader.loadFloat(data.Attribute("density"), 1f);
            fixtureDef.friction = Loader.loadFloat(data.Attribute("friction"), 1f);
            fixtureDef.restitution = Loader.loadFloat(data.Attribute("restitution"), 1f);
            switch (data.Attribute("type").Value)
            {
                case "Box":
                    bodyDef.angle = Loader.loadFloat(data.Attribute("angle"), 0f);
                    PolygonShape boxShape = new PolygonShape();
                    boxShape.SetAsBox(Loader.loadFloat(data.Attribute("half_width"), 1f), Loader.loadFloat(data.Attribute("half_height"), 1f));
                    fixtureDef.shape = boxShape;
                    break;

                case "Circle":
                    CircleShape circleShape = new CircleShape();
                    circleShape._radius = Loader.loadFloat(data.Attribute("radius"), 1f);
                    fixtureDef.shape = circleShape;
                    break;
            }
        }
    }
}
