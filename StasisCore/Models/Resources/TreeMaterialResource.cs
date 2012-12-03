using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore;

namespace StasisCore.Models
{
    public class TreeMaterialResource : MaterialResource
    {
        // Constructor
        public TreeMaterialResource(string name) : base(name)
        {
            _type = MaterialType.Trees;
        }

        // clone
        public override MaterialResource clone()
        {
            return new TreeMaterialResource(_name);
        }
    }
}
