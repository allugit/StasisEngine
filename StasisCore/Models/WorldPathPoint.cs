using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class WorldPathPoint
    {
        protected Vector2 _position;

        public Vector2 position { get { return _position; } set { _position = value; } }

        public WorldPathPoint(Vector2 position)
        {
            _position = position;
        }
    }
}
