using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class RopePhysicsComponent : IComponent
    {
        private RopeNode _ropeNodeHead;
        private int _timeToLive = -1;
        private bool _destroyAfterRelease;
        private bool _reverseClimbDirection;

        public ComponentType componentType { get { return ComponentType.RopePhysics; } }
        public RopeNode ropeNodeHead { get { return _ropeNodeHead; } }
        public int timeToLive { get { return _timeToLive; } set { _timeToLive = value; } }
        public bool destroyAfterRelease { get { return _destroyAfterRelease; } set { _destroyAfterRelease = value; } }
        public bool reverseClimbDirection { get { return _reverseClimbDirection; } set { _reverseClimbDirection = value; } }

        public RopePhysicsComponent(RopeNode ropeNodeHead, bool destroyAfterRelease, bool reverseClimbDirection)
        {
            _ropeNodeHead = ropeNodeHead;
            _destroyAfterRelease = destroyAfterRelease;
            _reverseClimbDirection = reverseClimbDirection;
        }
    }
}
