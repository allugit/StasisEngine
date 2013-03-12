using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using StasisCore;
using EasyStorage;

namespace StasisGame
{
    public enum GameState
    {
        Initializing,
        Intro,
        MainMenu,
        Options,
        CreatePlayer,
        WorldMap,
        Level
    };

    public class LoderGame : Game
    {
        private string[] _args;
        private string _argsDebug = "args:";
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _arial;
        private GameState _gameState;
        private Level _level;
        private MainMenu _mainMenu;
        private bool _settingsInitialized;
        public static IAsyncSaveDevice saveDevice;

        public SpriteBatch spriteBatch { get { return _spriteBatch; } }

        public LoderGame(string[] args)
        {
            _args = args;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Loder's Fall";
            _gameState = GameState.Initializing;
        }

        protected override void Initialize()
        {
            EasyStorageSettings.SetSupportedLanguages(Language.English);
            SharedSaveDevice sharedSaveDevice = new SharedSaveDevice();

            saveDevice = sharedSaveDevice;

            sharedSaveDevice.DeviceSelectorCanceled += (s, e) =>
            {
                e.Response = SaveDeviceEventResponse.Force;
            };
            sharedSaveDevice.DeviceDisconnected += (s, e) =>
            {
                e.Response = SaveDeviceEventResponse.Force;
            };
            sharedSaveDevice.PromptForDevice();

            Components.Add(sharedSaveDevice);
            Components.Add(new GamerServicesComponent(this));

            saveDevice.SaveCompleted += new SaveCompletedEventHandler(saveDevice_SaveCompleted);

            base.Initialize();

            //handleArgs();
        }

        void saveDevice_SaveCompleted(object sender, FileActionCompletedEventArgs args)
        {
            Console.WriteLine("save completed.");
        }

        protected override void LoadContent()
        {
            ResourceManager.initialize(GraphicsDevice);
            ResourceManager.loadAllCharacters(TitleContainer.OpenStream(ResourceManager.characterPath));
            ResourceManager.loadAllCircuits(TitleContainer.OpenStream(ResourceManager.circuitPath));
            ResourceManager.loadAllDialogue(TitleContainer.OpenStream(ResourceManager.dialoguePath));
            ResourceManager.loadAllItems(TitleContainer.OpenStream(ResourceManager.itemPath));
            ResourceManager.loadAllBlueprints(TitleContainer.OpenStream(ResourceManager.blueprintPath));
            ResourceManager.loadAllMaterials(TitleContainer.OpenStream(ResourceManager.materialPath));
            ResourceManager.loadAllBackgrounds(TitleContainer.OpenStream(ResourceManager.backgroundPath));

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _arial = Content.Load<SpriteFont>("arial");
        }

        protected override void UnloadContent()
        {
        }

        private void handleArgs()
        {
            foreach (string arg in _args)
                _argsDebug += arg;

            if (_args.Length > 0)
            {
                string flag = _args[0];
                switch (flag)
                {
                    case "-l":
                        quickLoadLevel(_args[1]);
                        break;
                }
            }
        }

        private void quickLoadLevel(string filePath)
        {
            // Load player data
            // ...

            // Character flags
            // ...

            // Load level
            _gameState = GameState.Level;
            _level = new Level(this, filePath);
        }

        protected override void Update(GameTime gameTime)
        {
            switch (_gameState)
            {
                case GameState.Initializing:
                    if (!_settingsInitialized && saveDevice.IsReady)
                    {
                        if (!saveDevice.FileExists("LodersFall_Save", "settings.xml"))
                        {
                            saveDevice.Save("LodersFall_Save", "settings.xml", (stream) =>
                            {
                                XDocument doc = new XDocument(new XElement("Settings"));
                                doc.Save(stream);
                            });
                        }

                        saveDevice.Load("LodersFall_Save", "settings.xml", (stream) =>
                        {
                            DisplayMode largestDisplayMode = null;
                            foreach (DisplayMode displayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                            {
                                if (largestDisplayMode == null)
                                {
                                    largestDisplayMode = displayMode;
                                    continue;
                                }

                                largestDisplayMode = displayMode.Width * displayMode.Height > largestDisplayMode.Width * largestDisplayMode.Height ? displayMode : largestDisplayMode;
                            }

                            XDocument doc = XDocument.Load(stream);
                            XElement data = doc.Element("Settings");
                            GameSettings.screenWidth = Loader.loadInt(data.Element("ScreenWidth"), largestDisplayMode.Width);
                            GameSettings.screenHeight = Loader.loadInt(data.Element("ScreenHeight"), largestDisplayMode.Height);
                            System.Diagnostics.Debug.WriteLine(GameSettings.screenWidth);
                            System.Diagnostics.Debug.WriteLine(GameSettings.screenHeight);
                        });

                        _graphics.PreferMultiSampling = true;
                        _graphics.PreferredBackBufferWidth = GameSettings.screenWidth;
                        _graphics.PreferredBackBufferHeight = GameSettings.screenHeight;
                        _graphics.ApplyChanges();

                        _settingsInitialized = true;

                        handleArgs();
                    }
                    break;

                case GameState.Intro:
                    _gameState = GameState.MainMenu;
                    _mainMenu = new MainMenu(this);
                    break;

                case GameState.MainMenu:
                    _mainMenu.update(gameTime);
                    break;

                case GameState.Level:
                    _level.update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //_spriteBatch.Begin();
            //_spriteBatch.DrawString(_arial, _argsDebug, new Vector2(16, 16), Color.White);

            switch (_gameState)
            {
                case GameState.MainMenu:
                    _spriteBatch.Begin();
                    _mainMenu.draw(gameTime);
                    _spriteBatch.End();
                    break;

                case GameState.Level:
                    _level.draw(gameTime);
                    break;
            }

            //_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
