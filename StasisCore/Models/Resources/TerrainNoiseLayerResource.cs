using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TerrainNoiseLayerResource : TerrainLayerResource
    {
        public TerrainNoiseLayerResource(List<TerrainLayerResource> layers = null, LayerProperties properties = null, bool enabled = true)
            : base(layers, enabled)
        {
            // Default options
            if (properties == null)
            {
                properties = new NoiseProperties(
                    NoiseType.Perlin,
                    TerrainBlendType.Opaque,
                    WorleyFeature.F1,
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
            List<TerrainLayerResource> layersCopy = new List<TerrainLayerResource>();
            foreach (TerrainLayerResource layer in _layers)
                layersCopy.Add(layer.clone());
            return new TerrainNoiseLayerResource(layersCopy, _properties.clone());
        }
    }
}
