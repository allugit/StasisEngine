using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class GeneralItemProperties : ItemProperties
    {
        private string _tag;
        private int _quantity;
        private string _inventoryTextureTag;
        private string _worldTextureTag;

        public string tag { get { return _tag; } set { _tag = value; } }
        public int quantity { get { return _quantity; } set { _quantity = value; } }
        public string inventoryTextureTag { get { return _inventoryTextureTag; } set { _inventoryTextureTag = value; } }
        public string worldTextureTag { get { return _worldTextureTag; } set { _worldTextureTag = value; } }

        public GeneralItemProperties(string tag, int quantity, string worldTextureTag, string inventoryTextureTag)
        {
            _tag = tag;
            _quantity = quantity;
            _worldTextureTag = worldTextureTag;
            _inventoryTextureTag = inventoryTextureTag;
        }

        // ToString
        public override string ToString()
        {
            return "General Properties";
        }

        // clone
        public override ItemProperties clone()
        {
            return new GeneralItemProperties(_tag, _quantity, _worldTextureTag, _inventoryTextureTag);
        }
    }
}
