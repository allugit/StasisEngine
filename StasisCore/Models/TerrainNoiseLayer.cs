using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TerrainNoiseLayer : TerrainLayer
    {
        private NoiseOptions _noiseOptions;
        public NoiseOptions noiseOptions { get { return _noiseOptions; } set { _noiseOptions = value; } }

        public TerrainNoiseLayer(NoiseOptions noiseOptions = null)
            : base()
        {
            // Default options
            if (noiseOptions == null)
            {
                noiseOptions = new NoiseOptions(
                    NoiseType.Perlin,
                    Vector2.Zero,
                    1f,
                    Vector2.Zero,
                    1.1f,
                    0.5f,
                    0.8f,
                    1f,
                    1f,
                    Color.Black,
                    Color.White,
                    1);
            }

            _noiseOptions = noiseOptions;
            _type = TerrainLayerType.Noise;
        }

        // Default string
        public override string ToString()
        {
            return "Noise Layer";
        }

        // clone
        public override TerrainLayer clone()
        {
            return new TerrainNoiseLayer(_noiseOptions.clone());
        }
    }
}
