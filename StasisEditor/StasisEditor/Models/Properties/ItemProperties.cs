using System;
using System.Collections.Generic;

namespace StasisEditor.Models
{
    public class ItemProperties : ActorProperties
    {
        private string _itemUID;
        private int _quantity;

        public string itemUID { get { return _itemUID; } set { _itemUID = value; } }
        public int quantity { get { return _quantity; } set { _quantity = value; } }

        public ItemProperties(string itemUID, int quantity)
            : base()
        {
            _itemUID = itemUID;
            _quantity = quantity;
        }

        public override ActorProperties clone()
        {
            return new ItemProperties(_itemUID, _quantity);
        }
    }
}
