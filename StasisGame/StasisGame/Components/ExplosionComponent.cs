using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class ExplosionComponent : IComponent
    {
        private float _strength;
        private float _radius;

        public float strength { get { return _strength; } }
        public float radius { get { return _radius; } }

        public ComponentType componentType { get { return ComponentType.Explosion; } }

        public ExplosionComponent(float strength, float radius)
        {
            _strength = strength;
            _radius = radius;
        }
    }
}
