﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace StasisCore.Models
{
    /*
     * Blueprint -- an item that is used to craft other items. It consists of:
     * - Scraps: The scraps that have to be pieced together to create the
     * whole blueprint.
     * - Sockets: The information needed to create a connection between scraps
     */
    public class Blueprint : Item
    {
        private string _itemUID;
        private List<BlueprintScrap> _scraps;
        private List<BlueprintSocket> _sockets;

        public string itemUID { get { return _itemUID; } }
        public List<BlueprintScrap> scraps { get { return _scraps; } }
        public List<BlueprintSocket> sockets { get { return _sockets; } }

        override public XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("type", "blueprint");
                d.SetAttributeValue("item_uid", _itemUID);
                foreach (BlueprintScrap scrap in _scraps)
                    d.Add(scrap.data);
                foreach (BlueprintSocket socket in _sockets)
                    d.Add(socket.data);

                return d;
            }
        }

        // Create new
        public Blueprint(string uid) : base(uid)
        {
            _itemUID = "";
            _scraps = new List<BlueprintScrap>();
            _sockets = new List<BlueprintSocket>();
        }

        // Create from xml
        public Blueprint(XElement data, List<BlueprintScrap> scraps, List<BlueprintSocket> sockets) : base(data)
        {
            _itemUID = data.Attribute("item_uid").Value;
            _scraps = scraps;
            _sockets = sockets;
        }

        // checkSockets
        private void checkSockets()
        {
            foreach (BlueprintSocket socket in _sockets)
                socket.test();
        }
    }
}
