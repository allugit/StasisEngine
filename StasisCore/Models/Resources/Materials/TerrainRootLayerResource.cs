using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class TerrainRootLayerResource : TerrainGroupLayerResource
    {
        public TerrainRootLayerResource(List<TerrainLayerResource> layers = null, LayerProperties properties = null)
            : base(layers, properties, true)
        {
            // Default properties
            if (properties == null)
                properties = new RootProperties();

            _properties = properties;
            _type = TerrainLayerType.Root;
        }

        // Default string
        public override string ToString()
        {
            return "Root";
        }

        // clone
        public override TerrainLayerResource clone()
        {
            List<TerrainLayerResource> layersCopy = new List<TerrainLayerResource>();
            foreach (TerrainLayerResource layer in _layers)
                layersCopy.Add(layer.clone());
            return new TerrainRootLayerResource(layersCopy, _properties.clone());
        }
    }
}
