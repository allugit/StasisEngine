using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class WorldPath
    {
        protected WorldPathPoint _controlA;
        protected WorldPathPoint _controlB;
        protected WorldPathPoint _pointA;
        protected WorldPathPoint _pointB;
        protected int _id;

        public int id { get { return _id; } }

        public WorldPath()
        {
        }

        public WorldPath(Vector2 controlA, Vector2 controlB, Vector2 pointA, Vector2 pointB, int id)
        {
            _controlA = new WorldPathPoint(controlA);
            _controlB = new WorldPathPoint(controlB);
            _pointA = new WorldPathPoint(pointA);
            _pointB = new WorldPathPoint(pointB);
            _id = id;
        }

        public WorldPath(XElement data)
        {
            _controlA = new WorldPathPoint(Loader.loadVector2(data.Attribute("control_a"), Vector2.Zero));
            _controlB = new WorldPathPoint(Loader.loadVector2(data.Attribute("control_b"), Vector2.Zero));
            _pointA = new WorldPathPoint(Loader.loadVector2(data.Attribute("point_a"), Vector2.Zero));
            _pointB = new WorldPathPoint(Loader.loadVector2(data.Attribute("point_b"), Vector2.Zero));
            _id = int.Parse(data.Attribute("id").Value);
        }
    }
}
