using System;
using System.Collections.Generic;
using StasisCore.Models;
using StasisGame.States;

namespace StasisGame.Managers
{
    public class WorldMapManager
    {
        private List<WorldMapDefinition> _worldMapDefinitions;
        private Dictionary<string, WorldMapState> _worldMapStates;

        public List<WorldMapDefinition> worldMapDefinitions { get { return _worldMapDefinitions; } }
        public Dictionary<string, WorldMapState> worldMapStates { get { return _worldMapStates; } set { _worldMapStates = value; } }

        public WorldMapManager(List<WorldMapDefinition> worldMapDefinitions)
        {
            _worldMapDefinitions = worldMapDefinitions;
        }

        public WorldMapDefinition getWorldMapDefinition(string worldMapUid)
        {
            foreach (WorldMapDefinition worldMapDefinition in _worldMapDefinitions)
            {
                if (worldMapDefinition.uid == worldMapUid)
                {
                    return worldMapDefinition;
                }
            }
            return null;
        }

        public LevelIconDefinition getLevelIconDefinition(string worldMapUid, string levelIconUid)
        {
            WorldMapDefinition worldMapDefinition = getWorldMapDefinition(worldMapUid);

            foreach (LevelIconDefinition levelIconDefinition in worldMapDefinition.levelIconDefinitions)
            {
                if (levelIconDefinition.uid == levelIconUid)
                {
                    return levelIconDefinition;
                }
            }
            return null;
        }

        public LevelPathDefinition getLevelPathDefinition(string worldMapUid, int id)
        {
            WorldMapDefinition worldMapDefinition = getWorldMapDefinition(worldMapUid);

            foreach (LevelPathDefinition levelPathDefinition in worldMapDefinition.levelPathDefinitions)
            {
                if (levelPathDefinition.id == id)
                {
                    return levelPathDefinition;
                }
            }
            return null;
        }
    }
}
