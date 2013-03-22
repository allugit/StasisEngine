using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.Components
{
    public class RopeGrabComponent : IComponent
    {
        private RopeNode _ropeNode;

        public ComponentType componentType { get { return ComponentType.RopeGrab; } }
        public RopeNode ropeNode { get { return _ropeNode; } set { _ropeNode = value; } }

        public RopeGrabComponent(RopeNode ropeNode)
        {
            _ropeNode = ropeNode;
        }
    }
}
