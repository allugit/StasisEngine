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
        private static GraphicsDevice _graphicsDevice;
        private static Dictionary<string, XElement> _materialResources;
        private static Dictionary<string, XElement> _itemResources;
        private static Dictionary<string, XElement> _blueprintResources;
        private static Dictionary<string, XElement> _blueprintScrapResources;
        private static Dictionary<string, XElement> _characterResources;
        private static Dictionary<string, XElement> _dialogueResources;
        private static Dictionary<string, XElement> _levelResources;
        private static Dictionary<string, XElement> _circuitResources;
        private static Dictionary<string, XElement> _backgroundResources;
        private static Dictionary<string, XElement> _worldMapResources;
        private static Dictionary<string, XElement> _ropeMaterialResources;
        private static Dictionary<string, Texture2D> _cachedTextures;
        private static List<Dictionary<string, XElement>> _resources;
        public static string rootDirectory = "";

        public static string texturePath { get { return rootDirectory + @"data/textures"; } }
        public static string levelPath { get { return rootDirectory + @"data/levels"; } }
        public static string materialPath { get { return rootDirectory + @"data/materials.xml"; } }
        public static string itemPath { get { return rootDirectory + @"data/items.xml"; } }
        public static string blueprintPath { get { return rootDirectory + @"data/blueprints.xml"; } }
        public static string characterPath { get { return rootDirectory + @"data/characters.xml"; } }
        public static string dialoguePath { get { return rootDirectory + @"data/dialogues.xml"; } }
        public static string circuitPath { get { return rootDirectory + @"data/circuits.xml"; } }
        public static string backgroundPath { get { return rootDirectory + @"data/backgrounds.xml"; } }
        public static string worldMapPath { get { return rootDirectory + @"data/world_maps.xml"; } }
        public static string ropeMaterialPath { get { return rootDirectory + @"data/rope_materials.xml"; } }
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
        public static List<XElement> blueprintScrapResources
        {
            get
            {
                List<XElement> resources = new List<XElement>();
                foreach (XElement resource in _blueprintScrapResources.Values)
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
        public static List<XElement> worldMapResources
        {
            get
            {
                List<XElement> resources = new List<XElement>();
                foreach (XElement resource in _worldMapResources.Values)
                    resources.Add(resource);
                return resources;
            }
        }
        public static List<XElement> ropeMaterialResources
        {
            get
            {
                List<XElement> resources = new List<XElement>();
                foreach (XElement resource in _ropeMaterialResources.Values)
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
            _blueprintScrapResources = new Dictionary<string, XElement>();
            _characterResources = new Dictionary<string, XElement>();
            _dialogueResources = new Dictionary<string, XElement>();
            _levelResources = new Dictionary<string, XElement>();
            _circuitResources = new Dictionary<string, XElement>();
            _backgroundResources = new Dictionary<string, XElement>();
            _worldMapResources = new Dictionary<string, XElement>();
            _ropeMaterialResources = new Dictionary<string, XElement>();
            _cachedTextures = new Dictionary<string, Texture2D>();

            // Store all resource dictionaries in a list
            _resources = new List<Dictionary<string, XElement>>();
            _resources.Add(_materialResources);
            _resources.Add(_itemResources);
            _resources.Add(_blueprintResources);
            _resources.Add(_blueprintScrapResources);
            _resources.Add(_characterResources);
            _resources.Add(_dialogueResources);
            _resources.Add(_levelResources);
            _resources.Add(_circuitResources);
            _resources.Add(_backgroundResources);
            _resources.Add(_worldMapResources);
            _resources.Add(_ropeMaterialResources);
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

        // Get texture -- Try to get the texture from the cache before loading it from file
        public static Texture2D getTexture(string textureUID, bool cacheTextures = true)
        {
            Texture2D texture = null;
            if (!_cachedTextures.TryGetValue(textureUID, out texture))
            {
                // Find file
                string[] textureFiles = Directory.GetFiles(texturePath, String.Format("{0}.*", textureUID));
                if (textureFiles.Length == 0)
                    throw new ResourceNotFoundException(textureUID);

                // Load texture
#if XBOX
                using (Stream stream = TitleContainer.OpenStream(textureFiles[0]))
                    texture = Texture2D.FromStream(_graphicsDevice, stream);
#else
                using (FileStream fs = new FileStream(textureFiles[0], FileMode.Open))
                    texture = Texture2D.FromStream(_graphicsDevice, fs);
#endif
                
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
                if (cacheTextures)
                    _cachedTextures[textureUID] = texture;
                
            }
            return texture;
        }

        // clearCache
        public static void clearCache()
        {
            _cachedTextures.Clear();
        }

        // Load materials
        public static void loadAllMaterials(Stream stream)
        {
            _materialResources.Clear();

            XElement data = XElement.Load(stream);

            foreach (XElement materialData in data.Elements("Material"))
            {
                _materialResources[materialData.Attribute("uid").Value] = materialData;
            }

            stream.Close();
        }

        // Load items
        public static void loadAllItems(Stream stream)
        {
            _itemResources.Clear();

            XElement data = XElement.Load(stream);

            foreach (XElement itemData in data.Elements("Item"))
            {
                _itemResources[itemData.Attribute("uid").Value] = itemData;
            }

            stream.Close();
        }

        // Load blueprints
        public static void loadAllBlueprints(Stream stream)
        {
            _blueprintResources.Clear();
            _blueprintScrapResources.Clear();

            XElement data = XElement.Load(stream);

            foreach (XElement blueprintData in data.Elements("Blueprint"))
            {
                _blueprintResources[blueprintData.Attribute("uid").Value] = blueprintData;
                foreach (XElement blueprintScrapData in blueprintData.Elements("BlueprintScrap"))
                {
                    _blueprintScrapResources[blueprintScrapData.Attribute("uid").Value] = blueprintScrapData;
                }
            }

            stream.Close();
        }

        // Load characters
        public static void loadAllCharacters(Stream stream)
        {
            _characterResources.Clear();

            XElement data = XElement.Load(stream);
            foreach (XElement characterData in data.Elements("Character"))
            {
                _characterResources[characterData.Attribute("uid").Value] = characterData;
            }

            stream.Close();
        }

        // Load dialogue
        public static void loadAllDialogue(Stream stream)
        {
            _dialogueResources.Clear();

            XElement data = XElement.Load(stream);

            foreach (XElement dialogueData in data.Elements("Dialogue"))
            {
                _dialogueResources[dialogueData.Attribute("uid").Value] = dialogueData;
            }

            stream.Close();
        }

        // Load circuits
        public static void loadAllCircuits(Stream stream)
        {
            _circuitResources.Clear();

            XElement data = XElement.Load(stream);
            foreach (XElement circuitData in data.Elements("Circuit"))
            {
                _circuitResources[circuitData.Attribute("uid").Value] = circuitData;
            }

            stream.Close();
        }

        // Load backgrounds
        public static void loadAllBackgrounds(Stream stream)
        {
            _backgroundResources.Clear();

            XElement data = XElement.Load(stream);
            foreach (XElement backgroundData in data.Elements("Background"))
            {
                _backgroundResources[backgroundData.Attribute("uid").Value] = backgroundData;
            }

            stream.Close();
        }

        // Load world maps
        public static void loadAllWorldMaps(Stream stream)
        {
            _worldMapResources.Clear();

            XElement data = XElement.Load(stream);
            foreach (XElement worldMapData in data.Elements("WorldMap"))
            {
                _worldMapResources[worldMapData.Attribute("uid").Value] = worldMapData;
            }

            stream.Close();
        }

        // Load all rope materials
        public static void loadAllRopeMaterials(Stream stream)
        {
            _ropeMaterialResources.Clear();

            XElement data = XElement.Load(stream);
            foreach (XElement ropeMaterialData in data.Elements("RopeMaterial"))
            {
                _ropeMaterialResources[ropeMaterialData.Attribute("uid").Value] = ropeMaterialData;
            }
        }
    }
}
