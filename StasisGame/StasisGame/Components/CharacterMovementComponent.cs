using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using StasisCore;

namespace StasisGame.Components
{
    public class CharacterMovementComponent : IComponent
    {
        private float _movementAngle;
        private List<Vector2> _collisionNormals;
        private Vector2 _movementNormal;
        private bool _walkLeft;
        private bool _walkRight;
        private Fixture _feetFixture;

        public ComponentType componentType { get { return ComponentType.CharacterMovement; } }
        public float movementAngle { get { return _movementAngle; } }
        public List<Vector2> collisionNormals { get { return _collisionNormals; } }
        public Vector2 movementNormal { get { return _movementNormal; } }
        public bool walkLeft { get { return _walkLeft; } set { _walkLeft = value; } }
        public bool walkRight { get { return _walkRight; } set { _walkRight = value; } }
        public bool onSurface { get { return _collisionNormals.Count > 0; } }
        public Fixture feetFixture { get { return _feetFixture; } }

        public CharacterMovementComponent(Fixture feetFixture)
        {
            _collisionNormals = new List<Vector2>();
            _feetFixture = feetFixture;
        }

        public void calculateMovementAngle()
        {
            if (_collisionNormals.Count > 0)
            {
                _movementNormal = Vector2.Zero;
                foreach (Vector2 normal in _collisionNormals)
                    _movementNormal += normal / _collisionNormals.Count;
                _movementAngle = (float)Math.Atan2(_movementNormal.Y, _movementNormal.X) + StasisMathHelper.halfPi;
            }
            else
            {
                _movementAngle = 0;
                _movementNormal = new Vector2(1, 0);
            }
        }
    }
}
