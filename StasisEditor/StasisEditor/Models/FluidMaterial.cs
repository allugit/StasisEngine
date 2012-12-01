using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore;

namespace StasisEditor.Models
{
    public class FluidMaterial : Material
    {
        // Constructor
        public FluidMaterial(string name) : base(name)
        {
            _type = MaterialType.Fluid;
        }

        // clone
        public override Material clone()
        {
            return new FluidMaterial(_name);
        }
    }
}
