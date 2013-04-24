using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisGame.Systems;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Data
{
    public class PlayerData
    {
        private SystemManager _systemManager;
        private int _playerSlot;
        private string _playerName;
        private CurrentLocation _currentLocation;
        private List<WorldMapData> _worldMapData;

        public int playerSlot { get { return _playerSlot; } set { _playerSlot = value; } }
        public string playerName { get { return _playerName; } set { _playerName = value; } }

        public XElement data
        {
            get
            {
                PlayerSystem playerSystem = (PlayerSystem)_systemManager.getSystem(SystemType.Player);
                XElement inventoryData = constructInventoryData();
                XElement toolbarData = constructToolbarData();
                XElement worldMapDatas = new XElement("WorldMapDatas");

                foreach (WorldMapData worldMapData in _worldMapData)
                    worldMapDatas.Add(worldMapData.data);

                return new XElement("PlayerData",
                    new XAttribute("name", _playerName),
                    inventoryData,
                    toolbarData,
                    _currentLocation.data,
                    worldMapDatas);
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
            _worldMapData[0].levelIconData.Add(new LevelIconData(0, LevelIconState.Finished));
        }

        // Construct a new PlayerData using data loaded from file -- used when loading an existing player
        public PlayerData(SystemManager systemManager, XElement data)
        {
            _systemManager = systemManager;
            throw new NotImplementedException();
        }

        // Helper function to construct inventory data from an inventory component
        private XElement constructInventoryData()
        {
            PlayerSystem playerSystem = (PlayerSystem)_systemManager.getSystem(SystemType.Player);
            XElement inventoryData = new XElement("Inventory");

            if (playerSystem != null)
            {
                InventoryComponent inventoryComponent = (InventoryComponent)playerSystem.entityManager.getComponent(playerSystem.playerId, ComponentType.Inventory);
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
            }
            return inventoryData;
        }

        // Helper function to construct toolbar data from a toolbar component
        private XElement constructToolbarData()
        {
            PlayerSystem playerSystem = (PlayerSystem)_systemManager.getSystem(SystemType.Player);
            XElement toolbarData = new XElement("Toolbar");

            if (playerSystem != null)
            {
                InventoryComponent inventoryComponent = (InventoryComponent)playerSystem.entityManager.getComponent(playerSystem.playerId, ComponentType.Inventory);
                ToolbarComponent toolbarComponent = (ToolbarComponent)playerSystem.entityManager.getComponent(playerSystem.playerId, ComponentType.Toolbar);
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
            }
            return toolbarData;
        }
    }
}
