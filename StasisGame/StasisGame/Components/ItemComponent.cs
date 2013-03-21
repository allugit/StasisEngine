using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private ItemType _itemType;
        private Texture2D _inventoryTexture;
        private int _quantity;
        private bool _inWorld;
        private bool _hasAiming;
        private float _maxRange;
        private bool _primaryAction;
        private bool _secondaryAction;

        public ComponentType componentType { get { return ComponentType.Item; } }
        public ItemType itemType { get { return _itemType; } }
        public Texture2D inventoryTexture { get { return _inventoryTexture; } }
        public int quantity { get { return _quantity; } set { _quantity = value; } }
        public bool inWorld { get { return _inWorld; } set { _inWorld = value; } }
        public bool hasAiming { get { return _hasAiming; } set { _hasAiming = value; } }
        public float maxRange { get { return _maxRange; } set { _maxRange = value; } }
        public bool primaryAction { get { return _primaryAction; } set { _primaryAction = value; } }
        public bool secondaryAction { get { return _secondaryAction; } set { _secondaryAction = value; } }

        public ItemComponent(ItemType itemType, Texture2D inventoryTexture, int quantity, bool inWorld, bool hasAiming, float maxRange)
        {
            _itemType = itemType;
            _inventoryTexture = inventoryTexture;
            _quantity = quantity;
            _inWorld = inWorld;
            _hasAiming = hasAiming;
            _maxRange = maxRange;
        }
    }
}
