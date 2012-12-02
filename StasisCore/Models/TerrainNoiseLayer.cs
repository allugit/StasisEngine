using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class TerrainNoiseLayer : TerrainLayer
    {
        private NoiseOptions _noiseOptions;
        public NoiseOptions noiseOptions { get { return _noiseOptions; } set { _noiseOptions = value; } }

        public TerrainNoiseLayer(NoiseOptions noiseOptions)
            : base()
        {
            _noiseOptions = noiseOptions;
            _type = TerrainLayerType.Noise;
        }

        // Default string
        public override string ToString()
        {
            return "Noise Layer";
        }

        // clone
        public override TerrainLayer clone()
        {
            return new TerrainNoiseLayer(_noiseOptions.clone());
        }
    }
}
