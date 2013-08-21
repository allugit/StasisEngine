using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using StasisCore;

namespace StasisGame.Components
{
    public class CharacterMovementComponent : IComponent
    {
        private List<Vector2> _collisionNormals;
        private Vector2 _movementUnitVector;
        private bool _walkLeft;
        private bool _walkRight;
        private float _speedLimit;
        private Fixture _feetFixture;
        private bool _jump;
        private bool _climbUp;
        private bool _climbDown;
        private float _climbAmount;
        private bool _doRopeGrab;
        private bool _allowRopeGrab = true;
        private bool _inFluid;

        public ComponentType componentType { get { return ComponentType.CharacterMovement; } }
        public List<Vector2> collisionNormals { get { return _collisionNormals; } }
        public Vector2 movementUnitVector { get { return _movementUnitVector; } set { _movementUnitVector = value; } }
        public bool walkLeft { get { return _walkLeft; } set { _walkLeft = value; } }
        public bool walkRight { get { return _walkRight; } set { _walkRight = value; } }
        public float speedLimit { get { return _speedLimit; } set { _speedLimit = value; } }
        public bool onSurface { get { return _collisionNormals.Count > 0; } }
        public Fixture feetFixture { get { return _feetFixture; } }
        public bool jump { get { return _jump; } set { _jump = value; } }
        public bool climbUp { get { return _climbUp; } set { _climbUp = value; } }
        public bool climbDown { get { return _climbDown; } set { _climbDown = value; } }
        public float climbAmount { get { return _climbAmount; } set { _climbAmount = value; } }
        public bool doRopeGrab { get { return _doRopeGrab; } set { _doRopeGrab = value; } }
        public bool allowRopeGrab { get { return _allowRopeGrab; } set { _allowRopeGrab = value; } }
        public bool inFluid { get { return _inFluid; } set { _inFluid = value; } }

        public CharacterMovementComponent(Fixture feetFixture, float speedLimit)
        {
            _collisionNormals = new List<Vector2>();
            _feetFixture = feetFixture;
            _speedLimit = speedLimit;
        }
    }
}
