using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisGame.Components
{
    public class ItemComponent : IComponent
    {
        private ItemDefinition _definition;
        private ItemState _state;
        private Texture2D _inventoryTexture;
        private bool _primarySingleAction;
        private bool _primaryContinuousAction;
        private bool _secondarySingleAction;
        private bool _secondaryContinuousAction;

        public ComponentType componentType { get { return ComponentType.Item; } }
        public Texture2D inventoryTexture { get { return _inventoryTexture; } }
        public ItemDefinition definition { get { return _definition; } }
        public ItemState state { get { return _state; } }
        public bool primarySingleAction { get { return _primarySingleAction; } set { _primarySingleAction = value; } }
        public bool secondarySingleAction { get { return _secondarySingleAction; } set { _secondarySingleAction = value; } }
        public bool primaryContinuousAction { get { return _primaryContinuousAction; } set { _primaryContinuousAction = value; } }
        public bool secondaryContinuousAction { get { return _secondaryContinuousAction; } set { _secondaryContinuousAction = value; } }

        public ItemComponent(ItemDefinition definition, ItemState state, Texture2D inventoryTexture)
        {
            _definition = definition;
            _state = state;
            _inventoryTexture = inventoryTexture;
        }
    }
}
