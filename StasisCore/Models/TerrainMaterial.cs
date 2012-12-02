using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TerrainMaterial : Material
    {
        private List<TerrainLayer> _layers;

        [Browsable(false)]
        public List<TerrainLayer> layers { get { return _layers; } set { _layers = layers; } }

        // Constructor
        public TerrainMaterial(string name, List<TerrainLayer> layers) : base(name)
        {
            _layers = layers;
            _type = MaterialType.Terrain;
        }

        // clone
        public override Material clone()
        {
            return new TerrainMaterial(_name, TerrainLayer.copyFrom(_layers));
        }
    }
}
