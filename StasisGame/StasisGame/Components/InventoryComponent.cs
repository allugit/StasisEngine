using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class InventoryComponent : IComponent
    {
        private Dictionary<int, ItemComponent> _inventory;
        private int _slots;

        public ComponentType componentType { get { return ComponentType.Inventory; } }
        public Dictionary<int, ItemComponent> inventory { get { return _inventory; } }
        public int slots { get { return _slots; } }

        public InventoryComponent(int slots)
        {
            _slots = slots;
            _inventory = new Dictionary<int, ItemComponent>();
        }
    }
}
