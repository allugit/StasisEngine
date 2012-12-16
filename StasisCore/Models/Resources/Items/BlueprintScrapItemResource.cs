using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class BlueprintScrapItemResource : ItemResource
    {
        private List<Vector2> _points;
        private List<BlueprintSocketResource> _sockets;
        private BlueprintScrapProperties _blueprintScrapProperties;
        private BlueprintScrapCraftingProperties _blueprintScrapCraftingProperties;

        public BlueprintScrapProperties blueprintScrapProperties { get { return _blueprintScrapProperties; } }
        public List<Vector2> points { get { return _points; } set { _points = value; } }
        public BlueprintScrapCraftingProperties blueprintScrapCraftingProperties { get { return _blueprintScrapCraftingProperties; } set { _blueprintScrapCraftingProperties = value; } }
        public List<BlueprintSocketResource> sockets { get { return _sockets; } }

        public BlueprintScrapItemResource(List<Vector2> points = null, List<BlueprintSocketResource> sockets = null, ItemProperties generalProperties = null, ItemProperties blueprintScrapProperties = null, ItemProperties blueprintScrapCraftingProperties = null)
            : base()
        {
            // Default points
            if (points == null)
                points = new List<Vector2>();

            // Default sockets
            if (sockets == null)
                sockets = new List<BlueprintSocketResource>();

            // Default general item properties
            if (generalProperties == null)
                generalProperties = new GeneralItemProperties("", 1, "", "");

            // Default blueprint scrap properties
            if (blueprintScrapProperties == null)
                blueprintScrapProperties = new BlueprintScrapProperties("", "");

            _points = points;
            _sockets = sockets;
            _generalProperties = generalProperties as GeneralItemProperties;
            _blueprintScrapProperties = blueprintScrapProperties as BlueprintScrapProperties;
            _blueprintScrapCraftingProperties = blueprintScrapCraftingProperties as BlueprintScrapCraftingProperties;
            _type = ItemType.BlueprintScrap;
        }

        // clone
        public override ItemResource clone()
        {
            return new BlueprintScrapItemResource(new List<Vector2>(_points), null, _generalProperties.clone(), _blueprintScrapProperties.clone(), _blueprintScrapCraftingProperties.clone());
        }
    }
}
