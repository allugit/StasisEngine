using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisCore.Models
{
    public class MaterialEdgeScatterLayer : MaterialScatterLayer
    {
        protected Vector2 _direction;
        protected float _threshold;
        protected bool _hardCutoff;
        protected Color _baseColor;
        protected int _randomRed;
        protected int _randomGreen;
        protected int _randomBlue;
        protected int _randomAlpha;

        public Vector2 direction { get { return _direction; } set { _direction = value; } }
        public float threshold { get { return _threshold; } set { _threshold = value; } }
        public bool hardCutoff { get { return _hardCutoff; } set { _hardCutoff = value; } }
        virtual public Color baseColor { get { return _baseColor; } set { _baseColor = value; } }
        public int randomRed { get { return _randomRed; } set { _randomRed = value; } }
        public int randomGreen { get { return _randomGreen; } set { _randomGreen = value; } }
        public int randomBlue { get { return _randomBlue; } set { _randomBlue = value; } }
        public int randomAlpha { get { return _randomAlpha; } set { _randomAlpha = value; } }
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("direction", _direction);
                d.SetAttributeValue("threshold", _threshold);
                d.SetAttributeValue("hard_cutoff", _hardCutoff);
                d.SetAttributeValue("base_color", _baseColor);
                d.SetAttributeValue("random_red", _randomRed);
                d.SetAttributeValue("random_green", _randomGreen);
                d.SetAttributeValue("random_blue", _randomBlue);
                d.SetAttributeValue("random_alpha", _randomAlpha);
                return d;
            }
        }

        public MaterialEdgeScatterLayer()
            : base("edge_scatter")
        {
            _direction = Vector2.Zero;
            _threshold = 0.5f;
            _hardCutoff = false;
            _baseColor = Color.White;
        }

        public MaterialEdgeScatterLayer(XElement data)
            : base(data)
        {
            _direction = Loader.loadVector2(data.Attribute("direction"), Vector2.Zero);
            _threshold = Loader.loadFloat(data.Attribute("threshold"), 0.5f);
            _hardCutoff = Loader.loadBool(data.Attribute("hard_cutoff"), false);
            _baseColor = Loader.loadColor(data.Attribute("base_color"), Color.White);
            _randomRed = Loader.loadInt(data.Attribute("random_red"), 0);
            _randomGreen = Loader.loadInt(data.Attribute("random_green"), 0);
            _randomBlue = Loader.loadInt(data.Attribute("random_blue"), 0);
            _randomAlpha = Loader.loadInt(data.Attribute("random_alpha"), 0);
        }

        public override MaterialLayer clone()
        {
            return new MaterialEdgeScatterLayer(data);
        }
    }
}
