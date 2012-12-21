using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
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
        private static Dictionary<string, ResourceObject> _materialResources;
        private static Dictionary<string, ResourceObject> _itemResources;
        private static Dictionary<string, ResourceObject> _characterResources;
        private static Dictionary<string, ResourceObject> _dialogueResources;
        private static Dictionary<string, ResourceObject> _levelResources;
        private static Dictionary<string, Texture2D> _cachedTextures;

        // Initialize -- Called once when the application starts
        public static void initialize(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _materialResources = new Dictionary<string, ResourceObject>();
            _itemResources = new Dictionary<string, ResourceObject>();
            _characterResources = new Dictionary<string, ResourceObject>();
            _dialogueResources = new Dictionary<string, ResourceObject>();
            _levelResources = new Dictionary<string, ResourceObject>();
            _cachedTextures = new Dictionary<string, Texture2D>();
        }

        // Checks to see if a resource has been loaded
        public static bool isResourceLoaded(string uid)
        {
            // Check texture resources
            if (_cachedTextures.ContainsKey(uid))
                return true;

            // Check material resources
            if (_materialResources.ContainsKey(uid))
                return true;

            // Check item resources
            if (_itemResources.ContainsKey(uid))
                return true;

            // Check character resources
            if (_characterResources.ContainsKey(uid))
                return true;

            // Check dialogue resources
            if (_dialogueResources.ContainsKey(uid))
                return true;

            // Check level resources
            if (_levelResources.ContainsKey(uid))
                return true;

            return false;
        }

        // Get texture -- Try to get the texture from the cache before loading it from file
        public static Texture2D getTexture(string textureUID)
        {
            Texture2D texture = null;
            if (!_cachedTextures.TryGetValue(textureUID, out texture))
            {
                // Find file
                string[] textureFiles = Directory.GetFiles(TEXTURE_PATH, textureUID);
                if (textureFiles.Length == 0)
                    throw new ResourceNotFoundException(textureUID);

                // Load texture
                using (FileStream fs = new FileStream(textureFiles[0], FileMode.Open))
                    texture = Texture2D.FromStream(_graphicsDevice, fs);

                // Add it to cache
                _cachedTextures[textureUID] = texture;
            }
            return texture;
        }

        // Load materials
        public static void loadMaterials()
        {
            _materialResources.Clear();

            using (FileStream fs = new FileStream(String.Format("{0}\\materials.xml", RESOURCE_PATH), FileMode.Open))
            {
                XElement data = XElement.Load(fs);

                foreach (XElement materialData in data.Elements("Material"))
                {
                    ResourceObject resource = new ResourceObject(materialData);
                    _materialResources[resource.uid] = resource;
                }
            }
        }

        // Load items
        public static void loadItems()
        {
            _itemResources.Clear();

            using (FileStream fs = new FileStream(String.Format("{0}\\items.xml", RESOURCE_PATH), FileMode.Open))
            {
                XElement data = XElement.Load(fs);

                foreach (XElement itemData in data.Elements("Item"))
                {
                    ResourceObject resource = new ResourceObject(itemData);
                    _itemResources[resource.uid] = resource;
                }
            }
        }

        // Load characters
        public static void loadCharacters()
        {
            _characterResources.Clear();

            using (FileStream fs = new FileStream(String.Format("{0}\\characters.xml", RESOURCE_PATH), FileMode.Open))
            {
                XElement data = XElement.Load(fs);

                foreach (XElement characterData in data.Elements("Character"))
                {
                    ResourceObject resource = new ResourceObject(characterData);
                    _characterResources[resource.uid] = resource;
                }
            }
        }

        // Load dialogue
        public static void loadDialogue()
        {
            _dialogueResources.Clear();

            using (FileStream fs = new FileStream(String.Format("{0}\\dialogue.xml", RESOURCE_PATH), FileMode.Open))
            {
                XElement data = XElement.Load(fs);

                foreach (XElement dialogueData in data.Elements("Dialogue"))
                {
                    ResourceObject resource = new ResourceObject(dialogueData);
                    _dialogueResources[resource.uid] = resource;
                }
            }
        }
    }
}
