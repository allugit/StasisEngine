using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class TerrainDecalSpatterLayer : TerrainLayer
    {
        public TerrainDecalSpatterLayer(LayerProperties properties = null)
            : base()
        {
            // Default options
            if (properties == null)
            {
                properties = new DecalSpatterOptions();
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
        public override TerrainLayer clone()
        {
            return new TerrainDecalSpatterLayer(_properties.clone());
        }
    }
}
