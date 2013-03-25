using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Box2D.XNA;

namespace StasisGame.Components
{
    public class RopeGrabComponent : IComponent
    {
        private int _ropeEntityId;
        private RopeNode _ropeNode;
        private float _distance;
        private Dictionary<Body, RevoluteJoint> _joints;
        private bool _reverseClimbDirection;

        public ComponentType componentType { get { return ComponentType.RopeGrab; } }
        public RopeNode ropeNode { get { return _ropeNode; } set { _ropeNode = value; } }
        public float distance { get { return _distance; } set { _distance = value; } }
        public Dictionary<Body, RevoluteJoint> joints { get { return _joints; } }
        public int ropeEntityId { get { return _ropeEntityId; } set { _ropeEntityId = value; } }
        public bool reverseClimbDirection { get { return _reverseClimbDirection; } }

        public RopeGrabComponent(int ropeEntityId, RopeNode ropeNode, float distance, bool reverseClimbDirection)
        {
            _ropeEntityId = ropeEntityId;
            _ropeNode = ropeNode;
            _distance = distance;
            _joints = new Dictionary<Body, RevoluteJoint>();
            _reverseClimbDirection = reverseClimbDirection;
        }
    }
}
