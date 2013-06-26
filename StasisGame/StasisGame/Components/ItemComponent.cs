using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.Components
{
    public enum ItemType
    {
        Blueprint,
        BlueprintScrap,
        RopeGun,
        Dynamite,
        WaterGun
    };

    public class ItemComponent : IComponent
    {
        private string _itemUID;
        private ItemType _itemType;
        private Texture2D _inventoryTexture;
        private int _quantity;
        private bool _inWorld;
        private bool _hasAiming;
        private float _maxRange;
        private bool _primarySingleAction;
        private bool _primaryContinuousAction;
        private bool _secondarySingleAction;
        private bool _secondaryContinuousAction;

        public ComponentType componentType { get { return ComponentType.Item; } }
        public string itemUID { get { return _itemUID; } }
        public ItemType itemType { get { return _itemType; } }
        public Texture2D inventoryTexture { get { return _inventoryTexture; } }
        public int quantity { get { return _quantity; } set { _quantity = value; } }
        public bool inWorld { get { return _inWorld; } set { _inWorld = value; } }
        public bool hasAiming { get { return _hasAiming; } set { _hasAiming = value; } }
        public float maxRange { get { return _maxRange; } set { _maxRange = value; } }
        public bool primarySingleAction { get { return _primarySingleAction; } set { _primarySingleAction = value; } }
        public bool secondarySingleAction { get { return _secondarySingleAction; } set { _secondarySingleAction = value; } }
        public bool primaryContinuousAction { get { return _primaryContinuousAction; } set { _primaryContinuousAction = value; } }
        public bool secondaryContinuousAction { get { return _secondaryContinuousAction; } set { _secondaryContinuousAction = value; } }

        public ItemComponent(string itemUID, ItemType itemType, Texture2D inventoryTexture, int quantity, bool inWorld, bool hasAiming, float maxRange)
        {
            _itemUID = itemUID;
            _itemType = itemType;
            _inventoryTexture = inventoryTexture;
            _quantity = quantity;
            _inWorld = inWorld;
            _hasAiming = hasAiming;
            _maxRange = maxRange;
        }
    }
}
