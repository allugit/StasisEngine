using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace StasisCore.Models
{
    /*
     * Blueprint -- Consists of:
     * - Scraps: The scraps that have to be pieced together to create the
     * whole blueprint.
     * - Sockets: The information needed to create a connection between scraps
     */
    public class Blueprint
    {
        protected string _uid;
        private string _itemUID;
        private List<BlueprintScrap> _scraps;
        private List<BlueprintSocket> _sockets;
        private string _worldTextureUID;
        private string _inventoryTextureUID;

        public string uid { get { return _uid; } set { _uid = value; } }
        public string itemUID { get { return _itemUID; } }
        public List<BlueprintScrap> scraps { get { return _scraps; } }
        public List<BlueprintSocket> sockets { get { return _sockets; } }
        public string worldTextureUID { get { return _worldTextureUID; } set { _worldTextureUID = value; } }
        public string inventoryTextureUID { get { return _inventoryTextureUID; } set { _inventoryTextureUID = value; } }

        public XElement data
        {
            get
            {
                XElement d = new XElement("Blueprint",
                    new XAttribute("type", "Blueprint"),
                    new XAttribute("uid", _uid),
                    new XAttribute("world_texture_uid", _worldTextureUID),
                    new XAttribute("inventory_texture_uid", _inventoryTextureUID),
                    new XAttribute("item_uid", _itemUID));
                foreach (BlueprintScrap scrap in _scraps)
                    d.Add(scrap.data);
                foreach (BlueprintSocket socket in _sockets)
                    d.Add(socket.data);
                return d;
            }
        }

        // Create new
        public Blueprint(string uid)
        {
            _uid = uid;
            _itemUID = "";
            _inventoryTextureUID = "default_item";
            _worldTextureUID = "default_item";
            _scraps = new List<BlueprintScrap>();
            _sockets = new List<BlueprintSocket>();
        }

        // Create from xml
        public Blueprint(XElement data, List<BlueprintScrap> scraps, List<BlueprintSocket> sockets)
        {
            _uid = data.Attribute("uid").Value;
            _itemUID = data.Attribute("item_uid").Value;
            _inventoryTextureUID = Loader.loadString(data.Attribute("inventory_texture_uid"), "default_item");
            _worldTextureUID = Loader.loadString(data.Attribute("world_texture_uid"), "default_item");
            _scraps = scraps;
            _sockets = sockets;
        }

        // checkSockets
        private void checkSockets()
        {
            foreach (BlueprintSocket socket in _sockets)
                socket.test(10f);
        }
    }
}
