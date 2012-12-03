using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class TemporaryTextureResource : TextureResource
    {
        private string _filePath;

        [Browsable(false)]
        public string filePath { get { return _filePath; } set { _filePath = value; } }

        [DisplayName("File Name")]
        public string fileName { get { return Path.GetFileName(_filePath); } }

        public TemporaryTextureResource(string filePath)
            : base("", "")
        {
            _filePath = filePath;
        }
    }
}
