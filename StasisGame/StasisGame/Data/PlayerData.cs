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
        private List<WorldMapData> _worldMapData;
        private XElement _inventoryData;
        private XElement _toolbarData;

        public int playerSlot { get { return _playerSlot; } set { _playerSlot = value; } }
        public string playerName { get { return _playerName; } set { _playerName = value; } }
        public CurrentLocation currentLocation { get { return _currentLocation; } }
        public XElement inventoryData { get { return _inventoryData; } }
        public XElement toolbarData { get { return _toolbarData; } }

        public XElement data
        {
            get
            {
                PlayerSystem playerSystem = (PlayerSystem)_systemManager.getSystem(SystemType.Player);

                XElement d = new XElement("PlayerData",
                    new XAttribute("name", _playerName),
                    new XAttribute("slot", _playerSlot),
                    _currentLocation.data);

                foreach (WorldMapData worldMapData in _worldMapData)
                    d.Add(worldMapData.data);

                appendInventoryData(d);
                appendToolbarData(d);

                return d;
            }
        }

        // Construct a new PlayerData using default values -- used when creating new players
        public PlayerData(SystemManager systemManager, int playerSlot, string playerName)
        {
            _systemManager = systemManager;
            _playerSlot = playerSlot;
            _playerName = playerName;
            _currentLocation = new CurrentLocation("oria_world_map", Vector2.Zero);
            _worldMapData = new List<WorldMapData>();
            _worldMapData.Add(new WorldMapData("oria_world_map"));
            _worldMapData[0].levelIconData.Add(new LevelIconData(0, LevelIconState.Finished));  // Assume (for now) the player starts at a level icon that has id=0
        }

        // Construct a new PlayerData using data loaded from file -- used when loading an existing player
        public PlayerData(SystemManager systemManager, XElement data)
        {
            _systemManager = systemManager;
            _playerSlot = int.Parse(data.Attribute("slot").Value);
            _playerName = data.Attribute("name").Value;
            _inventoryData = data.Element("Inventory");
            _toolbarData = data.Element("Toolbar");
            _currentLocation = new CurrentLocation(data.Element("CurrentLocation"));
            _worldMapData = new List<WorldMapData>();

            foreach (XElement childData in data.Elements("WorldMapData"))
                _worldMapData.Add(new WorldMapData(childData));
        }

        // Get player specific world data
        public WorldMapData getWorldData(string worldMapUID)
        {
            foreach (WorldMapData worldMapData in _worldMapData)
            {
                if (worldMapData.worldMapUID == worldMapUID)
                    return worldMapData;
            }
            return null;
        }

        // Helper function to construct inventory data from an inventory component
        private void appendInventoryData(XElement d)
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
                d.Add(inventoryData);
            }
        }

        // Helper function to construct toolbar data from a toolbar component
        private void appendToolbarData(XElement d)
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
                d.Add(toolbarData);
            }
        }
    }
}
