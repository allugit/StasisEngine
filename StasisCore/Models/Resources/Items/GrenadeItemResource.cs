using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class GrenadeItemResource : ItemResource
    {
        private bool _sticky;
        private float _radius;
        private float _strength;

        public bool sticky { get { return _sticky; } set { _sticky = value; } }
        public float radius { get { return _radius; } set { _radius = value; } }
        public float strength { get { return _strength; } set { _strength = value; } }

        public GrenadeItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag, bool sticky, float radius, float strength)
            : base(tag, quantity, worldTextureTag, inventoryTextureTag)
        {
            _sticky = sticky;
            _radius = radius;
            _strength = strength;
            _type = ItemType.Grenade;
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
                new XAttribute("sticky", _sticky),
                new XAttribute("radius", _radius),
                new XAttribute("strength", _strength));
        }

        // clone
        public override ItemResource clone()
        {
            return new GrenadeItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _sticky, _radius, _strength);
        }
    }
}
