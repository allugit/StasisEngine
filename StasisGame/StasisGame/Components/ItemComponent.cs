using System;
using Microsoft.Xna.Framework;

namespace StasisGame.Components
{
    public enum ItemType
    {
        Blueprint,
        BlueprintScrap,
        RopeGun
    };

    public class ItemComponent : IComponent
    {
        private int _quantity;
        private ItemType _itemType;

        public ComponentType componentType { get { return ComponentType.Item; } }
        public ItemType itemType { get { return _itemType; } }

        public ItemComponent(ItemType itemType, int quantity)
        {
            _itemType = itemType;
            _quantity = quantity;
        }
    }
}
