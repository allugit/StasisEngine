using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class TerrainDecalSpatterLayerResource : TerrainLayerResource
    {
        public TerrainDecalSpatterLayerResource(LayerProperties properties = null)
            : base()
        {
            // Default options
            if (properties == null)
            {
                properties = new DecalProperties();
            }

            _properties = properties;
            _type = TerrainLayerType.DecalSpatter;
        }

        // Default string
        public override string ToString()
        {
            return "Decale Spatter Layer";
        }

        // clone
        public override TerrainLayerResource clone()
        {
            return new TerrainDecalSpatterLayerResource(_properties.clone());
        }
    }
}
