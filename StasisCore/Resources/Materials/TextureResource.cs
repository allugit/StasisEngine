using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TextureResource
    {
        private string _tag;
        private string _category;
        private string _filePath;

        public TextureResource(string tag, string category, string filePath)
        {
            _tag = tag;
            _category = category;
            _filePath = filePath;
        }

        // clone
        public TextureResource clone()
        {
            return new TextureResource(_tag, _category, _filePath);
        }
    }
}
