using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class WorldMapState
    {
        private WorldMapDefinition _definition;
        private List<LevelIconState> _levelIconStates;
        private List<LevelPathState> _levelPathStates;
        private bool _discovered;

        public WorldMapDefinition definition { get { return _definition; } set { _definition = value; } }
        public bool discovered { get { return _discovered; } set { _discovered = value; } }
        public List<LevelIconState> levelIconStates { get { return _levelIconStates; } set { _levelIconStates = value; } }
        public List<LevelPathState> levelPathState { get { return _levelPathStates; } set { _levelPathStates = value; } }

        public WorldMapState(WorldMapDefinition definition, bool discovered)
        {
            _definition = definition;
            _discovered = discovered;
            _levelIconStates = new List<LevelIconState>();
            _levelPathStates = new List<LevelPathState>();
        }
    }
}
