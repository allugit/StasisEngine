using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class NoiseOptions
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
        public NoiseType NoiseType { get { return _noiseType; } set { _noiseType = value; } }
        [CategoryAttribute("General")]
        public Vector2 Position { get { return _position; } set { _position = value; } }
        [CategoryAttribute("General")]
        public float Scale { get { return _scale; } set { _scale = value; } }
        [CategoryAttribute("General")]
        public float Multiplier { get { return _multiplier; } set { _multiplier = value; } }
        [CategoryAttribute("General")]
        public Color ColorRangeLow { get { return _colorRangeLow; } set { _colorRangeLow = value; } }
        [CategoryAttribute("General")]
        public Color ColorRangeHigh { get { return _colorRangeHigh; } set { _colorRangeHigh = value; } }
        [CategoryAttribute("Fractional Brownian Motion")]
        public float NoiseFrequency { get { return _noiseFrequency; } set { _noiseFrequency = value; } }
        [CategoryAttribute("Fractional Brownian Motion")]
        public float NoiseGain { get { return _noiseGain; } set { _noiseGain = value; } }
        [CategoryAttribute("Fractional Brownian Motion")]
        public float NoiseLacunarity { get { return _noiseLacunarity; } set { _noiseLacunarity = value; } }
        [CategoryAttribute("Fractional Brownian Motion")]
        public Vector2 FBMOffset { get { return _fbmOffset; } set { _fbmOffset = value; } }
        [CategoryAttribute("Fractional Brownian Motion")]
        public float FBMScale { get { return _fbmScale; } set { _fbmScale = value; } }
        [CategoryAttribute("Fractional Brownian Motion")]
        public int Iterations { get { return _iterations; } set { _iterations = value; } }

        public NoiseOptions(
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
    }
}
