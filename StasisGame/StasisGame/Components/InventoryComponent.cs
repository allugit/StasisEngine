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

        public ItemComponent getItem(int i)
        {
            ItemComponent itemComponent;

            _inventory.TryGetValue(i, out itemComponent);

            return itemComponent;
        }

        public void addItem(ItemComponent item)
        {
            for (int i = 0; i < _slots; i++)
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
