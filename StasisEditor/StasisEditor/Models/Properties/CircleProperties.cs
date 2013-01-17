using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisEditor.Models
{
    public class CircleProperties : ActorProperties
    {
        private float _radius;

        public float radius { get { return _radius; } set { _radius = value; } }
        public XAttribute[] data
        {
            get
            {
                return new XAttribute[] { new XAttribute("radius", _radius) };
            }
        }

        public CircleProperties(float radius)
            : base()
        {
            _radius = radius;
        }

        // clone
        public override ActorProperties clone()
        {
            return new CircleProperties(_radius);
        }
    }
}
