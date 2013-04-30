using System;
using System.Collections.Generic;
using StasisCore;
using StasisCore.Models;
using StasisGame.Managers;
using StasisGame.Data;

namespace StasisGame.Systems
{
    public class WorldMapSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private bool _paused;
        private bool _singleStep;
        private WorldMap _worldMap;

        public int defaultPriority { get { return 20; } }
        public SystemType systemType { get { return SystemType.WorldMap; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public WorldMap worldMap { get { return _worldMap; } set { _worldMap = value; } }

        public WorldMapSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void loadWorldMap(WorldMapData worldMapData)
        {
            // Create world map
            _worldMap = new WorldMap(ResourceManager.getResource(worldMapData.worldMapUID));

            // Initialize states from stored world map data
            foreach (LevelIconData levelIconData in worldMapData.levelIconData)
            {
                LevelIcon levelIcon = _worldMap.getLevelIcon(levelIconData.id);
                levelIcon.state = levelIconData.state;
            }
            foreach (WorldPathData worldPathData in worldMapData.worldPathData)
            {
                WorldPath worldPath = _worldMap.getWorldPath(worldPathData.id);
                worldPath.state = worldPathData.state;
            }
        }

        public void update()
        {
        }
    }
}
