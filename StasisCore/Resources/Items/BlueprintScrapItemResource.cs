using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class BlueprintScrapItemResource : ItemResource
    {
        private List<Vector2> _points;
        private List<BlueprintSocketResource> _sockets;
        private string _blueprintTag;
        private string _scrapTextureTag;
        private Vector2 _craftingPosition;
        private float _craftingAngle;

        public List<Vector2> points { get { return _points; } set { _points = value; } }
        public List<BlueprintSocketResource> sockets { get { return _sockets; } }
        public string scrapTextureTag { get { return _scrapTextureTag; } set { _scrapTextureTag = value; } }
        public string blueprintTag { get { return _blueprintTag; } set { _blueprintTag = value; } }
        public Vector2 craftingPosition { get { return _craftingPosition; } set { _craftingPosition = value; } }
        public float craftingAngle { get { return _craftingAngle; } set { _craftingAngle = value; } }

        public BlueprintScrapItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag, string blueprintTag, string scrapTextureTag, Vector2 craftingPosition, float craftingAngle, List<Vector2> points = null, List<BlueprintSocketResource> sockets = null)
            : base(tag, quantity, worldTextureTag, inventoryTextureTag)
        {
            // Default points
            if (points == null)
                points = new List<Vector2>();

            // Default sockets
            if (sockets == null)
                sockets = new List<BlueprintSocketResource>();

            _points = points;
            _sockets = sockets;
            _blueprintTag = blueprintTag;
            _scrapTextureTag = scrapTextureTag;
            _craftingPosition = craftingPosition;
            _craftingAngle = craftingAngle;
            _type = ItemType.BlueprintScrap;
        }

        // fromXML
        public static BlueprintScrapItemResource fromXML(XElement element)
        {
            // Build points
            List<Vector2> points = new List<Vector2>();
            foreach (XElement pointXML in element.Elements("Point"))
                points.Add(XmlLoadHelper.getVector2(pointXML.Value));

            // Build sockets
            List<BlueprintSocketResource> sockets = new List<BlueprintSocketResource>();
            foreach (XElement socketXML in element.Elements("BlueprintSocket"))
                sockets.Add(BlueprintSocketResource.fromXML(socketXML));

            return new BlueprintScrapItemResource(
                element.Attribute("tag").Value,
                int.Parse(element.Attribute("quantity").Value),
                element.Attribute("worldTextureTag").Value,
                element.Attribute("inventoryTextureTag").Value,
                element.Attribute("blueprintTag").Value,
                element.Attribute("scrapTextureTag").Value,
                XmlLoadHelper.getVector2(element.Attribute("craftingPosition").Value),
                float.Parse(element.Attribute("craftingAngle").Value),
                points,
                sockets);
        }

        // toXML
        public override XElement toXML()
        {
            List<XElement> pointsXML = new List<XElement>();
            foreach (Vector2 point in _points)
                pointsXML.Add(new XElement("Point", point));

            //List<XElement> socketsXML = new List<XElement>();
            //foreach (BlueprintSocketResource socket in _sockets)
            //    socketsXML.Add(socket.toXML());

            return new XElement("Item",
                new XAttribute("type", _type),
                new XAttribute("tag", _tag),
                new XAttribute("quantity", _quantity),
                new XAttribute("worldTextureTag", _worldTextureTag),
                new XAttribute("inventoryTextureTag", _inventoryTextureTag),
                new XAttribute("blueprintTag", _blueprintTag),
                new XAttribute("scrapTextureTag", _scrapTextureTag),
                new XAttribute("craftingPosition", _craftingPosition),
                new XAttribute("craftingAngle", _craftingAngle),
                pointsXML);
        }

        // clone
        public override ItemResource clone()
        {
            return new BlueprintScrapItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _blueprintTag, _scrapTextureTag, _craftingPosition, _craftingAngle, new List<Vector2>(_points));
        }
    }
}
