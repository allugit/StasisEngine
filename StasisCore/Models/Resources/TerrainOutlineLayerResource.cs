using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TerrainOutlineLayerResource : TerrainLayerResource
    {
        public TerrainOutlineLayerResource(LayerProperties properties = null)
            : base()
        {
            // Default options
            if (properties == null)
            {
                properties = new OutlineProperties(Vector2.Zero);
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
        public override TerrainLayerResource clone()
        {
            return new TerrainOutlineLayerResource(_properties.clone());
        }
    }
}
