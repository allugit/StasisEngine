using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class TemporaryTextureResource : TextureResource
    {
        private string _sourcePath;

        [Browsable(false)]
        public string sourcePath { get { return _sourcePath; } set { _sourcePath = value; } }

        [DisplayName("Source File")]
        public string sourceFile { get { return Path.GetFileName(_sourcePath); } }

        public TemporaryTextureResource(string filePath)
            : base("", "", "")
        {
            _sourcePath = filePath;
        }

        // prepareForCopy
        public void prepareForCopy()
        {
            _extension = Path.GetExtension(_sourcePath);
        }
    }
}
