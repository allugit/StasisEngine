using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorWorldPath : WorldPath
    {
        private EditorWorldMap _worldMap;

        public XElement data
        {
            get
            {
                return new XElement("WorldPath",
                    new XAttribute("control_a", _controlA.position),
                    new XAttribute("control_b", _controlB.position),
                    new XAttribute("point_a", _pointA.position),
                    new XAttribute("point_b", _pointB.position),
                    new XAttribute("id", _id));
            }
        }

        public EditorWorldPath(EditorWorldMap worldMap, Vector2 controlA, Vector2 controlB, Vector2 pointA, Vector2 pointB, int id)
        {
            _worldMap = worldMap;
            _controlA = new EditorWorldPathPoint(this, controlA);
            _controlB = new EditorWorldPathPoint(this, controlB);
            _pointA = new EditorWorldPathPoint(this, pointA);
            _pointB = new EditorWorldPathPoint(this, pointB);
            _id = id;
        }

        public EditorWorldPath(EditorWorldMap worldMap, XElement data)
        {
            _worldMap = worldMap;
            _controlA = new EditorWorldPathPoint(this, Loader.loadVector2(data.Attribute("control_a"), Vector2.Zero));
            _controlB = new EditorWorldPathPoint(this, Loader.loadVector2(data.Attribute("control_b"), Vector2.Zero));
            _pointA = new EditorWorldPathPoint(this, Loader.loadVector2(data.Attribute("point_a"), Vector2.Zero));
            _pointB = new EditorWorldPathPoint(this, Loader.loadVector2(data.Attribute("point_b"), Vector2.Zero));
            _id = int.Parse(data.Attribute("id").Value);
        }

        public void delete()
        {
            _worldMap.worldPaths.Remove(this);
        }
    }
}
