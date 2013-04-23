using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisGame.Data
{
    public enum WorldPathState
    {
        Undiscovered,
        Discovered
    }

    public class WorldPathData
    {
        private int _id;
        private WorldPathState _state;

        public int id { get { return _id; } set { _id = value; } }
        public WorldPathState state { get { return _state; } set { _state = value; } }

        public WorldPathData(XElement data)
        {
            throw new NotImplementedException();
        }
    }
}
