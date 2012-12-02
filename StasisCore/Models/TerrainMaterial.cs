using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TerrainMaterial : Material
    {
        // Constructor
        public TerrainMaterial(string name) : base(name)
        {
            _type = MaterialType.Terrain;
        }

        // clone
        public override Material clone()
        {
            return new TerrainMaterial(_name);
        }
    }
}
