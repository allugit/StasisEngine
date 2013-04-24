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

        public XElement data
        {
            get
            {
                return new XElement("WorldPathData",
                    new XAttribute("id", _id),
                    new XAttribute("state", _state));
            }
        }

        public WorldPathData(XElement data)
        {
            _id = int.Parse(data.Attribute("id").Value);
            _state = (WorldPathState)Enum.Parse(typeof(WorldPathState), data.Attribute("state").Value);
        }
    }
}
