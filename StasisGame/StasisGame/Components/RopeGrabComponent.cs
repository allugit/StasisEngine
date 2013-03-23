using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Box2D.XNA;

namespace StasisGame.Components
{
    public class RopeGrabComponent : IComponent
    {
        private RopeNode _ropeNode;
        private float _distance;
        private Dictionary<Body, RevoluteJoint> _joints;

        public ComponentType componentType { get { return ComponentType.RopeGrab; } }
        public RopeNode ropeNode { get { return _ropeNode; } set { _ropeNode = value; } }
        public float distance { get { return _distance; } set { _distance = value; } }
        public Dictionary<Body, RevoluteJoint> joints { get { return _joints; } }

        public RopeGrabComponent(RopeNode ropeNode)
        {
            _ropeNode = ropeNode;
            _joints = new Dictionary<Body, RevoluteJoint>();
        }
    }
}
