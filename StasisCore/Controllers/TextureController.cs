using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisCore.Controllers
{
    public class TextureController
    {
        private static GraphicsDevice _graphicsDevice;
        public static GraphicsDevice graphicsDevice { get { return _graphicsDevice; } set { _graphicsDevice = value; } }

        // textures[category][tag]
        private static Dictionary<string, Dictionary<string, TextureResource>> _resources = new Dictionary<string, Dictionary<string, TextureResource>>();
        private static Dictionary<string, Dictionary<string, Texture2D>> _textures = new Dictionary<string,Dictionary<string,Texture2D>>();

        private static string _textureDirectory = "TextureResources";
        public static string textureDirectory { get { return _textureDirectory; } set { _textureDirectory = value; } }

        // addResources
        public static void addResources(List<TextureResource> resources)
        {
            // Resources
            foreach (TextureResource resource in resources)
                addResource(resource);
        }
        public static void addResource(TextureResource resource)
        {
            if (!_resources.ContainsKey(resource.category))
                _resources[resource.category] = new Dictionary<string, TextureResource>();
            if (!_resources[resource.category].ContainsKey(resource.tag))
                _resources[resource.category][resource.tag] = resource;
        }

        // getTexture
        public static Texture2D getTexture(string tag)
        {
            Texture2D texture = null;

            // Discover category name
            foreach (KeyValuePair<string, Dictionary<string, TextureResource>> rowPair in _resources)
            {
                if (rowPair.Value.ContainsKey(tag))
                    texture = getTexture(rowPair.Key, tag);
            }

            return texture;
        }
        public static Texture2D getTexture(string category, string tag)
        {
            Texture2D texture;

            // Create row if necessary
            if (!_textures.ContainsKey(category))
                _textures[category] = new Dictionary<string, Texture2D>();

            if (_textures[category].TryGetValue(tag, out texture))
                return texture;
            else
            {
                // Load texture
                string filePath = _resources[category][tag].getFullPath(_textureDirectory);
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    _textures[category][tag] = Texture2D.FromStream(_graphicsDevice, stream);
                }
                return _textures[category][tag];
            }
        }
    }
}
