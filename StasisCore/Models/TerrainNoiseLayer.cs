using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TerrainNoiseLayer : TerrainLayer
    {
        public TerrainNoiseLayer(LayerProperties properties = null)
            : base()
        {
            // Default options
            if (properties == null)
            {
                properties = new NoiseOptions(
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

            _properties = properties;
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
            return new TerrainNoiseLayer(_properties.clone());
        }
    }
}
