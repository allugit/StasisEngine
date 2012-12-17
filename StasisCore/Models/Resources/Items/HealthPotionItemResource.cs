using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class HealthPotionItemResource : ItemResource
    {
        private int _strength;

        public int strength { get { return _strength; } set { _strength = value; } }

        public HealthPotionItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag, int strength)
            : base(tag, quantity, worldTextureTag, inventoryTextureTag)
        {
            _strength = strength;
            _type = ItemType.HealthPotion;
        }

        // clone
        public override ItemResource clone()
        {
            return new HealthPotionItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _strength);
        }
    }
}
