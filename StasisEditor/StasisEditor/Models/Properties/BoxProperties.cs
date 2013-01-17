using System;
using System.ComponentModel;
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
        [Browsable(false)]
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

        // Create new
        public BoxProperties(float halfWidth, float halfHeight, float angle)
            : base()
        {
            _halfWidth = halfWidth;
            _halfHeight = halfHeight;
            _angle = angle;
        }

        // Load from xml
        public BoxProperties(XElement data)
        {
            _halfWidth = float.Parse(data.Attribute("half_width").Value);
            _halfHeight = float.Parse(data.Attribute("half_height").Value);
            _angle = float.Parse(data.Attribute("angle").Value);
        }

        // clone
        public override ActorProperties clone()
        {
            return new BoxProperties(_halfWidth, _halfHeight, _angle);
        }
    }
}
