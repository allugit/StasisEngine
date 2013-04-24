using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisGame.Data
{
    public class CurrentLocation
    {
        private string _worldMapUID;
        private Vector2 _position;

        public string worldMapUID { get { return _worldMapUID; } set { _worldMapUID = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }

        public XElement data
        {
            get
            {
                return new XElement("CurrentLocation",
                    new XAttribute("world_map_uid", _worldMapUID),
                    new XAttribute("position", _position));
            }
        }

        public CurrentLocation(string worldMapUID, Vector2 position)
        {
            _worldMapUID = worldMapUID;
            _position = position;
        }

        public CurrentLocation(XElement data)
        {
            throw new NotImplementedException();
        }
    }
}
