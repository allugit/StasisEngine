using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public enum MaterialType
    {
        Terrain = 0,
        Trees,
        Fluid,
        Items
    };

    public abstract class MaterialResource
    {
        protected string _tag;
        protected MaterialType _type;

        [Browsable(false)]
        public MaterialType type { get { return _type; } }

        [CategoryAttribute("General")]
        [DisplayName("Tag")]
        public string tag { get { return _tag; } set { _tag = value; } }

        // Constructor
        public MaterialResource(string tag)
        {
            _tag = tag;
        }

        // Override default string
        public override string ToString()
        {
            return _tag;
        }

        // copyFrom -- clones a list
        public static List<MaterialResource> copyFrom(IList<MaterialResource> list)
        {
            List<MaterialResource> copy = new List<MaterialResource>(list.Count);
            foreach (MaterialResource material in list)
                copy.Add(material.clone());
            return copy;
        }

        // toXML
        abstract public XElement toXML();

        // clone
        public abstract MaterialResource clone();
    }
}
