using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class BlueprintScrapItemResource : ItemResource
    {
        private BlueprintScrapProperties _blueprintScrapProperties;
        private BlueprintItemResource _owner;

        public BlueprintScrapProperties blueprintScrapProperties { get { return _blueprintScrapProperties; } }
        public BlueprintItemResource owner { get { return _owner; } }

        public BlueprintScrapItemResource(ItemProperties generalProperties = null, ItemProperties blueprintScrapProperties = null)
            : base()
        {
            // Default general item properties
            if (generalProperties == null)
                generalProperties = new GeneralItemProperties("", 1, "", "");

            // Default blueprint scrap properties
            if (blueprintScrapProperties == null)
                blueprintScrapProperties = new BlueprintScrapProperties("", "");

            _generalProperties = generalProperties as GeneralItemProperties;
            _blueprintScrapProperties = blueprintScrapProperties as BlueprintScrapProperties;
            _type = ItemType.BlueprintScrap;
        }

        // clone
        public override ItemResource clone()
        {
            return new BlueprintScrapItemResource(_generalProperties.clone(), _blueprintScrapProperties.clone());
        }
    }
}
