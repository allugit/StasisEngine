using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class ItemMaterialResource : MaterialResource
    {
        // Constructor
        public ItemMaterialResource(string name) : base(name)
        {
            _type = MaterialType.Items;
        }

        // clone
        public override MaterialResource clone()
        {
            return new ItemMaterialResource(_name);
        }
    }
}
