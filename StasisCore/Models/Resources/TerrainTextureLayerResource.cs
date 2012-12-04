using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class TerrainTextureLayerResource : TerrainLayerResource
    {
        public TerrainTextureLayerResource(LayerProperties properties = null)
            : base()
        {
            // Default options
            if (properties == null)
                properties = new TextureProperties(TerrainBlendType.Opaque, 1, 1);

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
            return new TerrainTextureLayerResource(_properties.clone());
        }
    }
}
