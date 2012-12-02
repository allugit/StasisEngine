using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TerrainBaseLayer : TerrainLayer
    {
        private string _textureTag;

        [CategoryAttribute("General")]
        [DisplayName("Texture Tag")]
        public string textureTag { get { return _textureTag; } set { _textureTag = value; } }

        public TerrainBaseLayer(string textureTag) : base()
        {
            _textureTag = textureTag;
            _type = TerrainLayerType.Base;
        }

        // Default string
        public override string ToString()
        {
            return "Base Layer";
        }

        // clone
        public override TerrainLayer clone()
        {
            return new TerrainBaseLayer(_textureTag);
        }
    }
}
