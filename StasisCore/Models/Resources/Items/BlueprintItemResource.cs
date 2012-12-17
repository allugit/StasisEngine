using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class BlueprintItemResource : ItemResource
    {
        private string _itemTag;

        public string itemTag { get { return _itemTag; } set { _itemTag = value; } }

        public BlueprintItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag, string itemTag)
            : base(tag, quantity, worldTextureTag, inventoryTextureTag)
        {
            _itemTag = itemTag;
            _type = ItemType.Blueprint;
        }

        // clone
        public override ItemResource clone()
        {
            return new BlueprintItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _itemTag);
        }
    }
}
