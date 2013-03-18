using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class InventoryComponent : IComponent
    {
        private Dictionary<int, ItemComponent> _inventory;
        private int _maxSlots;

        public ComponentType componentType { get { return ComponentType.Inventory; } }

        public InventoryComponent(int maxSlots)
        {
            _maxSlots = maxSlots;
            _inventory = new Dictionary<int, ItemComponent>();
        }

        public void addItem(ItemComponent item)
        {
            for (int i = 0; i < _maxSlots; i++)
            {
                if (!_inventory.ContainsKey(i))
                {
                    _inventory.Add(i, item);
                    return;
                }
            }
        }

        public void removeItem(ItemComponent item)
        {
            int index = -1;
            foreach (KeyValuePair<int, ItemComponent> pair in _inventory)
            {
                if (pair.Value == item)
                {
                    index = pair.Key;
                    break;
                }
            }

            if (index != -1)
            {
                _inventory.Remove(index);
            }
        }
    }
}
