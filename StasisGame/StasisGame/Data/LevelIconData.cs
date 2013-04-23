using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisGame.Data
{
    public enum LevelIconState
    {
        Undiscovered,
        Unfinished,
        Finished
    }

    public class LevelIconData
    {
        private int _id;
        private LevelIconState _state;

        public LevelIconData(XElement data)
        {
            throw new NotImplementedException();
        }
    }
}
