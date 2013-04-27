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
using StasisGame.Managers;
using StasisGame.UI;
using StasisGame.Systems;
using StasisGame.Components;
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

    public enum ControllerType
    {
        KeyboardAndMouse,
        Gamepad
    };

    public class LoderGame : Game
    {
        private string[] _args;
        private string _argsDebug = "args:";
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _arial;
        private GameState _gameState;
        private MainMenuScreen _mainMenuScreen;
        private ScreenSystem _screenSystem;
        private Level _level;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private WorldMapScreen _worldMapScreen;
        private PlayerSystem _playerSystem;

        public SpriteBatch spriteBatch { get { return _spriteBatch; } }
        public GraphicsDeviceManager graphics { get { return _graphics; } }
        public SystemManager systemManager { get { return _systemManager; } }
        public EntityManager entityManager { get { return _entityManager; } }

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
            _systemManager = new SystemManager();
            _entityManager = new EntityManager(_systemManager);
            _screenSystem = new ScreenSystem(_systemManager);
            _systemManager.add(_screenSystem, -1);


            DataManager.initialize(this, _systemManager);
            Components.Add(new GamerServicesComponent(this));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: Be more selective about which resources to load...
            ResourceManager.initialize(GraphicsDevice);
            ResourceManager.loadAllCharacters(TitleContainer.OpenStream(ResourceManager.characterPath));
            ResourceManager.loadAllCircuits(TitleContainer.OpenStream(ResourceManager.circuitPath));
            ResourceManager.loadAllDialogue(TitleContainer.OpenStream(ResourceManager.dialoguePath));
            ResourceManager.loadAllItems(TitleContainer.OpenStream(ResourceManager.itemPath));
            ResourceManager.loadAllBlueprints(TitleContainer.OpenStream(ResourceManager.blueprintPath));
            ResourceManager.loadAllMaterials(TitleContainer.OpenStream(ResourceManager.materialPath));
            ResourceManager.loadAllBackgrounds(TitleContainer.OpenStream(ResourceManager.backgroundPath));
            ResourceManager.loadAllWorldMaps(TitleContainer.OpenStream(ResourceManager.worldMapPath));

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
                        previewLevel(_args[1]);
                        break;
                }
            }
        }

        private void initializePlayer()
        {
            _playerSystem = new PlayerSystem(_systemManager, _entityManager);
            _playerSystem.playerId = _entityManager.createEntity();
            _systemManager.add(_playerSystem, -1);
        }

        private void initializePlayerInventory()
        {
            _entityManager.initializePlayerInventory(_playerSystem.playerId, DataManager.playerData.inventoryData);
            _entityManager.initializePlayerToolbar(
                _playerSystem.playerId,
                (InventoryComponent)_entityManager.getComponent(_playerSystem.playerId, ComponentType.Inventory),
                DataManager.playerData.toolbarData);
        }

        private void previewLevel(string levelUID)
        {
            initializePlayer();
            DataManager.createTemporaryPlayerData(_systemManager);
            initializePlayerInventory();

            // Load level
            loadLevel(levelUID);
        }

        public void newGame()
        {
            _screenSystem.removeScreen(_mainMenuScreen);
            // TODO: Destroy main menu screen?
            _gameState = GameState.CreatePlayer;
        }

        public void loadGame(int playerDataSlot)
        {
            initializePlayer();
            DataManager.loadPlayerData(playerDataSlot);
            initializePlayerInventory();

            _screenSystem.removeScreen(_mainMenuScreen);
            openWorldMap();
        }

        public void loadLevel(string levelUID)
        {
            _gameState = GameState.Level;
            _level = new Level(this, levelUID);
            _entityManager.addLevelComponentsToPlayer(_playerSystem);
            _screenSystem.addScreen(new LevelScreen(this, _level));
        }

        public void openLoadGameMenu()
        {
            _screenSystem.removeScreen(_mainMenuScreen);
            _screenSystem.addScreen(new LoadGameScreen(this));
        }

        public void closeLoadGameMenu()
        {
            _screenSystem.removeScreen(ScreenType.LoadGameMenu);
            _screenSystem.addScreen(_mainMenuScreen);
        }

        public void openOptionsMenu()
        {
            _screenSystem.removeScreen(_mainMenuScreen);
            _screenSystem.addScreen(new OptionsMenuScreen(this));
        }

        public void closeOptionsMenu()
        {
            _screenSystem.removeScreen(ScreenType.OptionsMenu);
            _screenSystem.addScreen(_mainMenuScreen);
        }

        public void openWorldMap()
        {
            _worldMapScreen = new WorldMapScreen(this);
            _screenSystem.addScreen(_worldMapScreen);
            _worldMapScreen.loadWorldMap(DataManager.playerData.getWorldData(DataManager.playerData.currentLocation.worldMapUID));
            _gameState = GameState.WorldMap;
        }

        public void closeWorldMap()
        {
            _screenSystem.removeScreen(_worldMapScreen);
            _gameState = GameState.Level;
        }

        protected override void Update(GameTime gameTime)
        {
            switch (_gameState)
            {
                case GameState.Initializing:
                    if (DataManager.ready)
                    {
                        // Load and apply game settings
                        DataManager.loadGameSettings();
                        _graphics.PreferMultiSampling = true;
                        _graphics.PreferredBackBufferWidth = DataManager.gameSettings.screenWidth;
                        _graphics.PreferredBackBufferHeight = DataManager.gameSettings.screenHeight;
                        _graphics.IsFullScreen = DataManager.gameSettings.fullscreen;
                        _graphics.ApplyChanges();

                        _gameState = GameState.Intro;

                        handleArgs();
                    }
                    break;

                case GameState.Intro:
                    _gameState = GameState.MainMenu;
                    _mainMenuScreen = new MainMenuScreen(this);
                    _screenSystem.addScreen(_mainMenuScreen);
                    break;

                case GameState.MainMenu:
                    _systemManager.process();
                    break;

                case GameState.CreatePlayer:
                    string playerName = "Wamboogley"; // TODO: Let user name their player
                    int playerSlot = DataManager.createPlayerData(_systemManager, playerName);
                    loadGame(playerSlot);
                    break;

                case GameState.WorldMap:
                    _systemManager.process();
                    break;

                case GameState.Level:
                    _systemManager.process();
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
                    _screenSystem.draw();
                    _spriteBatch.End();
                    break;

                case GameState.WorldMap:

                    if (_worldMapScreen != null)
                        _worldMapScreen.preProcess();

                    _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    _screenSystem.draw();
                    _spriteBatch.End();
                    break;

                case GameState.Level:
                    _level.draw(gameTime);

                    _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    _screenSystem.draw();
                    _spriteBatch.End();
                    break;
            }

            //_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
