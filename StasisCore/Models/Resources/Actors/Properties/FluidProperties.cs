using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class FluidProperties : ActorProperties
    {
        private float _viscosity;

        public float viscosity { get { return _viscosity; } set { _viscosity = value; } }

        public FluidProperties(float viscosity)
            : base()
        {
            _viscosity = viscosity;
        }

        // clone
        public override ActorProperties clone()
        {
            return new FluidProperties(_viscosity);
        }
    }
}
