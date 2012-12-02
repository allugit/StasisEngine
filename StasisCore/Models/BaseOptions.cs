using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class BaseOptions : LayerProperties
    {
        private string _textureTag;

        [CategoryAttribute("General")]
        [DisplayName("Texture Tag")]
        public string textureTag { get { return _textureTag; } set { _textureTag = value; } }

        public BaseOptions(string textureTag)
            : base()
        {
            _textureTag = textureTag;
        }

        // clone
        public override LayerProperties clone()
        {
            return new BaseOptions(_textureTag);
        }
    }
}
