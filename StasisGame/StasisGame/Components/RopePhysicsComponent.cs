using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class RopePhysicsComponent : IComponent
    {
        private RopeNode _ropeNodeHead;
        private bool _detached;
        private int _timeToLive = -1;

        public ComponentType componentType { get { return ComponentType.RopePhysics; } }
        public RopeNode ropeNodeHead { get { return _ropeNodeHead; } }
        public bool detached { get { return _detached; } set { _detached = value; } }
        public int timeToLive { get { return _timeToLive; } set { _timeToLive = value; } }

        public RopePhysicsComponent(RopeNode ropeNodeHead)
        {
            _ropeNodeHead = ropeNodeHead;
        }
    }
}
