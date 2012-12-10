using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class HealthPotionItemResource : ItemResource
    {
        private HealthPotionProperties _healthPotionProperties;

        public HealthPotionProperties healthPotionProperties { get { return _healthPotionProperties; } }

        public HealthPotionItemResource(ItemProperties generalProperties = null, ItemProperties healthPotionProperties = null)
            : base()
        {
            // Default general properties
            if (generalProperties == null)
                generalProperties = new GeneralItemProperties("", 1, "", "");

            // Default health potion properties
            if (healthPotionProperties == null)
                healthPotionProperties = new HealthPotionProperties(20);

            _generalProperties = generalProperties as GeneralItemProperties;
            _healthPotionProperties = healthPotionProperties as HealthPotionProperties;
            _type = ItemType.HealthPotion;
        }

        // clone
        public override ItemResource clone()
        {
            return new HealthPotionItemResource(_generalProperties.clone(), _healthPotionProperties.clone());
        }
    }
}
