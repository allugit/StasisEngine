using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.Components
{
    public class WaypointsComponent : IComponent
    {
        private string _uid;
        private List<Vector2> _waypoints;

        public ComponentType componentType { get { return ComponentType.Waypoints; } }
        public string uid { get { return _uid; } }
        public List<Vector2> waypoints { get { return _waypoints; } }

        public WaypointsComponent(string uid, List<Vector2> waypoints)
        {
            _waypoints = waypoints;
        }
    }
}
