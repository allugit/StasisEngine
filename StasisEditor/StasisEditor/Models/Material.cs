using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using StasisCore;
using StasisEditor.Models;

namespace StasisEditor.Models
{
    public abstract class Material
    {
        protected string _name;
        protected MaterialType _type;

        [Browsable(false)]
        public MaterialType type { get { return _type; } }

        [CategoryAttribute("General")]
        public string Name { get { return _name; } set { _name = value; } }

        // Constructor
        public Material(string name)
        {
            _name = name;
        }

        // Override default string
        public override string ToString()
        {
            return _name;
        }

        // copyFrom -- clones a list
        public static List<Material> copyFrom(ReadOnlyCollection<Material> list)
        {
            List<Material> copy = new List<Material>(list.Count);

            foreach (Material material in list)
                copy.Add(material.clone());

            return copy;
        }

        // clone
        public abstract Material clone();
    }
}
