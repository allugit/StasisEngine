using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class AIWanderBehaviorComponent : IComponent
    {
        private string _waypointsUid;
        private int _currentWaypointIndex;

        public ComponentType componentType { get { return ComponentType.AIWanderBehavior; } }
        public string waypointsUid { get { return _waypointsUid; } set { _waypointsUid = value; } }
        public int currentWaypointIndex { get { return _currentWaypointIndex; } set { _currentWaypointIndex = value; } }

        public AIWanderBehaviorComponent(string waypointsUid)
        {
            _waypointsUid = waypointsUid;
        }
    }
}
