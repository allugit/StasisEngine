using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisEditor.Models
{
    public class ItemProperties : ActorProperties
    {
        private string _itemUID;
        private int _quantity;

        public string itemUID { get { return _itemUID; } set { _itemUID = value; } }
        public int quantity { get { return _quantity; } set { _quantity = value; } }
        [Browsable(false)]
        public XAttribute[] data
        {
            get
            {
                return new XAttribute[]
                {
                    new XAttribute("item_uid", _itemUID),
                    new XAttribute("quantity", _quantity)
                };
            }
        }

        // Create new
        public ItemProperties(string itemUID, int quantity)
            : base()
        {
            _itemUID = itemUID;
            _quantity = quantity;
        }

        // Load from xml
        public ItemProperties(XElement data)
        {
            _itemUID = data.Attribute("item_uid").Value;
            _quantity = int.Parse(data.Attribute("quantity").Value);
        }

        // Clone
        public override ActorProperties clone()
        {
            return new ItemProperties(_itemUID, _quantity);
        }
    }
}
