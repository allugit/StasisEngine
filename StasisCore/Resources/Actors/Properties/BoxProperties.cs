using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Resources
{
    public class BoxProperties : ActorProperties
    {
        private float _halfWidth;
        private float _halfHeight;
        private float _angle;

        public float halfWidth { get { return _halfWidth; } set { _halfWidth = value; } }
        public float halfHeight { get { return _halfHeight; } set { _halfHeight = value; } }
        public float angle { get { return _angle; } set { _angle = value; } }

        public BoxProperties(float halfWidth, float halfHeight, float angle)
            : base()
        {
            _halfWidth = halfWidth;
            _halfHeight = halfHeight;
            _angle = angle;
        }

        // clone
        public override ActorProperties clone()
        {
            return new BoxProperties(_halfWidth, _halfHeight, _angle);
        }
    }
}
