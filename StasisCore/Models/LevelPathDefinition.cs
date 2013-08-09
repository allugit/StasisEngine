using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class LevelPathDefinition
    {
        private WorldMapDefinition _definition;
        private int _id;
        private List<LevelPathKey> _pathKeys;
        private string _levelIconAUid;
        private string _levelIconBUid;

        public WorldMapDefinition definition { get { return _definition; } }
        public int id { get { return _id; } set { _id = value; } }
        public List<LevelPathKey> pathKeys { get { return _pathKeys; } }
        public string levelIconAUid { get { return _levelIconAUid; } set { _levelIconAUid = value; } }
        public string levelIconBUid { get { return _levelIconBUid; } set { _levelIconBUid = value; } }

        public LevelPathDefinition(WorldMapDefinition definition, int id, string levelIconAUid, string levelIconBUid)
        {
            _definition = definition;
            _id = id;
            _levelIconAUid = levelIconAUid;
            _levelIconBUid = levelIconBUid;
            _pathKeys = new List<LevelPathKey>();
        }
    }
}
