using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class TerrainTextureLayerResource : TerrainLayerResource
    {
        public TerrainTextureLayerResource(List<TerrainLayerResource> layers = null, LayerProperties properties = null, bool enabled = true)
            : base(layers, enabled)
        {
            // Default options
            if (properties == null)
                properties = new TextureProperties(TerrainBlendType.Opaque, 1, 1, "");

            _properties = properties;
            _type = TerrainLayerType.Texture;
        }

        // Default string
        public override string ToString()
        {
            return "Texture Layer";
        }

        // clone
        public override TerrainLayerResource clone()
        {
            List<TerrainLayerResource> layersCopy = new List<TerrainLayerResource>();
            foreach (TerrainLayerResource layer in _layers)
                layersCopy.Add(layer.clone());
            return new TerrainTextureLayerResource(layersCopy, _properties.clone());
        }
    }
}
