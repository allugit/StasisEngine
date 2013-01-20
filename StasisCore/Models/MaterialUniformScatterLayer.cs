using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class MaterialUniformScatterLayer : MaterialScatterLayer
    {
        protected float _horizontalSpacing;
        protected float _verticalSpacing;
        protected float _jitter;
        protected Color _baseColor;

        public float horizontalSpacing { get { return _horizontalSpacing; } set { _horizontalSpacing = value; } }
        public float verticalSpacing { get { return _verticalSpacing; } set { _verticalSpacing = value; } }
        public float jitter { get { return _jitter; } set { _jitter = value; } }
        virtual public Color baseColor { get { return _baseColor; } set { _baseColor = value; } }

        override public XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("horizontal_spacing", _horizontalSpacing);
                d.SetAttributeValue("vertical_spacing", _verticalSpacing);
                d.SetAttributeValue("jitter", _jitter);
                return d;
            }
        }

        public MaterialUniformScatterLayer(float horizontalSpacing = 32f, float verticalSpacing = 32f, float jitter = 8f)
            : base("uniform_scatter")
        {
            _horizontalSpacing = horizontalSpacing;
            _verticalSpacing = verticalSpacing;
            _jitter = jitter;
        }

        public MaterialUniformScatterLayer(XElement data)
            : base(data)
        {
            _horizontalSpacing = float.Parse(data.Attribute("horizontal_spacing").Value);
            _verticalSpacing = float.Parse(data.Attribute("vertical_spacing").Value);
            _jitter = float.Parse(data.Attribute("jitter").Value);
        }
    }
}
