using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TerrainBaseLayer : TerrainLayer
    {
        public TerrainBaseLayer(LayerProperties properties = null) : base()
        {
            // Default options
            if (properties == null)
            {
                properties = new BaseOptions("");
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
        public override TerrainLayer clone()
        {
            return new TerrainBaseLayer(_properties.clone());
        }
    }
}
