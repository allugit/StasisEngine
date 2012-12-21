using System;
using System.IO;
using StasisCore.Controllers;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Resources
{
    public class TextureResourceObject : BaseResourceObject
    {
        protected string _filePath;

        public string filePath { get { return _filePath; } }
        public Texture2D texture { get { return ResourceController.getTexture(this); } }

        public TextureResourceObject(string filePath) : base(Path.GetFileNameWithoutExtension(filePath))
        {
            _filePath = filePath;
        }
    }
}
