using System;
using System.Collections.Generic;
using System.Xml.Linq;

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
        public XAttribute[] data
        {
            get
            {
                return new XAttribute[]
                {
                    new XAttribute("density", _density),
                    new XAttribute("friction", _friction),
                    new XAttribute("restitution", _restitution),
                    new XAttribute("body_type", _bodyType.ToString())
                };
            }
        }

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
