using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class CircleProperties : ActorProperties
    {
        private float _radius;

        public float radius { get { return _radius; } set { _radius = value; } }

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
