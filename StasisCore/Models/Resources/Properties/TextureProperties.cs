using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TextureProperties : LayerProperties
    {
        private TerrainBlendType _blendType;
        private float _scale;
        private float _multiplier;
        private string _textureTag;

        [CategoryAttribute("Blending")]
        [DisplayName("Type")]
        public TerrainBlendType blendType { get { return _blendType; } set { _blendType = value; } }

        [CategoryAttribute("General")]
        [DisplayName("Scale")]
        public float scale { get { return _scale; } set { _scale = value; } }

        [CategoryAttribute("General")]
        [DisplayName("Multiplier")]
        public float multiplier { get { return _multiplier; } set { _multiplier = value; } }

        [CategoryAttribute("General")]
        [DisplayName("Texture Tag")]
        public string textureTag { get { return _textureTag; } set { _textureTag = value; } }

        public TextureProperties(TerrainBlendType blendType, float scale, float multiplier, string textureTag)
            : base()
        {
            _blendType = blendType;
            _scale = scale;
            _multiplier = multiplier;
            _textureTag = textureTag;
        }

        // copyFrom -- clones a list
        public static List<LayerProperties> copyFrom(List<TextureProperties> list)
        {
            List<LayerProperties> copy = new List<LayerProperties>();
            foreach (TextureProperties options in list)
                copy.Add(options.clone());
            return copy;
        }

        // clone
        public override LayerProperties clone()
        {
            return new TextureProperties(_blendType, _scale, _multiplier, _textureTag);
        }
    }
}
