using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class RopeGrabComponent : IComponent
    {
        private RopeNode _ropeNode;
        private float _distance;

        public ComponentType componentType { get { return ComponentType.RopeGrab; } }
        public RopeNode ropeNode { get { return _ropeNode; } set { _ropeNode = value; } }
        public float distance { get { return _distance; } set { _distance = value; } }

        public RopeGrabComponent(RopeNode ropeNode)
        {
            _ropeNode = ropeNode;
        }
    }
}
