using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Resources;

namespace StasisCore.Controllers
{
    /*
     * Resource controller
     *   Loads resources from file and populates resource objects with the data. Resource objects are
     *   stored in a dictionary, using their UIDs as keys.
     */
    public class ResourceController
    {
        public const string RESOURCE_PATH = @"E:\StasisResources";
        public const string LEVEL_PATH =    @"E:\StasisResources\levels";
        public const string TEXTURE_PATH =  @"E:\StasisResources\textures";

        private static GraphicsDevice _graphicsDevice;
        private static Dictionary<string, ResourceObject> _resources;
        private static Dictionary<string, TextureResourceObject> _textureResources;
        private static Dictionary<string, Texture2D> _cachedTextures;

        // Initialize -- Called once when the application starts
        public static void initialize(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _resources = new Dictionary<string, ResourceObject>();
            _textureResources = new Dictionary<string, TextureResourceObject>();
            _cachedTextures = new Dictionary<string, Texture2D>();
        }

        // Checks to see if a resource has been loaded
        public static bool resourceLoaded(string uid)
        {
            return _resources.ContainsKey(uid) || _textureResources.ContainsKey(uid);
        }

        // Get texture -- Try to get the texture from the cache before loading it from file
        public static Texture2D getTexture(TextureResourceObject resource)
        {
            Texture2D texture = null;
            if (!_cachedTextures.TryGetValue(resource.uid, out texture))
            {
                // Load texture
                using (FileStream fs = new FileStream(resource.filePath, FileMode.Open))
                    texture = Texture2D.FromStream(_graphicsDevice, fs);

                // Add it to cache
                _cachedTextures[resource.uid] = texture;
            }
            return texture;
        }
    }
}
