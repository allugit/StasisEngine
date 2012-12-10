using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class GrenadeItemResource : ItemResource
    {
        private GrenadeProperties _grenadeProperties;

        public GrenadeProperties grenadeProperties { get { return _grenadeProperties; } }

        public GrenadeItemResource(ItemProperties generalProperties = null, ItemProperties grenadeProperties = null)
            : base()
        {
            // Default general properties
            if (generalProperties == null)
                generalProperties = new GeneralItemProperties("", 1, "", "");

            // Default grenade properties
            if (grenadeProperties == null)
                grenadeProperties = new GrenadeProperties(false, 1.5f, 1f);

            _generalProperties = generalProperties as GeneralItemProperties;
            _grenadeProperties = grenadeProperties as GrenadeProperties;
            _type = ItemType.Grenade;
        }

        // clone
        public override ItemResource clone()
        {
            return new GrenadeItemResource(_generalProperties.clone(), _grenadeProperties.clone());
        }
    }
}
