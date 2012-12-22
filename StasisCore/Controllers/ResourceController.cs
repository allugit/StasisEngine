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

        private static GraphicsDevice _graphicsDevice;
        private static Dictionary<string, ResourceObject> _materialResources;
        private static Dictionary<string, ResourceObject> _itemResources;
        private static Dictionary<string, ResourceObject> _characterResources;
        private static Dictionary<string, ResourceObject> _dialogueResources;
        private static Dictionary<string, ResourceObject> _levelResources;
        private static Dictionary<string, Texture2D> _cachedTextures;

        public static string texturePath { get { return String.Format("{0}\\textures", RESOURCE_PATH); } }
        public static string levelPath { get { return String.Format("{0}\\levels", RESOURCE_PATH); } }
        public static string materialPath { get { return String.Format("{0}\\materials.xml", RESOURCE_PATH); } }
        public static string itemPath { get { return String.Format("{0}\\items.xml", RESOURCE_PATH); } }
        public static string characterPath { get { return String.Format("{0}\\materials.xml", RESOURCE_PATH); } }
        public static string dialoguePath { get { return String.Format("{0}\\materials.xml", RESOURCE_PATH); } }

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

        // Checks to see if a resource exists
        public static bool exists(string uid)
        {
            try
            {
                // Check to see if the resource is loaded already
                if (isResourceLoaded(uid))
                    return true;

                // Check materials
                using (FileStream fs = new FileStream(materialPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement materialData in data.Elements("Material"))
                    {
                        if (materialData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check items
                using (FileStream fs = new FileStream(itemPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement itemData in data.Elements("Item"))
                    {
                        if (itemData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check characters
                using (FileStream fs = new FileStream(characterPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement characterData in data.Elements("Character"))
                    {
                        if (characterData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check dialogue
                using (FileStream fs = new FileStream(dialoguePath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement dialogueData in data.Elements("Dialogue"))
                    {
                        if (dialogueData.Attribute("uid").Value == uid)
                            return true;
                    }
                }
            }
            catch (XmlException e)
            {
                throw new InvalidResourceException();
            }
            catch (FileNotFoundException e)
            {
                throw new ResourceNotFoundException(uid);
            }

            return false;
        }

        // Destroy a resource
        public static void destroy(string uid)
        {
            try
            {
                // XML helper method
                Func<string, string, string, bool> updateXml = (string filePath, string parentElement, string element) =>
                    {
                        XDocument doc = XDocument.Load(filePath);
                        foreach (XElement data in doc.Element(parentElement).Elements(element))
                        {
                            if (data.Attribute("uid").Value == uid)
                            {
                                data.Remove();
                                doc.Save(filePath);
                                return true;
                            }
                        }
                        return false;
                    };

                // Materials
                if (updateXml(materialPath, "Materials", "Material"))
                    return;

                // Items
                if (updateXml(itemPath, "Items", "Item"))
                    return;

                // Characters
                if (updateXml(characterPath, "Characters", "Character"))
                    return;

                // Dialogue
                if (updateXml(dialoguePath, "Dialogues", "Dialogue"))
                    return;
            }
            catch (XmlException e)
            {
                throw new InvalidResourceException();
            }
        }

        // Get texture -- Try to get the texture from the cache before loading it from file
        public static Texture2D getTexture(string textureUID)
        {
            Texture2D texture = null;
            if (!_cachedTextures.TryGetValue(textureUID, out texture))
            {
                // Find file
                string[] textureFiles = Directory.GetFiles(texturePath, String.Format("{0}.*", textureUID));
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
        public static List<ResourceObject> loadMaterials()
        {
            _materialResources.Clear();

            List<ResourceObject> resourcesLoaded = new List<ResourceObject>();
            using (FileStream fs = new FileStream(materialPath, FileMode.Open))
            {
                XElement data = XElement.Load(fs);

                foreach (XElement materialData in data.Elements("Material"))
                {
                    ResourceObject resource = new ResourceObject(materialData);
                    _materialResources[resource.uid] = resource;
                    resourcesLoaded.Add(resource);
                }
            }

            return resourcesLoaded;
        }

        // Load items
        public static void loadItems()
        {
            _itemResources.Clear();

            using (FileStream fs = new FileStream(itemPath, FileMode.Open))
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

            using (FileStream fs = new FileStream(characterPath, FileMode.Open))
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

            using (FileStream fs = new FileStream(dialoguePath, FileMode.Open))
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
