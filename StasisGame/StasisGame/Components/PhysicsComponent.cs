using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Poly2Tri;
using StasisCore;

namespace StasisGame.Components
{
    public class PhysicsComponent : IComponent
    {
        private Body _body;

        public Body body { get { return _body; } }

        public ComponentType componentType { get { return ComponentType.Physics; } }

        public PhysicsComponent(Body body)
        {
            _body = body;
        }
    }
}
