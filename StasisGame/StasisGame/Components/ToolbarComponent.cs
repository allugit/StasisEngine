using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.Components
{
    public class ToolbarComponent : IComponent
    {
        private Dictionary<int, ItemComponent> _inventory;
        private int _slots;

        public ComponentType componentType { get { return ComponentType.Toolbar; } }
        public Dictionary<int, ItemComponent> inventory { get { return _inventory; } }
        public int slots { get { return _slots; } }

        public ToolbarComponent(int slots)
        {
            _slots = slots;
            _inventory = new Dictionary<int, ItemComponent>();

            for (int i = 0; i < _slots; i++)
            {
                _inventory.Add(i, null);
            }
        }

        public ItemComponent getItem(int slot)
        {
            return _inventory[slot];
        }

        public void assignItem(int slot, ItemComponent itemComponent)
        {
            _inventory[slot] = itemComponent;
        }

        public void clearItem(int slot)
        {
            _inventory[slot] = null;
        }
    }
}
