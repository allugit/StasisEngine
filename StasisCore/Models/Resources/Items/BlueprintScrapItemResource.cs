using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class BlueprintScrapItemResource : ItemResource
    {
        private List<Vector2> _points;
        private BlueprintScrapProperties _blueprintScrapProperties;
        private BlueprintItemResource _owner;

        public BlueprintScrapProperties blueprintScrapProperties { get { return _blueprintScrapProperties; } }
        public BlueprintItemResource owner { get { return _owner; } }
        public List<Vector2> points { get { return _points; } set { _points = value; } }

        public BlueprintScrapItemResource(List<Vector2> points = null, ItemProperties generalProperties = null, ItemProperties blueprintScrapProperties = null)
            : base()
        {
            // Default points
            if (points == null)
                points = new List<Vector2>();

            // Default general item properties
            if (generalProperties == null)
                generalProperties = new GeneralItemProperties("", 1, "", "");

            // Default blueprint scrap properties
            if (blueprintScrapProperties == null)
                blueprintScrapProperties = new BlueprintScrapProperties("", "");

            _points = points;
            _generalProperties = generalProperties as GeneralItemProperties;
            _blueprintScrapProperties = blueprintScrapProperties as BlueprintScrapProperties;
            _type = ItemType.BlueprintScrap;
        }

        // clone
        public override ItemResource clone()
        {
            return new BlueprintScrapItemResource(new List<Vector2>(_points), _generalProperties.clone(), _blueprintScrapProperties.clone());
        }
    }
}
