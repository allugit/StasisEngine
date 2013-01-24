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
        protected float _spacing;
        protected bool _useAbsoluteAngle;
        protected float _absoluteAngle;
        protected float _relativeAngle;
        protected float _angleJitter;
        protected float _jitter;
        protected Color _baseColor;
        protected int _randomRed;
        protected int _randomGreen;
        protected int _randomBlue;
        protected int _randomAlpha;

        public Vector2 direction { get { return _direction; } set { _direction = value; } }
        public float threshold { get { return _threshold; } set { _threshold = value; } }
        public bool hardCutoff { get { return _hardCutoff; } set { _hardCutoff = value; } }
        public float spacing { get { return _spacing; } set { _spacing = value; } }
        public bool useAbsoluteAngle { get { return _useAbsoluteAngle; } set { _useAbsoluteAngle = value; } }
        public float absoluteAngle { get { return _absoluteAngle; } set { _absoluteAngle = value; } }
        public float relativeAngle { get { return _relativeAngle; } set { _relativeAngle = value; } }
        public float angleJitter { get { return _angleJitter; } set { _angleJitter = value; } }
        public float jitter { get { return _jitter; } set { _jitter = value; } }
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
                d.SetAttributeValue("spacing", _spacing);
                d.SetAttributeValue("use_absolute_angle", _useAbsoluteAngle);
                d.SetAttributeValue("absolute_angle", _absoluteAngle);
                d.SetAttributeValue("relative_angle", _relativeAngle);
                d.SetAttributeValue("angle_jitter", _angleJitter);
                d.SetAttributeValue("jitter", _jitter);
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
            _threshold = 0;
            _hardCutoff = false;
            _spacing = 1f;
            _baseColor = Color.White;
            _useAbsoluteAngle = false;
            _absoluteAngle = 0f;
            _relativeAngle = 0f;
            _angleJitter = 0f;
            _jitter = 0f;
        }

        public MaterialEdgeScatterLayer(XElement data)
            : base(data)
        {
            _direction = Loader.loadVector2(data.Attribute("direction"), Vector2.Zero);
            _threshold = Loader.loadFloat(data.Attribute("threshold"), 0f);
            _hardCutoff = Loader.loadBool(data.Attribute("hard_cutoff"), false);
            _spacing = Loader.loadFloat(data.Attribute("spacing"), 1f);
            _useAbsoluteAngle = Loader.loadBool(data.Attribute("use_absolute_angle"), false);
            _absoluteAngle = Loader.loadFloat(data.Attribute("absolute_angle"), 0f);
            _relativeAngle = Loader.loadFloat(data.Attribute("relative_angle"), 0f);
            _angleJitter = Loader.loadFloat(data.Attribute("angle_jitter"), 0f);
            _jitter = Loader.loadFloat(data.Attribute("jitter"), 0f);
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
