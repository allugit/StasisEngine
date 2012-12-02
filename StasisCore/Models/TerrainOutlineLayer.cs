using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TerrainOutlineLayer : TerrainLayer
    {
        public TerrainOutlineLayer(LayerProperties properties = null)
            : base()
        {
            // Default options
            if (properties == null)
            {
                properties = new OutlineOptions(Vector2.Zero);
            }

            _properties = properties;
            _type = TerrainLayerType.Outline;
        }

        // Default string
        public override string ToString()
        {
            return "Outline Layer";
        }

        // clone
        public override TerrainLayer clone()
        {
            return new TerrainOutlineLayer(_properties.clone());
        }
    }
}
