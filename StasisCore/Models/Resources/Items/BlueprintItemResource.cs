using System;
using System.Collections.Generic;
using System.Xml.Linq;

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

        // toXML
        public override XElement toXML()
        {
            return new XElement("Item",
                new XAttribute("type", _type),
                new XAttribute("tag", _tag),
                new XAttribute("quantity", _quantity),
                new XAttribute("worldTextureTag", _worldTextureTag),
                new XAttribute("inventoryTextureTag", _inventoryTextureTag),
                new XAttribute("itemTag", _itemTag));
        }

        // clone
        public override ItemResource clone()
        {
            return new BlueprintItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _itemTag);
        }
    }
}
