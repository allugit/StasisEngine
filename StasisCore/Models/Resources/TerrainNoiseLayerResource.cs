using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TerrainNoiseLayerResource : TerrainLayerResource
    {
        public TerrainNoiseLayerResource(LayerProperties properties = null)
            : base()
        {
            // Default options
            if (properties == null)
            {
                properties = new NoiseProperties(
                    NoiseType.Perlin,
                    TerrainBlendType.Opaque,
                    Vector2.Zero,
                    1f,
                    Vector2.Zero,
                    1.1f,
                    0.5f,
                    2f,
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
        public override TerrainLayerResource clone()
        {
            return new TerrainNoiseLayerResource(_properties.clone());
        }
    }
}
