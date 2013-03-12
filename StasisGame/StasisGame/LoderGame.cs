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

namespace StasisGame
{
    public enum GameState
    {
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

        public SpriteBatch spriteBatch { get { return _spriteBatch; } }

        public LoderGame(string[] args)
        {
            _args = args;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Loder's Fall";
            _gameState = GameState.Intro;
        }

        protected override void Initialize()
        {
            loadGameSettings();

            base.Initialize();

            handleArgs();
        }

        private void loadGameSettings()
        {
            string appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string gameConfigDirectory = appDataDirectory + "\\LoderGame";
            string gameConfigFile = gameConfigDirectory + "\\settings.xml";
            XDocument settingsDocument = null;
            XElement gameSettings = null;
            int screenWidth;
            int screenHeight;
            bool fullscreen;

            if (!Directory.Exists(gameConfigDirectory))
                Directory.CreateDirectory(gameConfigDirectory);
            if (File.Exists(gameConfigFile))
            {
                settingsDocument = XDocument.Load(gameConfigFile);
                gameSettings = settingsDocument.Element("Settings");
            }
            else
            {
                settingsDocument = new XDocument(new XElement("Settings"));
                settingsDocument.Save(gameConfigFile);
                gameSettings = settingsDocument.Element("Settings");
            }

            screenWidth = Loader.loadInt(gameSettings.Element("ScreenWidth"), 1280);
            screenHeight = Loader.loadInt(gameSettings.Element("ScreenHeight"), 768);
            fullscreen = Loader.loadBool(gameSettings.Element("Fullscreen"), false);

            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.IsFullScreen = fullscreen;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            ResourceManager.initialize(GraphicsDevice);
            ResourceManager.loadAllCharacters();
            ResourceManager.loadAllCircuits();
            ResourceManager.loadAllDialogue();
            ResourceManager.loadAllItems();
            ResourceManager.loadAllMaterials();

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
                    _mainMenu.draw(gameTime);
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
