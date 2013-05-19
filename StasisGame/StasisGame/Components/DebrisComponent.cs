using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class DebrisComponent : IComponent
    {
        private int _timeToLive;

        public int timeToLive { get { return _timeToLive; } set { _timeToLive = value; } }
        public ComponentType componentType { get { return ComponentType.Debris; } }

        public DebrisComponent(int timeToLive)
        {
            _timeToLive = timeToLive;
        }
    }
}
