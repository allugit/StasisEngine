using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.Components
{
    public class WorldPositionComponent : IComponent
    {
        private Vector2 _position;

        public ComponentType componentType { get { return ComponentType.WorldPosition; } }
        public Vector2 position { get { return _position; } set { _position = value; } }

        public WorldPositionComponent(Vector2 position)
        {
            _position = position;
        }
    }
}
