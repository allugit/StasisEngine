using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TerrainMaterialResource : MaterialResource
    {
        private TerrainRootLayerResource _rootLayer;
        public TerrainRootLayerResource rootLayer { get { return _rootLayer; } }

        // Constructor
        public TerrainMaterialResource(string name, TerrainRootLayerResource rootLayer = null) : base(name)
        {
            // Default root layer
            if (rootLayer == null)
                rootLayer = new TerrainRootLayerResource();

            _rootLayer = rootLayer;
            _type = MaterialType.Terrain;
        }

        // clone
        public override MaterialResource clone()
        {
            return new TerrainMaterialResource(_name, _rootLayer.clone() as TerrainRootLayerResource);
        }
    }
}
