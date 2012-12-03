using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TerrainPrimitivesLayerResource : TerrainLayerResource
    {
        public TerrainPrimitivesLayerResource(LayerProperties properties = null) : base()
        {
            // Default options
            if (properties == null)
            {
                properties = new PrimitivesProperties("");
            }

            _properties = properties;
            _type = TerrainLayerType.Base;
        }

        // Default string
        public override string ToString()
        {
            return "Base Layer";
        }

        // clone
        public override TerrainLayerResource clone()
        {
            return new TerrainPrimitivesLayerResource(_properties.clone());
        }
    }
}
