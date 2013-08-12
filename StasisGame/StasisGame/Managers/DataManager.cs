using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;
using StasisGame.Components;
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
        private static EntityManager _entityManager;
        private static GameSettings _gameSettings;
        private static WorldMapManager _worldMapManager;
        private static QuestManager _questManager;
        private static string _playerName;
        private static int _playerSlot;
        private static XElement _playerData;

        public static GameSettings gameSettings { get { return _gameSettings; } }
        public static string playerName { get { return _playerName; } }
        public static int playerSlot { get { return _playerSlot; } }
        public static WorldMapManager worldMapManager { get { return _worldMapManager; } }

        // Initialize
        public static void initialize(LoderGame game, SystemManager systemManager, EntityManager entityManager)
        {
            _game = game;
            _systemManager = systemManager;
            _entityManager = entityManager;

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

        // Create quest manager
        public static QuestManager createQuestManager()
        {
            List<QuestDefinition> questDefinitions = new List<QuestDefinition>();
            List<XElement> allQuestData = ResourceManager.questResources;

            foreach (XElement questData in allQuestData)
            {
                QuestDefinition questDefinition = new QuestDefinition(questData.Attribute("uid").Value, questData.Attribute("title").Value, questData.Attribute("description").Value);

                foreach (XElement objectiveData in questData.Elements("Objective"))
                {
                    questDefinition.objectiveDefinitions.Add(
                        new ObjectiveDefinition(
                            objectiveData.Attribute("uid").Value,
                            objectiveData.Attribute("label").Value,
                            Loader.loadInt(objectiveData.Attribute("starting_value"), 0),
                            Loader.loadInt(objectiveData.Attribute("end_value"), 1),
                            Loader.loadBool(objectiveData.Attribute("optional"), false)));
                }
                questDefinitions.Add(questDefinition);
            }

            return new QuestManager(questDefinitions);
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

        // Create player inventory component
        public static InventoryComponent createPlayerInventoryComponent()
        {
            XElement inventoryData = _playerData.Element("Inventory");
            InventoryComponent inventoryComponent;

            if (inventoryData == null)
            {
                inventoryComponent = new InventoryComponent(32);
            }
            else
            {
                int slots = int.Parse(inventoryData.Attribute("slots").Value);
                inventoryComponent = new InventoryComponent(slots);
                foreach (XElement itemData in inventoryData.Elements("Item"))
                {
                    string itemUID = itemData.Attribute("item_uid").Value;
                    XElement itemResource = ResourceManager.getResource(itemUID);
                    ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), itemResource.Attribute("type").Value);
                    Texture2D inventoryTexture = ResourceManager.getTexture(itemResource.Attribute("inventory_texture_uid").Value);
                    int quantity = int.Parse(itemData.Attribute("quantity").Value);
                    bool inWorld = false;
                    bool hasAiming = Loader.loadBool(itemResource.Attribute("adds_reticle"), false);
                    int maxRange = Loader.loadInt(itemResource.Attribute("range"), 0);

                    ItemComponent itemComponent = new ItemComponent(itemUID, itemType, inventoryTexture, quantity, inWorld, hasAiming, maxRange);
                    inventoryComponent.addItem(itemComponent);
                }
            }
            return inventoryComponent;
        }

        // Create player toolbar component
        public static ToolbarComponent createPlayerToolbarComponent(int playerId)
        {
            XElement toolbarData = _playerData.Element("Toolbar");
            ToolbarComponent toolbarComponent;

            if (toolbarData == null)
            {
                toolbarComponent = new ToolbarComponent(4, playerId);
            }
            else
            {
                int slots = int.Parse(toolbarData.Attribute("slots").Value);
                toolbarComponent = new ToolbarComponent(slots, playerId);
                EquipmentSystem equipmentSystem = (EquipmentSystem)_systemManager.getSystem(SystemType.Equipment);
                InventoryComponent inventoryComponent = _entityManager.getComponent(playerId, ComponentType.Inventory) as InventoryComponent;

                foreach (XElement slotData in toolbarData.Elements("Slot"))
                {
                    int slotId = int.Parse(slotData.Attribute("id").Value);
                    int inventorySlot = int.Parse(slotData.Attribute("inventory_slot").Value);
                    ItemComponent itemComponent = inventoryComponent.getItem(inventorySlot);

                    equipmentSystem.assignItemToToolbar(itemComponent, toolbarComponent, slotId);
                }
            }
            return toolbarComponent;
        }

        // Create new player data
        public static int createPlayerData(string playerName)
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
                    WorldMapState startingWorldMapState;

                    _playerName = playerName;
                    _playerSlot = unusedPlayerSlot;

                    // Create managers
                    _worldMapManager = createWorldMapManager();
                    _questManager = createQuestManager();

                    // Create starting world map states
                    startingWorldMapState = new WorldMapState(_worldMapManager.getWorldMapDefinition("oria_world_map"), true);
                    startingWorldMapState.levelIconStates.Add(
                        new LevelIconState(
                            _worldMapManager.getLevelIconDefinition("oria_world_map", "home_village"),
                            true,
                            true));
                    _worldMapManager.worldMapStates.Add(
                        "oria_world_map",
                        startingWorldMapState);

                    // Create starting quest states
                    _questManager.addNewQuestState("helping_dagny_1");

                    // Save data
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
        public static void createTemporaryPlayerData()
        {
            createPlayerData("McCormick");
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

                _playerData = doc.Element("PlayerData");
                _playerSlot = playerSlot;
                _playerName = _playerData.Attribute("name").Value;
                _worldMapManager = createWorldMapManager();
                _worldMapManager.worldMapStates = loadWorldMapStates(new List<XElement>(_playerData.Elements("WorldMapState")));
            }

            Logger.log("DataManager.loadPlayerData method finished.");
        }

        // Save player data
        public static void savePlayerData()
        {
            Logger.log("DataManager.savePlayerData method starting.");

            string filePath = _playersDirectory + string.Format("player_data_{0}.xml", _playerSlot);

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                XDocument doc = new XDocument();

                // Basic player data
                _playerData = new XElement(
                    "PlayerData",
                    new XAttribute("name", _playerName),
                    new XAttribute("slot", _playerSlot));

                // World map states
                foreach (WorldMapState worldMapState in _worldMapManager.worldMapStates.Values)
                {
                    XElement worldMapStateData = new XElement(
                        "WorldMapState",
                        new XAttribute("world_map_uid", worldMapState.definition.uid),
                        new XAttribute("discovered", worldMapState.discovered));

                    foreach (LevelIconState levelIconState in worldMapState.levelIconStates)
                    {
                        worldMapStateData.Add(
                            new XElement("LevelIconState",
                                new XAttribute("level_icon_uid", levelIconState.definition.uid),
                                new XAttribute("discovered", levelIconState.discovered),
                                new XAttribute("finished", levelIconState.finished)));
                    }

                    foreach (LevelPathState levelPathState in worldMapState.levelPathState)
                    {
                        worldMapStateData.Add(new XElement("LevelPathState", new XAttribute("discovered", levelPathState.discovered)));
                    }

                    _playerData.Add(worldMapStateData);
                }

                // Quest states
                foreach (QuestState questState in _questManager.questStates.Values)
                {
                    XElement questStateData = new XElement("QuestState", new XAttribute("quest_uid", questState.definition.uid));

                    foreach (ObjectiveState objectiveState in questState.objectiveStates)
                    {
                        questStateData.Add(
                            new XElement("ObjectiveState",
                                new XAttribute("objective_uid", objectiveState.definition.uid),
                                new XAttribute("current_value", objectiveState.currentValue)));
                    }

                    _playerData.Add(questStateData);
                }

                doc.Add(_playerData);
                doc.Save(fs);
            }

            Logger.log("DataManager.savePlayerData method finished.");
        }
    }
}
