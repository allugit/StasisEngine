using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;

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
            _worldMapUID = data.Attribute("world_map_uid").Value;
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
        }
    }
}
