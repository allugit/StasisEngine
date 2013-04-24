using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore;

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

        public int id { get { return _id; } }
        public LevelIconState state { get { return _state; } }

        public XElement data
        {
            get
            {
                return new XElement("LevelIconData",
                    new XAttribute("id", _id),
                    new XAttribute("state", _state));
            }
        }

        public LevelIconData(int id, LevelIconState state)
        {
            _id = id;
            _state = state;
        }

        public LevelIconData(XElement data)
        {
            _id = int.Parse(data.Attribute("id").Value);
            _state = (LevelIconState)Enum.Parse(typeof(LevelIconState), data.Attribute("state").Value);
        }
    }
}
