using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class TerrainDecalSpatterLayer : TerrainLayer
    {
        private DecalSpatterOptions _decalSpatterOptions;
        public DecalSpatterOptions decalSpatterOptions { get { return _decalSpatterOptions; } set { _decalSpatterOptions = value; } }

        public TerrainDecalSpatterLayer(DecalSpatterOptions decalSpatterOptions)
            : base()
        {
            _decalSpatterOptions = decalSpatterOptions;
            _type = TerrainLayerType.DecalSpatter;
        }

        // clone
        public override TerrainLayer clone()
        {
            return new TerrainDecalSpatterLayer(_decalSpatterOptions.clone());
        }
    }
}
