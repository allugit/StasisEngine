using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisCore.Models;
using StasisGame.States;
using StasisGame.Systems;

namespace StasisGame.Managers
{
    /* Data manager uses static methods and variables as a convenience, since it's
     * going to be used throughout the program to save and load data as it's needed. */
    public class DataManager
    {
        private static string _rootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/LodersFall/";
        private static string _settingsDirectory = _rootDirectory + "settings/";
        private static string _playersDirectory = _rootDirectory + "players/";
        private static string _settingsPath = _settingsDirectory + "settings.xml";
        private static LoderGame _game;
        private static SystemManager _systemManager;
        private static GameSettings _gameSettings;
        private static WorldMapManager _worldMapManager;
        private static string _playerName;
        private static int _playerSlot;

        public static GameSettings gameSettings { get { return _gameSettings; } }
        public static string playerName { get { return _playerName; } }
        public static int playerSlot { get { return _playerSlot; } }

        // Initialize
        public static void initialize(LoderGame game, SystemManager systemManager)
        {
            _game = game;
            _systemManager = systemManager;

            if (!Directory.Exists(_rootDirectory))
                Directory.CreateDirectory(_rootDirectory);
            if (!Directory.Exists(_settingsDirectory))
                Directory.CreateDirectory(_settingsDirectory);
            if (!Directory.Exists(_playersDirectory))
                Directory.CreateDirectory(_playersDirectory);
        }

        // Load game settings
        public static void loadGameSettings()
        {
            Logger.log("DataManager.loadGameSettings method started.");

            if (File.Exists(_settingsPath))
            {
                using (FileStream fs = new FileStream(_settingsPath, FileMode.Open))
                {
                    XDocument doc = XDocument.Load(fs);
                    XElement data = doc.Element("Settings");
                    _gameSettings = new GameSettings(data);
                }
            }
            else
            {
                // Create and save default settings
                _gameSettings = new GameSettings(_game);

                using (FileStream fs = new FileStream(_settingsPath, FileMode.Create))
                {
                    XDocument doc = new XDocument(_gameSettings.data);
                    doc.Save(fs);
                }
            }

            Logger.log("DataManager.loadGameSettings method finished.");
        }

        // Save game settings
        public static void saveGameSettings()
        {
            Logger.log("DataManager.saveGameSettings method started.");

            using (FileStream fs = new FileStream(_settingsPath, FileMode.Create))
            {
                XDocument doc = new XDocument(_gameSettings.data);
                doc.Save(fs);
            }

            Logger.log("DataManager.saveGameSettings method finished.");
        }

        // Create world map manager
        private static WorldMapManager createWorldMapManager()
        {
            List<WorldMapDefinition> definitions = new List<WorldMapDefinition>();
            List<XElement> allWorldMapData = ResourceManager.worldMapResources;

            foreach (XElement worldMapData in allWorldMapData)
            {
                WorldMapDefinition worldMapDefinition = new WorldMapDefinition(worldMapData.Attribute("uid").Value, worldMapData.Attribute("texture_uid").Value, Loader.loadVector2("position", Vector2.Zero));

                foreach (XElement levelIconData in worldMapData.Elements("LevelIcon"))
                {
                    worldMapDefinition.levelIconDefinitions.Add(
                        new LevelIconDefinition(
                            worldMapDefinition,
                            levelIconData.Attribute("uid").Value,
                            levelIconData.Attribute("level_uid").Value,
                            levelIconData.Attribute("finished_texture_uid").Value,
                            levelIconData.Attribute("unfinished_texture_uid").Value,
                            levelIconData.Attribute("title").Value,
                            levelIconData.Attribute("description").Value,
                            Loader.loadVector2(levelIconData.Attribute("position"), Vector2.Zero)));
                }

                foreach (XElement levelPathData in worldMapData.Elements("LevelPath"))
                {
                    LevelPathDefinition levelPathDefinition = new LevelPathDefinition(
                        worldMapDefinition,
                        int.Parse(levelPathData.Attribute("id").Value),
                        levelPathData.Attribute("level_icon_a_uid").Value,
                        levelPathData.Attribute("level_icon_b_uid").Value);

                    foreach (XElement pathKeyData in levelPathData.Elements("PathKey"))
                    {
                        levelPathDefinition.pathKeys.Add(
                            new LevelPathKey(
                                levelPathDefinition,
                                Loader.loadVector2(pathKeyData.Attribute("p0"), Vector2.Zero),
                                Loader.loadVector2(pathKeyData.Attribute("p1"), Vector2.Zero),
                                Loader.loadVector2(pathKeyData.Attribute("p2"), Vector2.Zero),
                                Loader.loadVector2(pathKeyData.Attribute("p3"), Vector2.Zero)));
                    }

                    worldMapDefinition.levelPathDefinitions.Add(levelPathDefinition);
                }

                definitions.Add(worldMapDefinition);
            }

            return new WorldMapManager(definitions);
        }

        // Load world map states
        public static Dictionary<string, WorldMapState> loadWorldMapStates(List<XElement> allWorldMapStateData)
        {
            Dictionary<string, WorldMapState> worldMapStates = new Dictionary<string, WorldMapState>();

            foreach (XElement worldMapStateData in allWorldMapStateData)
            {
                string worldMapUid = worldMapStateData.Attribute("world_map_uid").Value;
                WorldMapState worldMapState = new WorldMapState(
                        _worldMapManager.getWorldMapDefinition(worldMapUid),
                        bool.Parse(worldMapStateData.Attribute("discovered").Value));

                foreach (XElement levelIconStateData in worldMapStateData.Elements("LevelIconState"))
                {
                    string levelIconUid = levelIconStateData.Attribute("level_icon_uid").Value;

                    worldMapState.levelIconStates.Add(
                        new LevelIconState(
                            _worldMapManager.getLevelIconDefinition(worldMapUid, levelIconUid),
                            bool.Parse(levelIconStateData.Attribute("discovered").Value),
                            bool.Parse(levelIconStateData.Attribute("finished").Value)));
                }

                foreach (XElement levelPathStateData in worldMapStateData.Elements("LevelPathState"))
                {
                    int id = int.Parse(levelPathStateData.Attribute("id").Value);

                    worldMapState.levelPathState.Add(
                        new LevelPathState(
                            _worldMapManager.getLevelPathDefinition(worldMapUid, id),
                            bool.Parse(levelPathStateData.Attribute("discovered").Value)));
                }

                worldMapStates.Add(worldMapUid, worldMapState);
            }

            return worldMapStates;
        }

        // Create new player data
        public static int createPlayerData(SystemManager systemManager, string playerName)
        {
            Logger.log("DataManager.createPlayerData method starting.");

            int unusedPlayerSlot = 0;
            bool created = false;

            while (!created)
            {
                if (File.Exists(_playersDirectory + string.Format("player_data_{0}.xml", unusedPlayerSlot)))
                    unusedPlayerSlot++;
                else
                {
                    Dictionary<string, WorldMapState> worldMapStates = new Dictionary<string, WorldMapState>();
                    List<LevelIconState> startingLevelIconStates = new List<LevelIconState>();

                    // Create world map manager
                    _worldMapManager = createWorldMapManager();

                    // Setup initial data and save it
                    _playerName = playerName;
                    _playerSlot = unusedPlayerSlot;
                    worldMapStates.Add(
                        "oria_world_map",
                        new WorldMapState(_worldMapManager.getWorldMapDefinition("oria_world_map"), true));

                    savePlayerData();
                    created = true;
                }
            }

            Logger.log("DataManager.createPlayerData method finished.");

            return unusedPlayerSlot;
        }

        // Delete player data
        public static void deletePlayerData(int slot)
        {
            Logger.log(string.Format("DataManager.deletePlayerData method starting -- slot: {0}", slot));

            string filePath = _playersDirectory + string.Format("player_data_{0}.xml", slot);

            if (File.Exists(filePath))
                File.Delete(filePath);

            Logger.log("DataManager.deletePlayerData method finished.");
        }

        // Create temporary player data
        public static void createTemporaryPlayerData(SystemManager systemManager)
        {
            _playerData = new PlayerData(systemManager, -1, "Test Character");
        }

        // Load player saves
        public static List<XElement> loadPlayerSaves()
        {
            Logger.log("DataManager.loadPlayerSaves method starting.");

            List<XElement> savesData = new List<XElement>();
            string[] files = Directory.GetFiles(_playersDirectory, "player_data_*.xml");
            foreach (string file in files)
            {
                Logger.log(string.Format("\tloading play data file: {0}...", file));
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    XDocument doc = XDocument.Load(fs);
                    savesData.Add(doc.Element("PlayerData"));
                }
                Logger.log("\tloaded.");
            }

            Logger.log("DataManager.loadPlayerSaves method finished.");

            return savesData;
        }

        // Load player data
        public static void loadPlayerData(int playerSlot)
        {
            Logger.log("DataManager.loadPlayerData method starting.");

            string filePath = _playersDirectory + string.Format("player_data_{0}.xml", playerSlot);

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                XDocument doc = XDocument.Load(fs);
                Dictionary<string, WorldMapState> worldMapStates;

                // Create world map manager and load states
                _worldMapManager = createWorldMapManager();
                worldMapStates = loadWorldMapStates(doc.Elements("WorldMapStates") as List<XElement>);
            }

            Logger.log("DataManager.loadPlayerData method finished.");
        }

        // Save player data
        public static void savePlayerData()
        {
            Logger.log("DataManager.savePlayerData method starting.");

            string filePath = _playersDirectory + string.Format("player_data_{0}.xml", _playerData.playerSlot);

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                XDocument doc = new XDocument(_playerData.data);
                doc.Save(fs);
            }

            Logger.log("DataManager.savePlayerData method finished.");

            /*
            bool saved = false;
            while (!saved)
            {
                if (_storageDevice.IsReady)
                {
                    _storageDevice.Save(PLAYER_CONTAINER, string.Format("player_data_{0}.xml", _playerData.playerSlot), (stream) =>
                        {
                            XDocument doc = new XDocument();
                            doc.Add(_playerData.data);
                            doc.Save(stream);
                        });
                    saved = true;
                }
            }*/
        }
    }
}
