using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisGame.Systems;
using StasisGame.Managers;
using StasisGame.Components;
using StasisCore;
using StasisCore.Models;

namespace StasisGame.Data
{
    public class PlayerData
    {
        private SystemManager _systemManager;
        private int _playerSlot;
        private string _playerName;
        private CurrentLocation _currentLocation;
        private XElement _loadedData;

        public int playerSlot { get { return _playerSlot; } set { _playerSlot = value; } }
        public string playerName { get { return _playerName; } set { _playerName = value; } }
        public CurrentLocation currentLocation { get { return _currentLocation; } }
        public XElement inventoryData { get { return _loadedData.Element("Inventory"); } }
        public XElement toolbarData { get { return _loadedData.Element("Toolbar"); } }

        public XElement data
        {
            get
            {
                XElement playerData = new XElement("PlayerData",
                    new XAttribute("name", _playerName),
                    new XAttribute("slot", _playerSlot),
                    _currentLocation.data);

                appendInventoryData(playerData);
                appendToolbarData(playerData);
                appendWorldMapData(playerData);

                return playerData;
            }
        }

        // Construct a new PlayerData using default values -- used when creating new players
        public PlayerData(SystemManager systemManager, int playerSlot, string playerName)
        {
            _systemManager = systemManager;
            _playerSlot = playerSlot;
            _playerName = playerName;
            _currentLocation = new CurrentLocation("oria_world_map", Vector2.Zero);

            // Default world map states -- needs to be set because appendWorldMapData requires existing data, and the first level needs to be visible
            _loadedData = new XElement("PlayerData");
            _loadedData.Add(
                new XElement("WorldMapData",
                    new XAttribute("world_map_uid", "oria_world_map"),
                    new XElement("LevelIconData",
                        new XAttribute("id", 0),
                        new XAttribute("state", LevelIconState.Finished))));

            //_worldMapData = new List<WorldMapData>();
            //_worldMapData.Add(new WorldMapData("oria_world_map"));
            //_worldMapData[0].levelIconData.Add(new LevelIconData(0, LevelIconState.Finished));  // Assume (for now) the player starts at a level icon that has id=0
        }

        // Construct a new PlayerData using data loaded from file -- used when loading an existing player
        public PlayerData(SystemManager systemManager, XElement data)
        {
            _systemManager = systemManager;
            _playerSlot = int.Parse(data.Attribute("slot").Value);
            _playerName = data.Attribute("name").Value;
            _loadedData = data;
            _currentLocation = new CurrentLocation(data.Element("CurrentLocation"));
        }

        // getWorldMapData -- Get state data for a specific world map
        public XElement getWorldMapData(string uid)
        {
            foreach (XElement data in _loadedData.Elements("WorldMapData"))
            {
                if (data.Attribute("world_map_uid").Value == uid)
                    return data;
            }
            return null;
        }

        // Helper function to construct inventory data from an inventory component
        private void appendInventoryData(XElement playerData)
        {
            PlayerSystem playerSystem = (PlayerSystem)_systemManager.getSystem(SystemType.Player);

            if (playerSystem != null)
            {
                InventoryComponent inventoryComponent = (InventoryComponent)playerSystem.entityManager.getComponent(playerSystem.playerId, ComponentType.Inventory);
                XElement inventoryData = new XElement("Inventory", new XAttribute("slots", inventoryComponent.slots));

                if (inventoryComponent != null)
                {
                    foreach (KeyValuePair<int, ItemComponent> slotItemPair in inventoryComponent.inventory)
                    {
                        inventoryData.Add(new XElement("Item",
                            new XAttribute("slot", slotItemPair.Key),
                            new XAttribute("item_uid", slotItemPair.Value.itemUID),
                            new XAttribute("quantity", slotItemPair.Value.quantity)));
                    }
                }
                playerData.Add(inventoryData);
            }
        }

        // Helper function to construct toolbar data from a toolbar component
        private void appendToolbarData(XElement playerData)
        {
            PlayerSystem playerSystem = (PlayerSystem)_systemManager.getSystem(SystemType.Player);

            if (playerSystem != null)
            {
                InventoryComponent inventoryComponent = (InventoryComponent)playerSystem.entityManager.getComponent(playerSystem.playerId, ComponentType.Inventory);
                ToolbarComponent toolbarComponent = (ToolbarComponent)playerSystem.entityManager.getComponent(playerSystem.playerId, ComponentType.Toolbar);
                XElement toolbarData = new XElement("Toolbar", new XAttribute("slots", toolbarComponent.slots));
                if (inventoryComponent != null && toolbarComponent != null)
                {
                    foreach (KeyValuePair<int, ItemComponent> toolbarSlotItemPair in toolbarComponent.inventory)
                    {
                        foreach (KeyValuePair<int, ItemComponent> inventorySlotItemPair in inventoryComponent.inventory)
                        {
                            if (toolbarSlotItemPair.Value == inventorySlotItemPair.Value)
                            {
                                toolbarData.Add(new XElement("Slot",
                                    new XAttribute("id", toolbarSlotItemPair.Key),
                                    new XAttribute("inventory_slot", inventorySlotItemPair.Key)));
                            }
                        }
                    }
                }
                playerData.Add(toolbarData);
            }
        }

        // appendWorldMapData -- Helper function to construct world map data
        // This method uses world map data that was loaded as a baseline, and overwrites data for the currently loaded world map.
        // PlayerData should be saved before a new world map is loaded, otherwise changes will be lost.
        private void appendWorldMapData(XElement playerData)
        {
            WorldMapSystem worldMapSystem = (WorldMapSystem)_systemManager.getSystem(SystemType.WorldMap);
            List<XElement> worldMapData = new List<XElement>(_loadedData.Elements("WorldMapData"));

            if (worldMapSystem != null)
            {
                foreach (XElement element in worldMapData)
                {
                    if (element.Attribute("world_map_uid").Value == worldMapSystem.worldMap.uid)
                    {
                        List<XElement> levelIconData = new List<XElement>();
                        List<XElement> worldPathData = new List<XElement>();

                        foreach (LevelIcon levelIcon in worldMapSystem.worldMap.levelIcons)
                        {
                            levelIconData.Add(new XElement("LevelIconData", new XAttribute("id", levelIcon.id), new XAttribute("state", levelIcon.state)));
                        }
                        foreach (WorldPath worldPath in worldMapSystem.worldMap.worldPaths)
                        {
                            worldPathData.Add(new XElement("WorldPathData", new XAttribute("id", worldPath.id), new XAttribute("state", worldPath.state)));
                        }

                        element.RemoveNodes();
                        element.Add(levelIconData);
                        element.Add(worldPathData);
                    }
                }
            }

            playerData.Add(worldMapData);
        }
    }
}
