using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisCore
{
    /*
     * Resource controller
     *   Loads resources from file and populates resource objects with the data. Resource objects are
     *   stored in a dictionary, using their UIDs as keys.
     */
    public class ResourceManager
    {
        public const string RESOURCE_PATH = @"D:\StasisResources";

        private static GraphicsDevice _graphicsDevice;
        private static Dictionary<string, XElement> _materialResources;
        private static Dictionary<string, XElement> _itemResources;
        private static Dictionary<string, XElement> _blueprintResources;
        private static Dictionary<string, XElement> _characterResources;
        private static Dictionary<string, XElement> _dialogueResources;
        private static Dictionary<string, XElement> _levelResources;
        private static Dictionary<string, XElement> _circuitResources;
        private static Dictionary<string, XElement> _backgroundResources;
        private static Dictionary<string, Texture2D> _cachedTextures;
        private static List<Dictionary<string, XElement>> _resources;

        public static string texturePath { get { return String.Format("{0}\\textures", RESOURCE_PATH); } }
        public static string levelPath { get { return String.Format("{0}\\levels", RESOURCE_PATH); } }
        public static string materialPath { get { return String.Format("{0}\\materials.xml", RESOURCE_PATH); } }
        public static string itemPath { get { return String.Format("{0}\\items.xml", RESOURCE_PATH); } }
        public static string blueprintPath { get { return String.Format("{0}\\blueprints.xml", RESOURCE_PATH); } }
        public static string characterPath { get { return String.Format("{0}\\characters.xml", RESOURCE_PATH); } }
        public static string dialoguePath { get { return String.Format("{0}\\dialogues.xml", RESOURCE_PATH); } }
        public static string circuitPath { get { return String.Format("{0}\\circuits.xml", RESOURCE_PATH); } }
        public static string backgroundPath { get { return String.Format("{0}\\backgrounds.xml", RESOURCE_PATH); } }
        public static GraphicsDevice graphicsDevice { get { return _graphicsDevice; } }

        public static List<XElement> materialResources
        {
            get
            {
                List<XElement> resources = new List<XElement>();
                foreach (XElement resource in _materialResources.Values)
                    resources.Add(resource);
                return resources;
            }
        }
        public static List<XElement> itemResources
        {
            get
            {
                List<XElement> resources = new List<XElement>();
                foreach (XElement resource in _itemResources.Values)
                    resources.Add(resource);
                return resources;
            }
        }
        public static List<XElement> blueprintResources
        {
            get
            {
                List<XElement> resources = new List<XElement>();
                foreach (XElement resource in _blueprintResources.Values)
                    resources.Add(resource);
                return resources;
            }
        }
        public static List<XElement> characterResources
        {
            get
            {
                List<XElement> resources = new List<XElement>();
                foreach (XElement resource in _characterResources.Values)
                    resources.Add(resource);
                return resources;
            }
        }
        public static List<XElement> dialogueResources
        {
            get
            {
                List<XElement> resources = new List<XElement>();
                foreach (XElement resource in _dialogueResources.Values)
                    resources.Add(resource);
                return resources;
            }
        }
        public static List<XElement> levelResources
        {
            get
            {
                List<XElement> resources = new List<XElement>();
                foreach (XElement resource in _levelResources.Values)
                    resources.Add(resource);
                return resources;
            }
        }
        public static List<XElement> circuitResources
        {
            get
            {
                List<XElement> resources = new List<XElement>();
                foreach (XElement resource in _circuitResources.Values)
                    resources.Add(resource);
                return resources;
            }
        }
        public static List<XElement> backgroundResources
        {
            get
            {
                List<XElement> resources = new List<XElement>();
                foreach (XElement resource in _backgroundResources.Values)
                    resources.Add(resource);
                return resources;
            }
        }

        // Initialize -- Called once when the application starts
        public static void initialize(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _materialResources = new Dictionary<string, XElement>();
            _itemResources = new Dictionary<string, XElement>();
            _blueprintResources = new Dictionary<string, XElement>();
            _characterResources = new Dictionary<string, XElement>();
            _dialogueResources = new Dictionary<string, XElement>();
            _levelResources = new Dictionary<string, XElement>();
            _circuitResources = new Dictionary<string, XElement>();
            _backgroundResources = new Dictionary<string, XElement>();
            _cachedTextures = new Dictionary<string, Texture2D>();

            // Store all resource dictionaries in a list
            _resources = new List<Dictionary<string, XElement>>();
            _resources.Add(_materialResources);
            _resources.Add(_itemResources);
            _resources.Add(_blueprintResources);
            _resources.Add(_characterResources);
            _resources.Add(_dialogueResources);
            _resources.Add(_levelResources);
            _resources.Add(_circuitResources);
            _resources.Add(_backgroundResources);
        }

        // Checks to see if a resource has been loaded
        public static bool isResourceLoaded(string uid)
        {
            foreach (Dictionary<string, XElement> dictionary in _resources)
            {
                if (dictionary.ContainsKey(uid))
                    return true;
            }

            return false;
        }

        // Returns a resource that matches a uid
        public static XElement getResource(string uid)
        {
            foreach (Dictionary<string, XElement> dictionary in _resources)
            {
                if (dictionary.ContainsKey(uid))
                    return dictionary[uid];
            }

            return null;
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

                // Check blueprints
                using (FileStream fs = new FileStream(blueprintPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement blueprintData in data.Elements("Item"))
                    {
                        if (blueprintData.Attribute("uid").Value == uid)
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

                // Check circuits
                using (FileStream fs = new FileStream(dialoguePath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement circuitData in data.Elements("Circuit"))
                    {
                        if (circuitData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check backgrounds
                using (FileStream fs = new FileStream(backgroundPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement backgroundData in data.Elements("Background"))
                    {
                        if (backgroundData.Attribute("uid").Value == uid)
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

        // Save material resources
        public static void saveMaterialResources(List<Material> materials, bool backup)
        {
            // Backup materials
            if (backup)
            {
                string backupFile = materialPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(materialPath, backupFile);
            }

            // Save materials
            XDocument doc = new XDocument(new XElement("Materials"));
            foreach (Material material in materials)
                doc.Element("Materials").Add(material.data);
            doc.Save(materialPath);

            // Reload materials
            loadAllMaterials();
        }

        // Save blueprint resources
        public static void saveBlueprintResources(List<Blueprint> blueprints, bool backup)
        {
            // Backup blueprints
            if (backup)
            {
                string backupFile = blueprintPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(blueprintPath, backupFile);
            }

            // Save blueprints
            XDocument doc = new XDocument(new XElement("Items"));
            foreach (Blueprint blueprint in blueprints)
                doc.Element("Items").Add(blueprint.data);
            doc.Save(blueprintPath);

            // Reload blueprints
            loadAllItems();
        }

        // Save circuit resources
        public static void saveCircuitResources(List<Circuit> circuits, bool backup)
        {
            // Backup circuits
            if (backup)
            {
                string backupFile = circuitPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(circuitPath, backupFile);
            }

            // Save circuits
            XDocument doc = new XDocument(new XElement("Circuits"));
            foreach (Circuit circuit in circuits)
                doc.Element("Circuits").Add(circuit.data);
            doc.Save(circuitPath);

            // Reload circuits
            loadAllCircuits();
        }

        // Save background resources
        public static void saveBackgroundResources(XElement data, bool backup)
        {
            // Backup backgrounds
            if (backup)
            {
                string backupFile = backgroundPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(backgroundPath, backupFile);
            }

            // Save backgrounds
            XDocument doc = new XDocument(data);
            doc.Save(backgroundPath);

            // Reload background
            loadAllBackgrounds();
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
                        foreach (XElement data in doc.Element(parentElement).Descendants(element))
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

                // Backgrounds
                if (updateXml(dialoguePath, "Backgrounds", "Background"))
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

                // Premultiply alpha
                RenderTarget2D result = new RenderTarget2D(_graphicsDevice, texture.Width, texture.Height);
                _graphicsDevice.SetRenderTarget(result);
                _graphicsDevice.Clear(Color.Black);

                // Multiply each color by the source alpha, and write in just the color values into the final texture
                BlendState blendColor = new BlendState();
                blendColor.ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue;

                blendColor.AlphaDestinationBlend = Blend.Zero;
                blendColor.ColorDestinationBlend = Blend.Zero;

                blendColor.AlphaSourceBlend = Blend.SourceAlpha;
                blendColor.ColorSourceBlend = Blend.SourceAlpha;

                SpriteBatch spriteBatch = new SpriteBatch(_graphicsDevice);
                spriteBatch.Begin(SpriteSortMode.Immediate, blendColor);
                spriteBatch.Draw(texture, texture.Bounds, Color.White);
                spriteBatch.End();

                // Now copy over the alpha values from the PNG source texture to the final one, without multiplying them
                BlendState blendAlpha = new BlendState();
                blendAlpha.ColorWriteChannels = ColorWriteChannels.Alpha;

                blendAlpha.AlphaDestinationBlend = Blend.Zero;
                blendAlpha.ColorDestinationBlend = Blend.Zero;

                blendAlpha.AlphaSourceBlend = Blend.One;
                blendAlpha.ColorSourceBlend = Blend.One;

                spriteBatch.Begin(SpriteSortMode.Immediate, blendAlpha);
                spriteBatch.Draw(texture, texture.Bounds, Color.White);
                spriteBatch.End();

                //Save texture and release the GPU back to drawing to the screen
                _graphicsDevice.SetRenderTarget(null);
                _graphicsDevice.Textures[0] = null;
                Color[] data = new Color[result.Width * result.Height];
                result.GetData<Color>(data);
                texture.SetData<Color>(data);
                result.Dispose();

                // Add it to cache
                _cachedTextures[textureUID] = texture;
            }
            return texture;
        }

        // Load materials
        public static void loadAllMaterials()
        {
            _materialResources.Clear();

            using (FileStream fs = new FileStream(materialPath, FileMode.Open))
            {
                XElement data = XElement.Load(fs);

                foreach (XElement materialData in data.Elements("Material"))
                {
                    _materialResources[materialData.Attribute("uid").Value] = materialData;
                }
            }
        }

        // Load items
        public static void loadAllItems()
        {
            _itemResources.Clear();

            using (FileStream fs = new FileStream(itemPath, FileMode.Open))
            {
                XElement data = XElement.Load(fs);

                foreach (XElement itemData in data.Elements("Item"))
                {
                    _itemResources[itemData.Attribute("uid").Value] = itemData;
                }
            }
        }

        // Load blueprints
        public static void loadAllBlueprints()
        {
            _blueprintResources.Clear();

            using (FileStream fs = new FileStream(blueprintPath, FileMode.Open))
            {
                XElement data = XElement.Load(fs);

                foreach (XElement blueprintData in data.Elements("Item"))
                {
                    _blueprintResources[blueprintData.Attribute("uid").Value] = blueprintData;
                }
            }
        }

        // Load characters
        public static void loadAllCharacters()
        {
            _characterResources.Clear();

            using (FileStream fs = new FileStream(characterPath, FileMode.Open))
            {
                XElement data = XElement.Load(fs);
                foreach (XElement characterData in data.Elements("Character"))
                {
                    _characterResources[characterData.Attribute("uid").Value] = characterData;
                }
            }
        }

        // Load dialogue
        public static void loadAllDialogue()
        {
            _dialogueResources.Clear();

            using (FileStream fs = new FileStream(dialoguePath, FileMode.Open))
            {
                XElement data = XElement.Load(fs);
                foreach (XElement dialogueData in data.Elements("Dialogue"))
                {
                    _dialogueResources[dialogueData.Attribute("uid").Value] = dialogueData;
                }
            }
        }

        // Load circuits
        public static void loadAllCircuits()
        {
            _circuitResources.Clear();

            using (FileStream fs = new FileStream(circuitPath, FileMode.Open))
            {
                XElement data = XElement.Load(fs);
                foreach (XElement circuitData in data.Elements("Circuit"))
                {
                    _circuitResources[circuitData.Attribute("uid").Value] = circuitData;
                }
            }
        }

        // Load backgrounds
        public static void loadAllBackgrounds()
        {
            _backgroundResources.Clear();

            using (FileStream fs = new FileStream(backgroundPath, FileMode.Open))
            {
                XElement data = XElement.Load(fs);
                foreach (XElement backgroundData in data.Elements("Background"))
                {
                    _backgroundResources[backgroundData.Attribute("uid").Value] = backgroundData;
                }
            }
        }
    }
}
