using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    abstract public class Item
    {
        protected string _uid;
        protected string _worldTextureUID;
        protected string _inventoryTextureUID;
        protected int _quantity;

        public string uid { get { return _uid; } set { _uid = value; } }
        public string worldTextureUID { get { return _worldTextureUID; } set { _worldTextureUID = value; } }
        public string inventoryTextureUID { get { return _inventoryTextureUID; } set { _inventoryTextureUID = value; } }
        public int quantity { get { return _quantity; } set { _quantity = value; } }

        virtual public XElement data
        {
            get
            {
                return new XElement("Item",
                    new XAttribute("uid", _uid),
                    new XAttribute("world_texture_uid", _worldTextureUID),
                    new XAttribute("inventory_texture_uid", _inventoryTextureUID),
                    new XAttribute("quantity", _quantity));
            }
        }

        // Create new
        public Item(string uid)
        {
            _uid = uid;
            _worldTextureUID = "default_item_texture";
            _inventoryTextureUID = "default_item_texture";
            _quantity = 1;
        }

        // Create from xml
        public Item(XElement data)
        {
            _uid = data.Attribute("uid").Value;
            _worldTextureUID = data.Attribute("world_texture_uid").Value;
            _inventoryTextureUID = data.Attribute("inventory_texture_uid").Value;
            _quantity = int.Parse(data.Attribute("quantity").Value);
        }
    }
}
