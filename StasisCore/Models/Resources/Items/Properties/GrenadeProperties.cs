using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class GrenadeProperties : ItemProperties
    {
        private bool _sticky;
        private float _radius;
        private float _strength;

        public bool sticky { get { return _sticky; } set { _sticky = value; } }
        public float radius { get { return _radius; } set { _radius = value; } }
        public float strength { get { return _strength; } set { _strength = value; } }

        public GrenadeProperties(bool sticky, float radius, float strength)
            : base()
        {
            _sticky = sticky;
            _radius = radius;
            _strength = strength;
        }

        // ToString
        public override string ToString()
        {
            return "Grenade Properties";
        }

        // clone
        public override ItemProperties clone()
        {
            return new GrenadeProperties(_sticky, _radius, _strength);
        }
    }
}
