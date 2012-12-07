using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class TerrainGroupLayerResource : TerrainLayerResource
    {
        protected List<TerrainLayerResource> _layers;
        public List<TerrainLayerResource> layers { get { return _layers; } }

        public TerrainGroupLayerResource(List<TerrainLayerResource> layers = null, LayerProperties properties = null, bool enabled = true)
            : base(enabled)
        {
            // Default layers
            if (layers == null)
                layers = new List<TerrainLayerResource>();

            // Default properties
            if (properties == null)
                properties = new GroupProperties(TerrainBlendType.Opaque);

            _layers = layers;
            _properties = properties;
            _type = TerrainLayerType.Group;
        }

        // Default string
        public override string ToString()
        {
            return "Group";
        }

        // clone
        public override TerrainLayerResource clone()
        {
            List<TerrainLayerResource> layersCopy = new List<TerrainLayerResource>();
            foreach (TerrainLayerResource layer in _layers)
                layersCopy.Add(layer.clone());
            return new TerrainGroupLayerResource(layersCopy, _properties.clone(), enabled);
        }
    }
}
