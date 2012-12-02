using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TerrainOutlineLayer : TerrainLayer
    {
        private Vector2 _normal;

        [CategoryAttribute("General")]
        [DisplayName("Outline Normal")]
        public Vector2 normal
        {
            get { return _normal; } 
            set
            {
                Vector2 val = value;
                val.Normalize();
                _normal = val;
            } 
        }

        public TerrainOutlineLayer(Vector2 normal)
            : base()
        {
            _normal = normal;
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
            return new TerrainOutlineLayer(_normal);
        }
    }
}
