using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class GravityGunItemResource : ItemResource
    {
        private GravityGunProperties _gravityGunProperties;

        public GravityGunProperties gravityGunProperties { get { return _gravityGunProperties; } }

        public GravityGunItemResource(ItemProperties generalProperties = null, ItemProperties gravityGunProperties = null)
            : base()
        {
            // Default general properties
            if (generalProperties == null)
                generalProperties = new GeneralItemProperties("", 1, "", "");

            // Default gravity gun properties
            if (gravityGunProperties == null)
                gravityGunProperties = new GravityGunProperties(false, 32f, 3f, 1f);

            _generalProperties = generalProperties as GeneralItemProperties;
            _gravityGunProperties = gravityGunProperties as GravityGunProperties;
            _type = ItemType.GravityGun;
        }

        // clone
        public override ItemResource clone()
        {
            return new GravityGunItemResource(_generalProperties.clone(), _gravityGunProperties.clone());
        }
    }
}
