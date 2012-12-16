using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class ItemMaterialResource : MaterialResource
    {
        // Constructor
        public ItemMaterialResource(string tag) : base(tag)
        {
            _type = MaterialType.Items;
        }

        // toXML
        public override XElement toXML()
        {
            XElement element = new XElement("Material",
                new XAttribute("type", _type),
                new XAttribute("tag", _tag));
            return element;
        }

        // clone
        public override MaterialResource clone()
        {
            return new ItemMaterialResource(_tag);
        }
    }
}
