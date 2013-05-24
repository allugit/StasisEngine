using System;
using System.Collections.Generic;
using StasisGame.Systems;

namespace StasisGame.Components
{
    public class RopePhysicsComponent : IComponent
    {
        private List<RopeNode> _segmentHeads;
        private int _timeToLive = -1;
        private bool _destroyAfterRelease;
        private bool _reverseClimbDirection;
        private bool _doubleAnchor;

        public ComponentType componentType { get { return ComponentType.RopePhysics; } }
        public List<RopeNode> segmentHeads { get { return _segmentHeads; } }
        public int timeToLive { get { return _timeToLive; } set { _timeToLive = value; } }
        public bool destroyAfterRelease { get { return _destroyAfterRelease; } set { _destroyAfterRelease = value; } }
        public bool reverseClimbDirection { get { return _reverseClimbDirection; } set { _reverseClimbDirection = value; } }
        public bool doubleAnchor { get { return _doubleAnchor; } set { _doubleAnchor = value; } }

        public RopePhysicsComponent(RopeNode ropeNodeHead, bool destroyAfterRelease, bool reverseClimbDirection, bool doubleAnchor)
        {
            _destroyAfterRelease = destroyAfterRelease;
            _reverseClimbDirection = reverseClimbDirection;
            _doubleAnchor = doubleAnchor;
            _segmentHeads = new List<RopeNode>();
            _segmentHeads.Add(ropeNodeHead);
        }

        public void startTTLCountdown()
        {
            _timeToLive = (_timeToLive > -1 && _timeToLive < RopeSystem.ROPE_TIME_TO_LIVE) ? _timeToLive : RopeSystem.ROPE_TIME_TO_LIVE;
        }
    }
}
