using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class ItemMaterial : Material
    {
        // Constructor
        public ItemMaterial(string name) : base(name)
        {
            _type = MaterialType.Items;
        }

        // clone
        public override Material clone()
        {
            return new ItemMaterial(_name);
        }
    }
}
