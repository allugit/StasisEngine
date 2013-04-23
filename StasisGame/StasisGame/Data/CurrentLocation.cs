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

        public CurrentLocation(XElement data)
        {
            throw new NotImplementedException();
        }
    }
}
