using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TerrainOutlineLayer : TerrainLayer
    {
        private OutlineOptions _outlineOptions;

        public OutlineOptions outlineOptions { get { return _outlineOptions; } set { _outlineOptions = value; } }

        public TerrainOutlineLayer(OutlineOptions outlineOptions = null)
            : base()
        {
            // Default options
            if (outlineOptions == null)
            {
                outlineOptions = new OutlineOptions(Vector2.Zero);
            }

            _outlineOptions = outlineOptions;
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
            return new TerrainOutlineLayer(_outlineOptions.clone());
        }
    }
}
