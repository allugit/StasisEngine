using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.Components
{
    public class CharacterMovementComponent : IComponent
    {
        private float _movementAngle;
        private List<Vector2> _movementNormals;
        private bool _walkLeft;
        private bool _walkRight;

        public ComponentType componentType { get { return ComponentType.CharacterMovement; } }
        public float movementAngle { get { return _movementAngle; } }
        public List<Vector2> movementNormals { get { return _movementNormals; } }
        public bool walkLeft { get { return _walkLeft; } set { _walkLeft = value; } }
        public bool walkRight { get { return _walkRight; } set { _walkRight = value; } }

        public CharacterMovementComponent()
        {
            _movementNormals = new List<Vector2>();
        }

        public void calculateMovementAngle()
        {
            if (_movementNormals.Count > 0)
            {
                Vector2 averageNormal = Vector2.Zero;
                foreach (Vector2 normal in _movementNormals)
                    averageNormal += normal / _movementNormals.Count;
                _movementAngle = (float)Math.Atan2(averageNormal.Y, averageNormal.X);
            }
            else
            {
                _movementAngle = 0;
            }
        }
    }
}
