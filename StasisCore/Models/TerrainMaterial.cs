using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TerrainMaterial : Material
    {
        private string _textureTag;
        private List<NoiseOptions> _noiseLayerOptions;

        [CategoryAttribute("General")]
        [DisplayName("Texture Tag")]
        public string textureTag { get { return _textureTag; } set { _textureTag = value; } }

        // Constructor
        public TerrainMaterial(string name, List<NoiseOptions> noiseLayerOptions) : base(name)
        {
            _type = MaterialType.Terrain;
            _noiseLayerOptions = noiseLayerOptions;
        }

        // clone
        public override Material clone()
        {
            return new TerrainMaterial(_name, NoiseOptions.copyFrom(_noiseLayerOptions));
        }
    }
}
