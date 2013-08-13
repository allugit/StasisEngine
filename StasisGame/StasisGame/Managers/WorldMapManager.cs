using System;
using System.Collections.Generic;
using StasisCore.Models;

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
            _worldMapStates = new Dictionary<string, WorldMapState>();
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

        public WorldMapState getWorldMapState(string worldMapUid)
        {
            return worldMapStates[worldMapUid];
        }

        public LevelIconState getLevelIconState(string worldMapUid, string levelIconUid)
        {
            foreach (LevelIconState levelIconState in _worldMapStates[worldMapUid].levelIconStates)
            {
                if (levelIconState.definition.uid == levelIconUid)
                {
                    return levelIconState;
                }
            }
            return null;
        }

        public bool isLevelPathDiscovered(string worldMapUid, int id)
        {
            LevelPathDefinition levelPathDefinition = getLevelPathDefinition(worldMapUid, id);
            LevelIconState levelA = getLevelIconState(worldMapUid, levelPathDefinition.levelIconAUid);
            LevelIconState levelB = getLevelIconState(worldMapUid, levelPathDefinition.levelIconBUid);

            if (levelA == null || levelB == null)
                return false;

            return levelA.discovered && levelB.discovered;
        }
    }
}
