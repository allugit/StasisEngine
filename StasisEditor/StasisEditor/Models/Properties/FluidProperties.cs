using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisEditor.Models
{
    public class FluidProperties : ActorProperties
    {
        private float _viscosity;

        public float viscosity { get { return _viscosity; } set { _viscosity = value; } }
        public XAttribute[] data
        {
            get
            {
                return new XAttribute[] { new XAttribute("viscosity", _viscosity) };
            }
        }

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
