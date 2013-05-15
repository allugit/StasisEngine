using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.Components
{
    public class ExplosionComponent : IComponent
    {
        private Vector2 _position;
        private float _strength;
        private float _radius;

        public Vector2 position { get { return _position; } }
        public float strength { get { return _strength; } }
        public float radius { get { return _radius; } }
        public ComponentType componentType { get { return ComponentType.Explosion; } }

        public ExplosionComponent(Vector2 position, float strength, float radius)
        {
            _position = position;
            _strength = strength;
            _radius = radius;
        }
    }
}
