using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore.Resources;
using StasisEditor.Models;

namespace StasisEditor.Models
{
    public class EditorHealthPotion : EditorItem
    {
        private HealthPotionItemResource _healthPotionItemResource;

        [Browsable(false)]
        public HealthPotionItemResource healthPotionItemResource { get { return _healthPotionItemResource; } }

        [CategoryAttribute("Potion Properties")]
        [DisplayName("Strength")]
        public int strength { get { return _healthPotionItemResource.strength; } set { _healthPotionItemResource.strength = value; } }

        public EditorHealthPotion(ItemResource resource)
            : base(resource)
        {
            _healthPotionItemResource = resource as HealthPotionItemResource;
        }

        // toXML
        public override XElement toXML()
        {
            return _healthPotionItemResource.toXML();
        }
    }
}
