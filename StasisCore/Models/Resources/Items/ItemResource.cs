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
        BlueprintScrap,
        Blueprint
    };

    abstract public class ItemResource
    {
        protected ItemType _type;
        protected string _tag;
        protected int _quantity;
        protected string _inventoryTextureTag;
        protected string _worldTextureTag;

        public ItemType type { get { return _type; } }
        public string tag { get { return _tag; } set { _tag = value; } }
        public int quantity { get { return _quantity; } set { _quantity = value; } }
        public string inventoryTextureTag { get { return _inventoryTextureTag; } set { _inventoryTextureTag = value; } }
        public string worldTextureTag { get { return _worldTextureTag; } set { _worldTextureTag = value; } }

        public ItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag)
        {
            _tag = tag;
            _quantity = quantity;
            _worldTextureTag = worldTextureTag;
            _inventoryTextureTag = inventoryTextureTag;
        }

        // clone
        abstract public ItemResource clone();
    }
}
