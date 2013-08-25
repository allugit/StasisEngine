using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;
using StasisGame.Components;
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
        private static ItemManager _itemManager;
        private static DialogueManager _dialogueManager;
        private static string _playerName;
        private static int _playerSlot;
        private static XElement _playerData;
        private static Dictionary<string, bool> _customFlags;
        private static Dictionary<string, int> _customValues;
        private static Dictionary<string, string> _customStrings;

        public static GameSettings gameSettings { get { return _gameSettings; } }
        public static string playerName { get { return _playerName; } }
        public static int playerSlot { get { return _playerSlot; } }
        public static WorldMapManager worldMapManager { get { return _worldMapManager; } }
        public static QuestManager questManager { get { return _questManager; } }
        public static ItemManager itemManager { get { return _itemManager; } }

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

            _customFlags = new Dictionary<string, bool>();
            _customValues = new Dictionary<string, int>();
            _customStrings = new Dictionary<string, string>();
        }

        // Set custom flag
        public static void setCustomFlag(string flagUid, bool value)
        {
            _customFlags.Add(flagUid, value);
        }

        // Get custom flag
        public static bool getCustomFlag(string flagUid)
        {
            return _customFlags[flagUid];
        }

        // Set custom value
        public static void setCustomValue(string valueUid, int value)
        {
            _customValues.Add(valueUid, value);
        }

        // Get custom value
        public static int getCustomValue(string valueUid)
        {
            return _customValues[valueUid];
        }

        // Set custom string
        public static void setCustomString(string stringUid, string value)
        {
            _customStrings.Add(stringUid, value);
        }

        // Get custom string
        public static string getCustomString(string stringUid)
        {
            return _customStrings[stringUid];
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
        private static QuestManager createQuestManager()
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

        // Create item manager
        private static ItemManager createItemManager()
        {
            List<ItemDefinition> itemDefinitions = new List<ItemDefinition>();
            List<XElement> allItemData = ResourceManager.itemResources;
            List<XElement> allBlueprintData = ResourceManager.blueprintResources;

            // Items
            foreach (XElement itemData in allItemData)
            {
                itemDefinitions.Add(
                    new ItemDefinition(
                        itemData.Attribute("uid").Value,
                        bool.Parse(itemData.Attribute("has_aiming_component").Value),
                        float.Parse(itemData.Attribute("min_range_limit").Value),
                        float.Parse(itemData.Attribute("max_range_limit").Value),
                        bool.Parse(itemData.Attribute("stackable").Value),
                        itemData.Attribute("inventory_texture_uid").Value,
                        itemData.Attribute("world_texture_uid").Value));
            }

            // Blueprints
            foreach (XElement blueprintData in allBlueprintData)
            {
                itemDefinitions.Add(
                    new ItemDefinition(
                        blueprintData.Attribute("uid").Value,
                        false,
                        0,
                        1,
                        false,
                        blueprintData.Attribute("inventory_texture_uid").Value,
                        blueprintData.Attribute("world_texture_uid").Value));
            }

            return new ItemManager(itemDefinitions);
        }

        // Create dialogue manager
        public static DialogueManager createDialogueManager()
        {
            return new DialogueManager(_systemManager, _entityManager);
        }

        // Load world map states
        private static Dictionary<string, WorldMapState> loadWorldMapStates(List<XElement> allWorldMapStateData)
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

        // Load quest states
        private static Dictionary<string, QuestState> loadQuestStates(List<XElement> allQuestStateData)
        {
            Dictionary<string, QuestState> questStates = new Dictionary<string, QuestState>();

            foreach (XElement questStateData in allQuestStateData)
            {
                string questUid = questStateData.Attribute("quest_uid").Value;
                QuestState questState = new QuestState(_questManager.getQuestDefinition(questUid));

                foreach (XElement objectiveStateData in questStateData.Elements("ObjectiveState"))
                {
                    questState.objectiveStates.Add(
                        new ObjectiveState(
                            _questManager.getObjectiveDefinition(questUid, objectiveStateData.Attribute("objective_uid").Value),
                            int.Parse(objectiveStateData.Attribute("current_value").Value)));
                }
                questStates.Add(questUid, questState);
            }

            return questStates;
        }

        // Load dialogue states
        private static Dictionary<string, DialogueState> loadDialogueStates(List<XElement> allDialogueStateData)
        {
            Dictionary<string, DialogueState> dialogueStates = new Dictionary<string, DialogueState>();

            foreach (XElement dialogueStateData in allDialogueStateData)
            {
                string dialogueUid = dialogueStateData.Attribute("dialogue_uid").Value;
                DialogueState dialogueState = new DialogueState(
                    _dialogueManager.getDialogueDefinition(dialogueUid),
                    dialogueStateData.Attribute("current_node_uid").Value);

                dialogueStates.Add(dialogueUid, dialogueState);
            }

            return dialogueStates;
        }

        // Load player inventory
        public static void loadPlayerInventory()
        {
            XElement inventoryData = _playerData.Element("InventoryState");
            InventoryComponent inventoryComponent = new InventoryComponent(int.Parse(inventoryData.Attribute("slots").Value));
            EquipmentSystem equipmentSystem = _systemManager.getSystem(SystemType.Equipment) as EquipmentSystem;

            _entityManager.addComponent("global", PlayerSystem.PLAYER_ID, inventoryComponent);
            foreach (XElement itemStateData in inventoryData.Elements("ItemState"))
            {
                ItemDefinition itemDefinition = _itemManager.getItemDefinition(itemStateData.Attribute("item_uid").Value);
                ItemState itemState = new ItemState(
                    int.Parse(itemStateData.Attribute("quantity").Value),
                    float.Parse(itemStateData.Attribute("current_range_limit").Value),
                    false);
                ItemComponent itemComponent = new ItemComponent(itemDefinition, itemState, ResourceManager.getTexture(itemDefinition.inventoryTextureUid));

                equipmentSystem.addInventoryItem(inventoryComponent, itemComponent);
            }
        }

        // Load player toolbar
        public static void loadPlayerToolbar()
        {
            XElement toolbarData = _playerData.Element("ToolbarState");
            ToolbarComponent toolbarComponent = new ToolbarComponent(int.Parse(toolbarData.Attribute("slots").Value), PlayerSystem.PLAYER_ID);
            EquipmentSystem equipmentSystem = _systemManager.getSystem(SystemType.Equipment) as EquipmentSystem;
            InventoryComponent inventoryComponent = _entityManager.getComponent("global", PlayerSystem.PLAYER_ID, ComponentType.Inventory) as InventoryComponent;

            _entityManager.addComponent("global", PlayerSystem.PLAYER_ID, toolbarComponent);
            foreach (XElement slotData in toolbarData.Elements("Slot"))
            {
                equipmentSystem.assignItemToToolbar(
                    "global",
                    equipmentSystem.getInventoryItem(inventoryComponent, int.Parse(slotData.Attribute("inventory_slot").Value)),
                    toolbarComponent,
                    int.Parse(slotData.Attribute("slot_id").Value));
            }

            equipmentSystem.selectToolbarSlot("global", toolbarComponent, int.Parse(toolbarData.Attribute("selected_index").Value));
        }

        // Load custom flags
        private static void loadCustomFlags()
        {
            List<XElement> allCustomFlagData = new List<XElement>(_playerData.Elements("CustomFlag"));

            _customFlags.Clear();
            foreach (XElement customFlagData in allCustomFlagData)
            {
                _customFlags.Add(
                    customFlagData.Attribute("uid").Value,
                    bool.Parse(customFlagData.Attribute("value").Value));
            }
        }

        // Load custom values
        private static void loadCustomValues()
        {
            List<XElement> allCustomValueData = new List<XElement>(_playerData.Elements("CustomValue"));

            _customValues.Clear();
            foreach (XElement customValueData in allCustomValueData)
            {
                _customValues.Add(
                    customValueData.Attribute("uid").Value,
                    int.Parse(customValueData.Attribute("value").Value));
            }
        }

        // Load custom strings
        private static void loadCustomStrings()
        {
            List<XElement> allCustomStringData = new List<XElement>(_playerData.Elements("CustomString"));

            _customStrings.Clear();
            foreach (XElement customStringData in allCustomStringData)
            {
                _customStrings.Add(
                    customStringData.Attribute("uid").Value,
                    customStringData.Attribute("value").Value);
            }
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
                    _itemManager = createItemManager();
                    _dialogueManager = createDialogueManager();

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
                    //_questManager.addNewQuestState("helping_dagny_1");

                    // Create inventory and toolbar
                    _entityManager.addComponent("global", PlayerSystem.PLAYER_ID, new InventoryComponent(32));
                    _entityManager.addComponent("global", PlayerSystem.PLAYER_ID, new ToolbarComponent(4, PlayerSystem.PLAYER_ID));

                    // Custom flags
                    //setCustomFlag("new_game", true);

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

                // Create managers
                _worldMapManager = createWorldMapManager();
                _questManager = createQuestManager();
                _itemManager = createItemManager();
                _dialogueManager = createDialogueManager();

                // Basic player data
                _playerData = doc.Element("PlayerData");
                _playerSlot = playerSlot;
                _playerName = _playerData.Attribute("name").Value;

                // World map states
                _worldMapManager.worldMapStates = loadWorldMapStates(new List<XElement>(_playerData.Elements("WorldMapState")));

                // Quest states
                _questManager.questStates = loadQuestStates(new List<XElement>(_playerData.Elements("QuestState")));

                // Dialogue states
                _dialogueManager.dialogueStates = loadDialogueStates(new List<XElement>(_playerData.Elements("DialogueState")));

                // Inventory and toolbar
                loadPlayerInventory();
                loadPlayerToolbar();

                // Custom flags and values
                loadCustomFlags();
                loadCustomValues();
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
                InventoryComponent inventoryComponent = _entityManager.getComponent("global", PlayerSystem.PLAYER_ID, ComponentType.Inventory) as InventoryComponent;
                ToolbarComponent toolbarComponent = _entityManager.getComponent("global", PlayerSystem.PLAYER_ID, ComponentType.Toolbar) as ToolbarComponent;
                EquipmentSystem equipmentSystem = _systemManager.getSystem(SystemType.Equipment) as EquipmentSystem;
                XElement inventoryData = new XElement("InventoryState");
                XElement toolbarData = new XElement("ToolbarState");
                List<XElement> customFlagsData = new List<XElement>();
                List<XElement> customValuesData = new List<XElement>();
                List<XElement> customStringsData = new List<XElement>();

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

                // Inventory states
                inventoryData.SetAttributeValue("slots", inventoryComponent.slots);
                foreach (KeyValuePair<int, ItemComponent> slotItemPair in inventoryComponent.inventory)
                {
                    inventoryData.Add(
                        new XElement(
                            "ItemState",
                            new XAttribute("slot_id", slotItemPair.Key),
                            new XAttribute("item_uid", slotItemPair.Value.definition.uid),
                            new XAttribute("quantity", slotItemPair.Value.state.quantity),
                            new XAttribute("current_range_limit", slotItemPair.Value.state.currentRangeLimit)));
                }
                _playerData.Add(inventoryData);

                // Toolbar states
                toolbarData.SetAttributeValue("slots", toolbarComponent.slots);
                toolbarData.SetAttributeValue("selected_index", toolbarComponent.selectedIndex);
                foreach (KeyValuePair<int, ItemComponent> slotItemPair in toolbarComponent.inventory)
                {
                    if (slotItemPair.Value != null)
                    {
                        toolbarData.Add(
                            new XElement(
                                "Slot",
                                new XAttribute("slot_id", slotItemPair.Key),
                                new XAttribute("inventory_slot", equipmentSystem.getInventorySlot(inventoryComponent, slotItemPair.Value))));
                    }
                }
                _playerData.Add(toolbarData);

                // Custom flags
                foreach (KeyValuePair<string, bool> uidFlagPair in _customFlags)
                {
                    customFlagsData.Add(new XElement("CustomFlag", new XAttribute("uid", uidFlagPair.Key), new XAttribute("value", uidFlagPair.Value)));
                }
                _playerData.Add(customFlagsData);

                // Custom values
                foreach (KeyValuePair<string, int> uidValuePair in _customValues)
                {
                    customValuesData.Add(new XElement("CustomValue", new XAttribute("uid", uidValuePair.Key), new XAttribute("value", uidValuePair.Value)));
                }
                _playerData.Add(customValuesData);

                // Custom values
                foreach (KeyValuePair<string, string> uidStringPair in _customStrings)
                {
                    customStringsData.Add(new XElement("CustomString", new XAttribute("uid", uidStringPair.Key), new XAttribute("value", uidStringPair.Value)));
                }
                _playerData.Add(customStringsData);

                doc.Add(_playerData);
                doc.Save(fs);
            }

            Logger.log("DataManager.savePlayerData method finished.");
        }
    }
}
