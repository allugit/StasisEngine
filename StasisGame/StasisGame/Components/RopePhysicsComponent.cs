using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class RopePhysicsComponent : IComponent
    {
        private RopeNode _ropeNodeHead;
        private int _timeToLive;
        private bool _detached;

        public ComponentType componentType { get { return ComponentType.RopePhysics; } }
        public RopeNode ropeNodeHead { get { return _ropeNodeHead; } }
        public int timeToLive { get { return _timeToLive; } set { _timeToLive = value; } }
        public bool detached { get { return _detached; } set { _detached = value; } }

        public RopePhysicsComponent(RopeNode ropeNodeHead)
        {
            _ropeNodeHead = ropeNodeHead;
        }
    }
}
