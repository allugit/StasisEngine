using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
        protected string _name;
        protected MaterialType _type;

        [Browsable(false)]
        public MaterialType type { get { return _type; } }

        [CategoryAttribute("General")]
        [DisplayName("Name")]
        public string name { get { return _name; } set { _name = value; } }

        // Constructor
        public MaterialResource(string name)
        {
            _name = name;
        }

        // Override default string
        public override string ToString()
        {
            return _name;
        }

        // copyFrom -- clones a list
        public static List<MaterialResource> copyFrom(IList<MaterialResource> list)
        {
            List<MaterialResource> copy = new List<MaterialResource>(list.Count);
            foreach (MaterialResource material in list)
                copy.Add(material.clone());
            return copy;
        }

        // clone
        public abstract MaterialResource clone();
    }
}
