using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisGame.Data;
using StasisCore;
using StasisGame.Systems;

namespace StasisGame.Managers
{
    /* Data manager uses static methods and variables as a convenience, since it's
     * going to be used throughout the program to save and load data as it's needed. */
    public class DataManager
    {
        private static string _rootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/LodersFall";
        private static string _settingsDirectory = _rootDirectory + "settings/";
        private static string _playersDirectory = _rootDirectory + "players/";
        private static string _settingsPath = _settingsDirectory + "settings.xml";
        private static LoderGame _game;
        private static SystemManager _systemManager;
        private static GameSettings _gameSettings;
        private static PlayerData _playerData;

        public static GameSettings gameSettings { get { return _gameSettings; } }
        public static PlayerData playerData { get { return _playerData; } }

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
                    _playerData = new PlayerData(systemManager, unusedPlayerSlot, playerName);
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
                Logger.log(string.Format("\tloading player data file: {0}...", filePath));
                XDocument doc = XDocument.Load(fs);
                _playerData = new PlayerData(_systemManager, doc.Element("PlayerData"));
                Logger.log("\tloaded.");
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
