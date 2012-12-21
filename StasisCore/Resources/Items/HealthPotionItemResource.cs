using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Resources
{
    public class HealthPotionItemResource : ItemResource
    {
        private int _strength;

        public int strength { get { return _strength; } set { _strength = value; } }

        public HealthPotionItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag, int strength)
            : base(tag, quantity, worldTextureTag, inventoryTextureTag)
        {
            _strength = strength;
            _type = ItemType.HealthPotion;
        }

        // fromXML
        public static HealthPotionItemResource fromXML(XElement element)
        {
            return new HealthPotionItemResource(
                element.Attribute("tag").Value,
                int.Parse(element.Attribute("quantity").Value),
                element.Attribute("worldTextureTag").Value,
                element.Attribute("inventoryTextureTag").Value,
                int.Parse(element.Attribute("strength").Value));
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
                new XAttribute("strength", _strength));
        }

        // clone
        public override ItemResource clone()
        {
            return new HealthPotionItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _strength);
        }
    }
}
