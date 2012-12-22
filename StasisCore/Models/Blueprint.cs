﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class Blueprint : Item
    {
        private string _itemUID;
        private List<BlueprintScrap> _scraps;
        private List<BlueprintSocket> _sockets;

        public string itemUID { get { return _itemUID; } }
        public List<BlueprintScrap> scraps { get { return _scraps; } }
        public List<BlueprintSocket> sockets { get { return _sockets; } }

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
            _itemUID = data.Attribute("itemUID").Value;
            _scraps = scraps;
            _sockets = sockets;
        }
    }
}