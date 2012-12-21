using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore;

namespace StasisCore.Models
{
    public class TreeMaterialResource : MaterialResource
    {
        // Constructor
        public TreeMaterialResource(string tag) : base(tag)
        {
            _type = MaterialType.Trees;
        }

        // fromXML
        public static TreeMaterialResource fromXML(XElement element)
        {
            return new TreeMaterialResource(element.Attribute("tag").Value);
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
            return new TreeMaterialResource(_tag);
        }
    }
}
