using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class MaterialPerlinLayer : MaterialLayer
    {
        private Vector2 _position;
        private float _scale;
        private float _frequency;
        private float _gain;
        private float _lacunarity;
        private float _multiplier;
        private Vector2 _fbmOffset;
        private Color _colorLow;
        private Color _colorHigh;
        private int _iterations;
        private LayerBlendType _blendType;
        private bool _invert;
        private int _seed;

        public Vector2 position { get { return _position; } set { _position = value; } }
        public float scale { get { return _scale; } set { _scale = value; } }
        public float frequency { get { return _frequency; } set { _frequency = value; } }
        public float gain { get { return _gain; } set { _gain = value; } }
        public float lacunarity { get { return _lacunarity; } set { _lacunarity = value; } }
        public float multiplier { get { return _multiplier; } set { _multiplier = value; } }
        public Vector2 fbmOffset { get { return _fbmOffset; } set { _fbmOffset = value; } }
        virtual public Color colorLow { get { return _colorLow; } set { _colorLow = value; } }
        virtual public Color colorHigh { get { return _colorHigh; } set { _colorHigh = value; } }
        public int iterations { get { return _iterations; } set { _iterations = value; } }
        public LayerBlendType blendType { get { return _blendType; } set { _blendType = value; } }
        public bool invert { get { return _invert; } set { _invert = value; } }
        public int seed { get { return _seed; } set { _seed = value; } }

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("scale", _scale);
                d.SetAttributeValue("frequency", _frequency);
                d.SetAttributeValue("gain", _gain);
                d.SetAttributeValue("lacunarity", _lacunarity);
                d.SetAttributeValue("multiplier", _multiplier);
                d.SetAttributeValue("fbm_offset", _fbmOffset);
                d.SetAttributeValue("color_low", _colorLow);
                d.SetAttributeValue("color_high", _colorHigh);
                d.SetAttributeValue("iterations", _iterations);
                d.SetAttributeValue("blend_type", _blendType.ToString().ToLower());
                d.SetAttributeValue("invert", _invert);
                d.SetAttributeValue("seed", _seed);
                return d;
            }
        }

        // Create new
        public MaterialPerlinLayer()
            : base("perlin", true)
        {
            _position = Vector2.Zero;
            _scale = 1f;
            _frequency = 1.2f;
            _gain = 0.5f;
            _lacunarity = 2f;
            _multiplier = 1f;
            _fbmOffset = Vector2.Zero;
            _colorLow = Color.Black;
            _colorHigh = Color.White;
            _iterations = 1;
            _blendType = LayerBlendType.Opaque;
            _invert = false;
            _seed = 12345;
        }

        // Create from xml
        public MaterialPerlinLayer(XElement data) : base(data)
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _scale = Loader.loadFloat(data.Attribute("scale"), 1);
            _frequency = Loader.loadFloat(data.Attribute("frequency"), 1);
            _gain = Loader.loadFloat(data.Attribute("gain"), 0.5f);
            _lacunarity = Loader.loadFloat(data.Attribute("lacunarity"), 1.8f);
            _multiplier = Loader.loadFloat(data.Attribute("multiplier"), multiplier);
            _fbmOffset = Loader.loadVector2(data.Attribute("fbm_offset"), Vector2.Zero);
            _colorLow = Loader.loadColor(data.Attribute("color_low"), Color.Black);
            _colorHigh = Loader.loadColor(data.Attribute("color_high"), Color.White);
            _iterations = Loader.loadInt(data.Attribute("iterations"), 0);
            _blendType = (LayerBlendType)Loader.loadEnum(typeof(LayerBlendType), data.Attribute("blend_type"), (int)LayerBlendType.Opaque);
            _invert = Loader.loadBool(data.Attribute("invert"), false);
            _seed = Loader.loadInt(data.Attribute("seed"), 12345);
        }

        // Clone
        public override MaterialLayer clone()
        {
            return new MaterialPerlinLayer(data);
        }
    }
}
