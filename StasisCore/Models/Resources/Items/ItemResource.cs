using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public enum ItemType
    {
        RopeGun,
        GravityGun,
        Grenade,
        HealthPotion,
        TreeSeed,
        BlueprintScrap
    };

    abstract public class ItemResource
    {
        protected ItemProperties _generalProperties;
        protected ItemType _type;

        public ItemProperties generalProperties { get { return _generalProperties; } }
        public ItemType type { get { return _type; } }

        public ItemResource(ItemProperties generalProperties)
        {
            _generalProperties = generalProperties;
        }

        // clone
        abstract public ItemResource clone();
    }
}
