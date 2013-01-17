using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisEditor.Models
{
    public class CircleProperties : ActorProperties
    {
        private float _radius;

        public float radius { get { return _radius; } set { _radius = value; } }
        [Browsable(false)]
        public XAttribute[] data
        {
            get
            {
                return new XAttribute[] { new XAttribute("radius", _radius) };
            }
        }

        // Create new
        public CircleProperties(float radius)
            : base()
        {
            _radius = radius;
        }

        // Load from xml
        public CircleProperties(XElement data)
        {
            _radius = float.Parse(data.Attribute("radius").Value);
        }

        // clone
        public override ActorProperties clone()
        {
            return new CircleProperties(_radius);
        }
    }
}
