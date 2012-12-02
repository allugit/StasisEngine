using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore;

namespace StasisCore.Models
{
    public class TreeMaterial : Material
    {
        // Constructor
        public TreeMaterial(string name) : base(name)
        {
            _type = MaterialType.Trees;
        }

        // clone
        public override Material clone()
        {
            return new TreeMaterial(_name);
        }
    }
}
