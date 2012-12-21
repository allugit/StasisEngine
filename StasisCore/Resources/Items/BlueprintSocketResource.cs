using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Resources
{
    public class BlueprintSocketResource
    {
        private string _scrapTagA;
        private string _scrapTagB;
        private Vector2 _relativePoint;

        public string scrapTagA { get { return _scrapTagA; } }
        public string scrapTagB { get { return _scrapTagB; } }
        public Vector2 relativePoint { get { return _relativePoint; } }

        public BlueprintSocketResource(string scrapTagA, string scrapTagB, Vector2 relativePoint)
        {
            _scrapTagA = scrapTagA;
            _scrapTagB = scrapTagB;
            _relativePoint = relativePoint;
        }

        // fromXML
        public static BlueprintSocketResource fromXML(XElement element)
        {
            return new BlueprintSocketResource(
                element.Attribute("scrapTagA").Value,
                element.Attribute("scrapTagB").Value,
                XmlLoadHelper.getVector2(element.Attribute("relativePoint").Value));

        }

        // toXML
        public XElement toXML()
        {
            return new XElement("BlueprintSocket",
                new XAttribute("scrapTagA", _scrapTagA),
                new XAttribute("scrapTagB", _scrapTagB),
                new XAttribute("relativePoint", _relativePoint));
        }
    }
}
