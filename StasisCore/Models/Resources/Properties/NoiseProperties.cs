using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class NoiseProperties : LayerProperties
    {
        private NoiseType _noiseType;
        private Vector2 _position;
        private float _scale;
        private Vector2 _fbmOffset;
        private float _noiseFrequency;
        private float _noiseGain;
        private float _noiseLacunarity;
        private float _multiplier;
        private float _fbmScale;
        private Color _colorRangeLow;
        private Color _colorRangeHigh;
        private int _iterations;

        [CategoryAttribute("General")]
        [DisplayName("Noise Type")]
        public NoiseType noiseType { get { return _noiseType; } set { _noiseType = value; } }

        [CategoryAttribute("General")]
        [DisplayName("Position")]
        public Vector2 position { get { return _position; } set { _position = value; } }

        [CategoryAttribute("General")]
        [DisplayName("Scale")]
        public float scale { get { return _scale; } set { _scale = value; } }

        [CategoryAttribute("General")]
        [DisplayName("Multiplier")]
        public float multiplier { get { return _multiplier; } set { _multiplier = value; } }

        [CategoryAttribute("General")]
        [DisplayName("Low Color")]
        public Color colorRangeLow { get { return _colorRangeLow; } set { _colorRangeLow = value; } }

        [CategoryAttribute("General")]
        [DisplayName("High Color")]
        public Color colorRangeHigh { get { return _colorRangeHigh; } set { _colorRangeHigh = value; } }

        [CategoryAttribute("Fractional Brownian Motion")]
        [DisplayName("Noise Frequency")]
        public float noiseFrequency { get { return _noiseFrequency; } set { _noiseFrequency = value; } }

        [CategoryAttribute("Fractional Brownian Motion")]
        [DisplayName("Noise Gain")]
        public float noiseGain { get { return _noiseGain; } set { _noiseGain = value; } }

        [CategoryAttribute("Fractional Brownian Motion")]
        [DisplayName("Noise Lacunarity")]
        public float noiseLacunarity { get { return _noiseLacunarity; } set { _noiseLacunarity = value; } }

        [CategoryAttribute("Fractional Brownian Motion")]
        [DisplayName("Offset")]
        public Vector2 fbmOffset { get { return _fbmOffset; } set { _fbmOffset = value; } }

        [CategoryAttribute("Fractional Brownian Motion")]
        [DisplayName("Coordinate Scale")]
        public float fbmScale { get { return _fbmScale; } set { _fbmScale = value; } }

        [CategoryAttribute("Fractional Brownian Motion")]
        [DisplayName("Iterations")]
        public int iterations { get { return _iterations; } set { _iterations = value; } }

        public NoiseProperties(
            NoiseType noiseType,
            Vector2 position,
            float scale,
            Vector2 fbmOffset,
            float noiseFrequency,
            float noiseGain,
            float noiseLacunarity,
            float multiplier,
            float fbmScale,
            Color colorRangeLow,
            Color colorRangeHigh,
            int iterations)
            : base()
        {
            _noiseType = noiseType;
            _position = position;
            _scale = scale;
            _fbmOffset = fbmOffset;
            _noiseFrequency = noiseFrequency;
            _noiseGain = noiseGain;
            _noiseLacunarity = noiseLacunarity;
            _multiplier = multiplier;
            _fbmScale = fbmScale;
            _colorRangeLow = colorRangeLow;
            _colorRangeHigh = colorRangeHigh;
            _iterations = iterations;
        }

        // copyFrom -- clones a list
        public static List<LayerProperties> copyFrom(List<NoiseProperties> list)
        {
            List<LayerProperties> copy = new List<LayerProperties>();
            foreach (NoiseProperties options in list)
                copy.Add(options.clone());
            return copy;
        }

        // clone
        public override LayerProperties clone()
        {
            return new NoiseProperties(
                _noiseType,
                _position,
                _scale,
                _fbmOffset,
                _noiseFrequency,
                _noiseGain,
                _noiseLacunarity,
                _multiplier,
                _fbmScale,
                _colorRangeLow,
                _colorRangeHigh,
                _iterations);
        }
    }
}
