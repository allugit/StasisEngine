using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class TerrainDecalSpatterLayer : TerrainLayer
    {
        private DecalSpatterOptions _decalSpatterOptions;
        public DecalSpatterOptions decalSpatterOptions { get { return _decalSpatterOptions; } set { _decalSpatterOptions = value; } }

        public TerrainDecalSpatterLayer(DecalSpatterOptions decalSpatterOptions = null)
            : base()
        {
            // Default options
            if (decalSpatterOptions == null)
            {
                decalSpatterOptions = new DecalSpatterOptions();
            }

            _decalSpatterOptions = decalSpatterOptions;
            _type = TerrainLayerType.DecalSpatter;
        }

        // Default string
        public override string ToString()
        {
            return "Decale Spatter Layer";
        }

        // clone
        public override TerrainLayer clone()
        {
            return new TerrainDecalSpatterLayer(_decalSpatterOptions.clone());
        }
    }
}
