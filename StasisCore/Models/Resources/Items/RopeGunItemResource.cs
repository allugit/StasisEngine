using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class RopeGunItemResource : ItemResource
    {
        private RopeGunProperties _ropeGunProperties;

        public RopeGunProperties ropeGunProperties { get { return _ropeGunProperties; } }

        public RopeGunItemResource(ItemProperties generalProperties = null, ItemProperties ropeGunProperties = null)
            : base()
        {
            // Default base properties
            if (generalProperties == null)
                generalProperties = new GeneralItemProperties("", 1, "", "");

            // Default rope gun properties
            if (ropeGunProperties == null)
                ropeGunProperties = new RopeGunProperties(false, 32f);

            _generalProperties = generalProperties as GeneralItemProperties;
            _ropeGunProperties = ropeGunProperties as RopeGunProperties;
            _type = ItemType.RopeGun;
        }

        // clone
        public override ItemResource clone()
        {
            return new RopeGunItemResource(_generalProperties.clone(), _ropeGunProperties.clone());
        }
    }
}
