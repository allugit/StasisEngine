using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.Components
{
    public class DynamiteComponent : IComponent
    {
        private float _strength;
        private float _radius;
        private int _timeToLive;

        public float strength { get { return _strength; } set { _strength = value; } }
        public float radius { get { return _radius; } set { _radius = value; } }
        public int timeToLive { get { return _timeToLive; } set { _timeToLive = value; } }

        public ComponentType componentType { get { return ComponentType.Dynamite; } }

        public DynamiteComponent(float strength, float radius, int timeToLive)
        {
            _strength = strength;
            _radius = radius;
            _timeToLive = timeToLive;
        }
    }
}
