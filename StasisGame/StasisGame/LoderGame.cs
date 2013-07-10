using System.Diagnostics;
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
using StasisCore.Models;
using EasyStorage;

namespace StasisGame
{
    public enum GameState
    {
        Initializing,
        Intro,
        MainMenu,
        Options,
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
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private ScriptManager _scriptManager;
        private PlayerSystem _playerSystem;
        private EquipmentSystem _equipmentSystem;
        private WorldMapSystem _worldMapSystem;
        private LevelSystem _levelSystem;
        private ScreenSystem _screenSystem;
        private MainMenuScreen _mainMenuScreen;
        private WorldMapScreen _worldMapScreen;
        private BackgroundRenderer _menuBackgroundRenderer;
        private Vector2 _menuBackgroundScreenOffset;

        public static bool debug;
        public SpriteBatch spriteBatch { get { return _spriteBatch; } }
        public GraphicsDeviceManager graphics { get { return _graphics; } }
        public SystemManager systemManager { get { return _systemManager; } }
        public EntityManager entityManager { get { return _entityManager; } }
        public ScriptManager scriptManager { get { return _scriptManager; } }
        public BackgroundRenderer menuBackgroundRenderer { get { return _menuBackgroundRenderer; } }
        public Vector2 menuBackgroundScreenOffset { get { return _menuBackgroundScreenOffset; } }

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
            _scriptManager = new ScriptManager(_systemManager, _entityManager);
            _screenSystem = new ScreenSystem(_systemManager);
            _systemManager.add(_screenSystem, -1);

            DataManager.initialize(this, _systemManager);
            Components.Add(new GamerServicesComponent(this));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new[] { Color.White });

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
            ResourceManager.loadAllRopeMaterials(TitleContainer.OpenStream(ResourceManager.ropeMaterialPath));

            // Load user interface textures
            ResourceManager.setTexture("pane_top_left_corner", Content.Load<Texture2D>("shared_ui/pane_top_left_corner"));
            ResourceManager.setTexture("pane_top_right_corner", Content.Load<Texture2D>("shared_ui/pane_top_right_corner"));
            ResourceManager.setTexture("pane_bottom_right_corner", Content.Load<Texture2D>("shared_ui/pane_bottom_right_corner"));
            ResourceManager.setTexture("pane_bottom_left_corner", Content.Load<Texture2D>("shared_ui/pane_bottom_left_corner"));
            ResourceManager.setTexture("pane_left_side", Content.Load<Texture2D>("shared_ui/pane_left_side"));
            ResourceManager.setTexture("pane_top_side", Content.Load<Texture2D>("shared_ui/pane_top_side"));
            ResourceManager.setTexture("pane_right_side", Content.Load<Texture2D>("shared_ui/pane_right_side"));
            ResourceManager.setTexture("pane_bottom_side", Content.Load<Texture2D>("shared_ui/pane_bottom_side"));
            ResourceManager.setTexture("pane_background", Content.Load<Texture2D>("shared_ui/pane_background"));
            ResourceManager.setTexture("line_indicator", Content.Load<Texture2D>("shared_ui/line_indicator"));
            ResourceManager.setTexture("pixel", pixel);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _arial = Content.Load<SpriteFont>("arial");

            _menuBackgroundRenderer = new BackgroundRenderer(_spriteBatch);
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

        // initializePersistentSystems -- Certain systems (such as WorldMap, Level, Player, etc...) need to be active throughout the entire game
        private void startPersistentSystems()
        {
            _playerSystem = new PlayerSystem(_systemManager, _entityManager);
            _equipmentSystem = new EquipmentSystem(_systemManager, _entityManager);
            _worldMapSystem = new WorldMapSystem(_systemManager, _entityManager);
            _levelSystem = new LevelSystem(this, _systemManager, _entityManager);
            _systemManager.add(_playerSystem, -1);
            _systemManager.add(_equipmentSystem, -1);
            _systemManager.add(_worldMapSystem, -1);
            _systemManager.add(_levelSystem, -1);
        }

        private void previewLevel(string levelUID)
        {
            startPersistentSystems();

            DataManager.createTemporaryPlayerData(_systemManager);
            _playerSystem.createPlayer();
            _playerSystem.initializeInventory();

            // Load level
            loadLevel(levelUID);
        }

        public void loadGame(int playerDataSlot)
        {
            startPersistentSystems();

            DataManager.loadPlayerData(playerDataSlot);
            _playerSystem.createPlayer();
            _playerSystem.initializeInventory();

            _screenSystem.removeScreen(_mainMenuScreen);
            openWorldMap();
        }

        public void loadLevel(string levelUID)
        {
            _gameState = GameState.Level;
            _scriptManager.loadLevelScript(levelUID);
            _levelSystem.load(levelUID);
            _playerSystem.addLevelComponents();
            _screenSystem.addScreen(new LevelScreen(this, _systemManager, _entityManager));
        }

        public void closeMainMenu()
        {
            _screenSystem.removeScreen(_mainMenuScreen);
        }

        public void openMainMenu()
        {
            _screenSystem.addScreen(_mainMenuScreen);
        }

        public void openLoadGameMenu()
        {
            _screenSystem.addScreen(new LoadGameScreen(this));
        }

        public void closeLoadGameMenu()
        {
            _screenSystem.removeScreen(ScreenType.LoadGameMenu);
        }

        public void openOptionsMenu()
        {
            _screenSystem.addScreen(new OptionsMenuScreen(this));
        }

        public void closeOptionsMenu()
        {
            _screenSystem.removeScreen(ScreenType.OptionsMenu);
        }

        public void openPlayerCreationScreen()
        {
            _screenSystem.addScreen(new PlayerCreationScreen(this));
        }

        public void closePlayerCreationScreen()
        {
            _screenSystem.removeScreen(ScreenType.PlayerCreation);
        }

        public void openWorldMap()
        {
            WorldMapSystem worldMapSystem = (WorldMapSystem)_systemManager.getSystem(SystemType.WorldMap);
            string currentWorldMapUID = DataManager.playerData.currentLocation.worldMapUID;

            worldMapSystem.loadWorldMap(currentWorldMapUID, DataManager.playerData.getWorldMapData(currentWorldMapUID));
            _worldMapScreen = _worldMapScreen ?? new WorldMapScreen(this, _systemManager);
            _screenSystem.addScreen(_worldMapScreen);

            _gameState = GameState.WorldMap;
        }

        public void closeWorldMap()
        {
            _screenSystem.removeScreen(_worldMapScreen);
            _gameState = GameState.Level;
        }

        protected override void Update(GameTime gameTime)
        {
            _menuBackgroundScreenOffset += new Vector2(0.005f, 0f);

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
                    // TODO: Do some sort of intro, and then open the main menu
                    Background background = new Background(ResourceManager.getResource("main_menu_background"));

                    _gameState = GameState.MainMenu;
                    _mainMenuScreen = new MainMenuScreen(this);
                    background.loadTextures();
                    _menuBackgroundRenderer.background = background;
                    _screenSystem.addScreen(_mainMenuScreen);
                    break;

                case GameState.MainMenu:
                    _systemManager.process();
                    break;

                case GameState.WorldMap:
                    _systemManager.process();
                    break;

                case GameState.Level:
                    _systemManager.process();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (_gameState)
            {
                case GameState.MainMenu:
                    _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
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
                    _levelSystem.draw();
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
