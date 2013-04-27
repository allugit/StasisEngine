using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisGame.Data;
using EasyStorage;
using StasisCore;
using StasisGame.Systems;

namespace StasisGame.Managers
{
    /* Data manager uses static methods and variables as a convenience, since it's
     * going to be used throughout the program to save and load data as it's needed. */
    public class DataManager
    {
        public const string GLOBAL_CONTAINER = "LodersFallGlobal";
        public const string PLAYER_CONTAINER = "LodersFallPlayer";
        private static LoderGame _game;
        private static SystemManager _systemManager;
        private static GameSettings _gameSettings;
        private static PlayerData _playerData;
        private static IAsyncSaveDevice _storageDevice;

        public static GameSettings gameSettings { get { return _gameSettings; } }
        public static PlayerData playerData { get { return _playerData; } }
        public static bool ready { get { return _storageDevice.IsReady; } } // Have to wait for Update() to be called once before the storage device is ready... ugh.

        public static void initialize(LoderGame game, SystemManager systemManager)
        {
            _game = game;
            _systemManager = systemManager;

            EasyStorageSettings.SetSupportedLanguages(Language.English);
            SharedSaveDevice sharedSaveDevice = new SharedSaveDevice();

            _storageDevice = sharedSaveDevice;
            sharedSaveDevice.DeviceSelectorCanceled += (s, e) => { e.Response = SaveDeviceEventResponse.Force; };
            sharedSaveDevice.DeviceDisconnected += (s, e) => { e.Response = SaveDeviceEventResponse.Force; };
            sharedSaveDevice.PromptForDevice();

            game.Components.Add(sharedSaveDevice);
            _storageDevice.SaveCompleted += new SaveCompletedEventHandler(saveDevice_SaveCompleted);
        }

        // Save complete event
        private static void saveDevice_SaveCompleted(object sender, FileActionCompletedEventArgs args)
        {
            Console.WriteLine("save completed.");
        }

        // Load game settings
        public static void loadGameSettings()
        {
            if (_storageDevice.FileExists(GLOBAL_CONTAINER, "settings.xml"))
            {
                // Load settings
                _storageDevice.Load(GLOBAL_CONTAINER, "settings.xml", (stream) =>
                {
                    XDocument doc = XDocument.Load(stream);
                    XElement data = doc.Element("Settings");

                    _gameSettings = new GameSettings(data);
                });
            }
            else
            {
                // Create and save default settings
                XDocument doc;

                _gameSettings = new GameSettings(_game);
                doc = new XDocument(_gameSettings.data);
                _storageDevice.Save(GLOBAL_CONTAINER, "settings.xml", (stream) => doc.Save(stream));
            }
        }

        // Save game settings
        public static void saveGameSettings()
        {
            bool saved = false;
            while (!saved)
            {
                if (_storageDevice.IsReady)
                {
                    _storageDevice.Save(GLOBAL_CONTAINER, "settings.xml", (stream) =>
                    {
                        XDocument doc = new XDocument();
                        doc.Add(_gameSettings.data);
                        doc.Save(stream);
                    });
                    saved = true;
                }
            }
        }

        // Create new player data
        public static int createPlayerData(SystemManager systemManager, string playerName)
        {
            bool created = false;
            int unusedPlayerSlot = 0;
            while (!created)
            {
                if (_storageDevice.IsReady)
                {
                    if (_storageDevice.FileExists(PLAYER_CONTAINER, string.Format("player_data_{0}.xml", unusedPlayerSlot)))
                        unusedPlayerSlot++;
                    else
                    {
                        _playerData = new PlayerData(systemManager, unusedPlayerSlot, playerName);
                        savePlayerData();
                        created = true;
                    }
                }
            }
            return unusedPlayerSlot;
        }

        // Create temporary player data
        public static void createTemporaryPlayerData(SystemManager systemManager)
        {
            _playerData = new PlayerData(systemManager, -1, "Test Character");
        }

        // Load player saves
        public static List<XElement> loadPlayerSaves()
        {
            List<XElement> savesData = new List<XElement>();
            bool loaded = false;
            while (!loaded)
            {
                if (_storageDevice.IsReady)
                {
                    string[] files = _storageDevice.GetFiles(PLAYER_CONTAINER, "player_data_*.xml");
                    foreach (string file in files)
                    {
                        _storageDevice.Load(PLAYER_CONTAINER, file, (stream) =>
                            {
                                XDocument doc = XDocument.Load(stream);
                                savesData.Add(doc.Element("PlayerData"));
                            });
                    }
                    loaded = true;
                }
            }
            return savesData;
        }

        // Load player data
        public static void loadPlayerData(int playerSlot)
        {
            bool loaded = false;
            while (!loaded)
            {
                if (_storageDevice.IsReady)
                {
                    _storageDevice.Load(PLAYER_CONTAINER, string.Format("player_data_{0}.xml", playerSlot), (stream) =>
                        {
                            XDocument doc = XDocument.Load(stream);
                            _playerData = new PlayerData(_systemManager, doc.Element("PlayerData"));
                        });
                    loaded = true;
                }
            }
        }

        // Save player data
        public static void savePlayerData()
        {
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
            }
        }
    }
}
