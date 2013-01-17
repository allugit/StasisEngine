using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisEditor.Models
{
    public class BoxProperties : ActorProperties
    {
        private float _halfWidth;
        private float _halfHeight;
        private float _angle;

        public float halfWidth { get { return _halfWidth; } set { _halfWidth = value; } }
        public float halfHeight { get { return _halfHeight; } set { _halfHeight = value; } }
        public float angle { get { return _angle; } set { _angle = value; } }
        public XAttribute[] data
        {
            get
            {
                return new XAttribute[]
                {
                    new XAttribute("half_width", _halfWidth),
                    new XAttribute("half_height", _halfHeight),
                    new XAttribute("angle", _angle)
                };
            }
        }

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
