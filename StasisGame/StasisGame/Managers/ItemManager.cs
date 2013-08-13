using System;
using System.Collections.Generic;
using StasisCore.Models;

namespace StasisGame.Managers
{
    public class ItemManager
    {
        private List<ItemDefinition> _itemDefinitions;

        public ItemManager(List<ItemDefinition> itemDefinitions)
        {
            _itemDefinitions = itemDefinitions;
        }

        public ItemDefinition getItemDefinition(string itemUid)
        {
            foreach (ItemDefinition itemDefinition in _itemDefinitions)
            {
                if (itemDefinition.uid == itemUid)
                {
                    return itemDefinition;
                }
            }
            return null;
        }
    }
}
