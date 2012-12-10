using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class GravityGunProperties : ItemProperties
    {
        public bool _wellGun;
        private float _range;
        private float _radius;
        private float _strength;

        public bool wellGun { get { return _wellGun; } set { _wellGun = value; } }
        public float range { get { return _range; } set { _range = value; } }
        public float radius { get { return _radius; } set { _radius = value; } }
        public float strength { get { return _strength; } set { _strength = value; } }

        public GravityGunProperties(bool wellGun, float range, float radius, float strength)
            : base()
        {
            _wellGun = wellGun;
            _range = range;
            _radius = radius;
            _strength = strength;
        }

        // ToString
        public override string ToString()
        {
            return "Gravity Gun Properties";
        }

        // clone
        public override ItemProperties clone()
        {
            return new GravityGunProperties(_wellGun, _range, _radius, _strength);
        }
    }
}
