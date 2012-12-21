﻿using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Resources;

namespace StasisCore.Models
{
    public class MaterialNoiseLayer : MaterialLayer
    {
        private NoiseType _noiseType;
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
        private WorleyFeatureType _worleyFeature;

        public NoiseType noiseType { get { return _noiseType; } set { _noiseType = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public float scale { get { return _scale; } set { _scale = value; } }
        public float frequency { get { return _frequency; } set { _frequency = value; } }
        public float gain { get { return _gain; } set { _gain = value; } }
        public float lacunarity { get { return _lacunarity; } set { _lacunarity = value; } }
        public float multiplier { get { return _multiplier; } set { _multiplier = value; } }
        public Vector2 fbmOffset { get { return _fbmOffset; } set { _fbmOffset = value; } }
        public Color colorLow { get { return _colorLow; } set { _colorLow = value; } }
        public Color colorHigh { get { return _colorHigh; } set { _colorHigh = value; } }
        public int iterations { get { return _iterations; } set { _iterations = value; } }
        public LayerBlendType blendType { get { return _blendType; } set { _blendType = value; } }
        public bool invert { get { return _invert; } set { _invert = value; } }
        public WorleyFeatureType worleyFeature { get { return _worleyFeature; } set { _worleyFeature = value; } }

        public MaterialNoiseLayer(XElement data) : base(data)
        {
            _noiseType = (NoiseType)Enum.Parse(typeof(NoiseType), data.Attribute("noise_type").Value, true);
            _position = XmlLoadHelper.getVector2(data.Attribute("position").Value);
            _scale = float.Parse(data.Attribute("scale").Value);
            _frequency = float.Parse(data.Attribute("frequency").Value);
            _gain = float.Parse(data.Attribute("gain").Value);
            _lacunarity = float.Parse(data.Attribute("lacunarity").Value);
            _multiplier = float.Parse(data.Attribute("multiplier").Value);
            _fbmOffset = XmlLoadHelper.getVector2(data.Attribute("fbm_offset").Value);
            _colorLow = XmlLoadHelper.getColor(data.Attribute("color_low").Value);
            _colorHigh = XmlLoadHelper.getColor(data.Attribute("color_high").Value);
            _iterations = int.Parse(data.Attribute("iterations").Value);
            _blendType = (LayerBlendType)Enum.Parse(typeof(LayerBlendType), data.Attribute("blend_type").Value, true);
            _invert = bool.Parse(data.Attribute("invert").Value);
            _worleyFeature = (WorleyFeatureType)Enum.Parse(typeof(WorleyFeatureType), data.Attribute("worley_feature").Value, true);
        }
    }
}
