using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisEditor.Models
{
    public class TreeMaterial : Material
    {
        private string _name;

        [CategoryAttribute("General")]
        public string Name { get { return _name; } set { _name = value; } }

        // Constructor
        public TreeMaterial(string name)
        {
            _name = name;
        }

        // Override default string
        public override string ToString()
        {
            return _name;
        }

        // clone
        public override Material clone()
        {
            return new TreeMaterial(_name);
        }
    }
}
