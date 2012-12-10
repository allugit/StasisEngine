using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class HealthPotionProperties : ItemProperties
    {
        private int _strength;

        public int strength { get { return _strength; } set { _strength = value; } }

        public HealthPotionProperties(int strength)
            : base()
        {
            _strength = strength;
        }

        // ToString
        public override string ToString()
        {
            return "Health Potion Properties";
        }

        // clone
        public override ItemProperties clone()
        {
            return new HealthPotionProperties(_strength);
        }
    }
}
