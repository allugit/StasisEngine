using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Resources
{
    public class GravityGunItemResource : ItemResource
    {
        private bool _wellGun;
        private float _range;
        private float _radius;
        private float _strength;

        public bool wellGun { get { return _wellGun; } set { _wellGun = value; } }
        public float range { get { return _range; } set { _range = value; } }
        public float radius { get { return _radius; } set { _radius = value; } }
        public float strength { get { return _strength; } set { _strength = value; } }

        public GravityGunItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag, bool wellGun, float range, float radius, float strength)
            : base(tag, quantity, worldTextureTag, inventoryTextureTag)
        {
            _wellGun = wellGun;
            _range = range;
            _radius = radius;
            _strength = strength;
            _type = ItemType.GravityGun;
        }

        // fromXML
        public static GravityGunItemResource fromXML(XElement element)
        {
            return new GravityGunItemResource(
                element.Attribute("tag").Value,
                int.Parse(element.Attribute("quantity").Value),
                element.Attribute("worldTextureTag").Value,
                element.Attribute("inventoryTextureTag").Value,
                bool.Parse(element.Attribute("wellGun").Value),
                float.Parse(element.Attribute("range").Value),
                float.Parse(element.Attribute("radius").Value),
                float.Parse(element.Attribute("strength").Value));
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
                new XAttribute("wellGun", _wellGun),
                new XAttribute("range", _range),
                new XAttribute("radius", _radius),
                new XAttribute("strength", _strength));
        }

        // clone
        public override ItemResource clone()
        {
            return new GravityGunItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _wellGun, _range, _radius, _strength);
        }
    }
}
