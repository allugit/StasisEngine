using System;
using System.Collections.Generic;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Models
{
    public class EditorHealthPotion : EditorItem
    {
        private HealthPotionItemResource _healthPotionItemResource;

        public HealthPotionItemResource healthPotionItemResource { get { return _healthPotionItemResource; } }

        public EditorHealthPotion(ItemResource resource)
            : base(resource)
        {
            _healthPotionItemResource = resource as HealthPotionItemResource;
        }
    }
}
