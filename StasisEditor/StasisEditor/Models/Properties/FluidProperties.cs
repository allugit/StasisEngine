using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisEditor.Models
{
    public class FluidProperties : ActorProperties
    {
        private float _viscosity;

        public float viscosity { get { return _viscosity; } set { _viscosity = value; } }
        [Browsable(false)]
        public XAttribute[] data
        {
            get
            {
                return new XAttribute[] { new XAttribute("viscosity", _viscosity) };
            }
        }

        // Create new
        public FluidProperties(float viscosity)
            : base()
        {
            _viscosity = viscosity;
        }

        // Load from xml
        public FluidProperties(XElement data)
        {
            _viscosity = float.Parse(data.Attribute("viscosity").Value);
        }

        // clone
        public override ActorProperties clone()
        {
            return new FluidProperties(_viscosity);
        }
    }
}
