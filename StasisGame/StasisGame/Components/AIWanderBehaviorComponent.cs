using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class AIWanderBehaviorComponent : IComponent
    {
        private string _waypointsUid;
        private int _currentWaypointIndex;
        private float _minWalkSpeed;
        private float _maxWalkSpeed;
        private int _minDelay;
        private int _maxDelay;
        private int _currentDelay;

        public ComponentType componentType { get { return ComponentType.AIWanderBehavior; } }
        public string waypointsUid { get { return _waypointsUid; } set { _waypointsUid = value; } }
        public int currentWaypointIndex { get { return _currentWaypointIndex; } set { _currentWaypointIndex = value; } }
        public float minWalkSpeed { get { return _minWalkSpeed; } set { _minWalkSpeed = value; } }
        public float maxWalkSpeed { get { return _maxWalkSpeed; } set { _maxWalkSpeed = value; } }
        public int minDelay { get { return _minDelay; } set { _minDelay = value; } }
        public int maxDelay { get { return _maxDelay; } set { _maxDelay = value; } }
        public int currentDelay { get { return _currentDelay; } set { _currentDelay = value; } }

        public AIWanderBehaviorComponent(string waypointsUid, float minWalkSpeed, float maxWalkSpeed, int minDelay, int maxDelay)
        {
            _waypointsUid = waypointsUid;
            _minWalkSpeed = minWalkSpeed;
            _maxWalkSpeed = maxWalkSpeed;
            _minDelay = minDelay;
            _maxDelay = maxDelay;
        }
    }
}
