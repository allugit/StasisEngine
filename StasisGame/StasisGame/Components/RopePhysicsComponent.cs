using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class RopePhysicsComponent : IComponent
    {
        private RopeNode _ropeNodeHead;
        private List<RopeNode> _segmentHeads;
        private int _timeToLive = -1;
        private bool _destroyAfterRelease;
        private bool _reverseClimbDirection;
        private bool _doubleAnchor;

        public ComponentType componentType { get { return ComponentType.RopePhysics; } }
        public RopeNode ropeNodeHead { get { return _ropeNodeHead; } }
        public List<RopeNode> segmentHeads { get { return _segmentHeads; } }
        public int timeToLive { get { return _timeToLive; } set { _timeToLive = value; } }
        public bool destroyAfterRelease { get { return _destroyAfterRelease; } set { _destroyAfterRelease = value; } }
        public bool reverseClimbDirection { get { return _reverseClimbDirection; } set { _reverseClimbDirection = value; } }
        public bool doubleAnchor { get { return _doubleAnchor; } set { _doubleAnchor = value; } }

        public RopePhysicsComponent(RopeNode ropeNodeHead, bool destroyAfterRelease, bool reverseClimbDirection, bool doubleAnchor)
        {
            _ropeNodeHead = ropeNodeHead;
            _destroyAfterRelease = destroyAfterRelease;
            _reverseClimbDirection = reverseClimbDirection;
            _doubleAnchor = doubleAnchor;
            _segmentHeads = new List<RopeNode>();
            _segmentHeads.Add(ropeNodeHead);
        }
    }
}
