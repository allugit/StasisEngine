using System;
using System.Collections.Generic;
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

        // clone
        public override ItemResource clone()
        {
            return new BlueprintScrapItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _blueprintTag, _scrapTextureTag, _craftingPosition, _craftingAngle, new List<Vector2>(_points));
        }
    }
}
