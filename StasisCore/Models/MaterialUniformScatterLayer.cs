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
        protected int _randomRed;
        protected int _randomGreen;
        protected int _randomBlue;
        protected int _randomAlpha;

        public float horizontalSpacing { get { return _horizontalSpacing; } set { _horizontalSpacing = value; } }
        public float verticalSpacing { get { return _verticalSpacing; } set { _verticalSpacing = value; } }
        public float jitter { get { return _jitter; } set { _jitter = value; } }
        virtual public Color baseColor { get { return _baseColor; } set { _baseColor = value; } }
        public int randomRed { get { return _randomRed; } set { _randomRed = value; } }
        public int randomGreen { get { return _randomGreen; } set { _randomGreen = value; } }
        public int randomBlue { get { return _randomBlue; } set { _randomBlue = value; } }
        public int randomAlpha { get { return _randomAlpha; } set { _randomAlpha = value; } }

        override public XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("horizontal_spacing", _horizontalSpacing);
                d.SetAttributeValue("vertical_spacing", _verticalSpacing);
                d.SetAttributeValue("jitter", _jitter);
                d.SetAttributeValue("base_color", _baseColor);
                d.SetAttributeValue("random_red", _randomRed);
                d.SetAttributeValue("random_green", _randomGreen);
                d.SetAttributeValue("random_blue", _randomBlue);
                d.SetAttributeValue("random_alpha", _randomAlpha);
                return d;
            }
        }

        // Create new
        public MaterialUniformScatterLayer()
            : base("uniform_scatter")
        {
            _baseColor = Color.White;
            _horizontalSpacing = 32f;
            _verticalSpacing = 32f;
            _jitter = 16f;
            _randomRed = 0;
            _randomGreen = 0;
            _randomBlue = 0;
            _randomAlpha = 0;
        }

        // Load from xml
        public MaterialUniformScatterLayer(XElement data)
            : base(data)
        {
            _horizontalSpacing = Loader.loadFloat(data.Attribute("horizontal_spacing"), 32f);
            _verticalSpacing = Loader.loadFloat(data.Attribute("vertical_spacing"), 32f);
            _jitter = Loader.loadFloat(data.Attribute("jitter"), 0f);
            _baseColor = Loader.loadColor(data.Attribute("base_color"), Color.White);
            _randomRed = Loader.loadInt(data.Attribute("random_red"), 0);
            _randomGreen = Loader.loadInt(data.Attribute("random_green"), 0);
            _randomBlue = Loader.loadInt(data.Attribute("random_blue"), 0);
            _randomAlpha = Loader.loadInt(data.Attribute("random_alpha"), 0);
        }

        // Clone
        public override MaterialLayer clone()
        {
            return new MaterialUniformScatterLayer(data);
        }
    }
}
