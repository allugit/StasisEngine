using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class TerrainDecalSpatterLayerResource : TerrainLayerResource
    {
        public TerrainDecalSpatterLayerResource(List<TerrainLayerResource> layers = null, LayerProperties properties = null, bool enabled = true)
            : base(layers, enabled)
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
            List<TerrainLayerResource> layersCopy = new List<TerrainLayerResource>();
            foreach (TerrainLayerResource layer in _layers)
                layersCopy.Add(layer.clone());

            return new TerrainDecalSpatterLayerResource(layersCopy, _properties.clone());
        }
    }
}
