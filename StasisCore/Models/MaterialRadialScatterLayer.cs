using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Resources;

namespace StasisCore.Models
{
    public class MaterialRadialScatterLayer : MaterialScatterLayer
    {
        protected float _a;
        protected float _b;
        protected float _intersections;
        protected float _maxRadius;
        protected int _arms;
        protected bool _twinArms;
        protected bool _flipArms;
        protected bool _useAbsoluteTextureAngle;
        protected float _absoluteTextureAngle;
        protected float _relativeTextureAngle;
        protected float _textureAngleJitter;
        protected float _jitter;
        protected float _centerJitter;
        protected Vector2 _centerOffset;
        protected Color _baseColor;
        protected int _randomRed;
        protected int _randomGreen;
        protected int _randomBlue;
        protected int _randomAlpha;

        public float a { get { return _a; } set { _a = value; } }
        public float b { get { return _b; } set { _b = value; } }
        public float intersections { get { return _intersections; } set { _intersections = value; } }
        public float maxRadius { get { return _maxRadius; } set { _maxRadius = value; } }
        public int arms { get { return _arms; } set { _arms = value; } }
        public bool twinArms { get { return _twinArms; } set { _twinArms = value; } }
        public bool flipArms { get { return _flipArms; } set { _flipArms = value; } }
        public bool useAbsoluteTextureAngle { get { return _useAbsoluteTextureAngle; } set { _useAbsoluteTextureAngle = value; } }
        public float absoluteTextureAngle { get { return _absoluteTextureAngle; } set { _absoluteTextureAngle = value; } }
        public float relativeTextureAngle { get { return _relativeTextureAngle; } set { _relativeTextureAngle = value; } }
        public float textureAngleJitter { get { return _textureAngleJitter; } set { _textureAngleJitter = value; } }
        public float jitter { get { return _jitter; } set { _jitter = value; } }
        public float centerJitter { get { return _centerJitter; } set { _centerJitter = value; } }
        public Vector2 centerOffset { get { return _centerOffset; } set { _centerOffset = value; } }
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
                d.SetAttributeValue("a", _a);
                d.SetAttributeValue("b", _b);
                d.SetAttributeValue("intersections", _intersections);
                d.SetAttributeValue("max_radius", _maxRadius);
                d.SetAttributeValue("arms", _arms);
                d.SetAttributeValue("twin_arms", _twinArms);
                d.SetAttributeValue("flip_arms", _flipArms);
                d.SetAttributeValue("use_absolute_texture_angle", _useAbsoluteTextureAngle);
                d.SetAttributeValue("absolute_texture_angle", _absoluteTextureAngle);
                d.SetAttributeValue("relative_texture_angle", _relativeTextureAngle);
                d.SetAttributeValue("texture_angle_jitter", _textureAngleJitter);
                d.SetAttributeValue("jitter", _jitter);
                d.SetAttributeValue("center_jitter", _centerJitter);
                d.SetAttributeValue("center_offset", _centerOffset);
                d.SetAttributeValue("base_color", _baseColor);
                d.SetAttributeValue("random_red", _randomRed);
                d.SetAttributeValue("random_green", _randomGreen);
                d.SetAttributeValue("random_blue", _randomBlue);
                d.SetAttributeValue("random_alpha", _randomAlpha);
                return d;
            }
        }

        public MaterialRadialScatterLayer()
            : base("radial_scatter")
        {
            _a = 1f;
            _b = 1f;
            _intersections = 32f;
            _maxRadius = 64f;
            _arms = 9;
            _twinArms = false;
            _flipArms = false;
            _useAbsoluteTextureAngle = false;
            _absoluteTextureAngle = 0f;
            _relativeTextureAngle = 0f;
            _textureAngleJitter = 0f;
            _jitter = 0f;
            _centerJitter = 0f;
            _centerOffset = Vector2.Zero;
            _baseColor = Color.White;
        }

        public MaterialRadialScatterLayer(XElement data)
            : base(data)
        {
            _a = Loader.loadFloat(data.Attribute("a"), 1f);
            _b = Loader.loadFloat(data.Attribute("b"), 1f);
            _intersections = Loader.loadFloat(data.Attribute("intersections"), 32f);
            _maxRadius = Loader.loadFloat(data.Attribute("max_radius"), 64f);
            _arms = Loader.loadInt(data.Attribute("arms"), 9);
            _twinArms = Loader.loadBool(data.Attribute("twin_arms"), false);
            _flipArms = Loader.loadBool(data.Attribute("flip_arms"), false);
            _useAbsoluteTextureAngle = Loader.loadBool(data.Attribute("use_absolute_texture_angle"), false);
            _absoluteTextureAngle = Loader.loadFloat(data.Attribute("absolute_texture_angle"), 0f);
            _relativeTextureAngle = Loader.loadFloat(data.Attribute("relative_texture_angle"), 0f);
            _textureAngleJitter = Loader.loadFloat(data.Attribute("texture_angle_jitter"), 0f);
            _jitter = Loader.loadFloat(data.Attribute("jitter"), 0f);
            _centerJitter = Loader.loadFloat(data.Attribute("center_jitter"), 0f);
            _centerOffset = Loader.loadVector2(data.Attribute("center_offset"), Vector2.Zero);
            _baseColor = Loader.loadColor(data.Attribute("base_color"), Color.White);
            _randomRed = Loader.loadInt(data.Attribute("random_red"), 0);
            _randomGreen = Loader.loadInt(data.Attribute("random_green"), 0);
            _randomBlue = Loader.loadInt(data.Attribute("random_blue"), 0);
            _randomAlpha = Loader.loadInt(data.Attribute("random_alpha"), 0);
        }

        // Clone
        public override MaterialLayer clone()
        {
            return new MaterialRadialScatterLayer(data);
        }
    }
}
