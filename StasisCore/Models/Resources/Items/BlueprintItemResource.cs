using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class BlueprintItemResource : ItemResource
    {
        private BlueprintProperties _blueprintProperties;
        private List<BlueprintScrapItemResource> _blueprintScraps;

        public BlueprintProperties blueprintProperties { get { return _blueprintProperties; } }

        public BlueprintItemResource(ItemProperties generalProperties = null, ItemProperties blueprintProperties = null)
            : base()
        {
            // Default general item properties
            if (generalProperties == null)
                generalProperties = new GeneralItemProperties("", 1, "", "");

            // Default blueprint properties
            if (blueprintProperties == null)
                blueprintProperties = new BlueprintProperties("");

            _generalProperties = generalProperties as GeneralItemProperties;
            _blueprintProperties = blueprintProperties as BlueprintProperties;
            _type = ItemType.Blueprint;
        }

        // clone
        public override ItemResource clone()
        {
            return new BlueprintItemResource(_generalProperties.clone(), _blueprintProperties.clone());
        }
    }
}
