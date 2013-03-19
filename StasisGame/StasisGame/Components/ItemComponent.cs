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
        private bool _inWorld;

        public ComponentType componentType { get { return ComponentType.Item; } }
        public ItemType itemType { get { return _itemType; } }
        public bool inWorld { get { return _inWorld; } set { _inWorld = value; } }

        public ItemComponent(ItemType itemType, int quantity, bool inWorld)
        {
            _itemType = itemType;
            _quantity = quantity;
            _inWorld = inWorld;
        }
    }
}
