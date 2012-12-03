using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TerrainMaterialResource : MaterialResource
    {
        private List<TerrainLayerResource> _layers;

        [Browsable(false)]
        public List<TerrainLayerResource> layers { get { return _layers; } set { _layers = layers; } }

        // Constructor
        public TerrainMaterialResource(string name, List<TerrainLayerResource> layers) : base(name)
        {
            _layers = layers;
            _type = MaterialType.Terrain;
        }

        // clone
        public override MaterialResource clone()
        {
            return new TerrainMaterialResource(_name, TerrainLayerResource.copyFrom(_layers));
        }
    }
}
