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
        protected GeneralItemProperties _generalProperties;
        protected ItemType _type;

        public GeneralItemProperties generalProperties { get { return _generalProperties; } }
        public ItemType type { get { return _type; } }

        public ItemResource()
        {
        }

        // ToString
        public override string ToString()
        {
            return _generalProperties.tag;
        }

        // clone
        abstract public ItemResource clone();
    }
}
