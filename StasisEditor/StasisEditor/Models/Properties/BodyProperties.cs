using System;
using System.Collections.Generic;

namespace StasisEditor.Models
{
    public enum CoreBodyType
    {
        Dynamic = 0,
        Static,
        Kinematic
    };

    public class BodyProperties : ActorProperties
    {
        private float _density;
        private float _friction;
        private float _restitution;
        private CoreBodyType _bodyType;

        public CoreBodyType bodyType { get { return _bodyType; } set { _bodyType = value; } }
        public float density { get { return _density; } set { _density = value; } }
        public float friction { get { return _friction; } set { _friction = value; } }
        public float restitution { get { return _restitution; } set { _restitution = value; } }

        public BodyProperties(CoreBodyType bodyType, float density, float friction, float restitution)
            : base()
        {
            _bodyType = bodyType;
            _density = density;
            _friction = friction;
            _restitution = restitution;
        }

        // clone
        public override ActorProperties clone()
        {
            return new BodyProperties(_bodyType, _density, _friction, _restitution);
        }
    }
}
