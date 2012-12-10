using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class RopeGunItemResource : ItemResource
    {
        public RopeGunItemResource(ItemProperties generalProperties = null)
            : base(generalProperties)
        {
            // Default base properties
            if (generalProperties == null)
                generalProperties = new GeneralItemProperties("", 1, "", "");

            _generalProperties = generalProperties as GeneralItemProperties;
            _type = ItemType.RopeGun;
        }

        // clone
        public override ItemResource clone()
        {
            return new RopeGunItemResource(_generalProperties.clone());
        }
    }
}
