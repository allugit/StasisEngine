using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class FluidMaterialResource : MaterialResource
    {
        // Constructor
        public FluidMaterialResource(string name) : base(name)
        {
            _type = MaterialType.Fluid;
        }

        // clone
        public override MaterialResource clone()
        {
            return new FluidMaterialResource(_name);
        }
    }
}
