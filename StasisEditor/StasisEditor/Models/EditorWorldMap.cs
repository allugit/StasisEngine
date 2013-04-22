using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorWorldMap : WorldMap
    {
        private string _uid;
        private string _textureUID;

        public string uid { get { return _uid; } set { _uid = value; } }
        public string textureUID { get { return _textureUID; } set { _textureUID = value; } }
        [Browsable(false)]
        public XElement data
        {
            get
            {
                XElement d = new XElement("WorldMap",
                    new XAttribute("uid", _uid),
                    new XAttribute("texture_uid", _textureUID));
                return d;
            }
        }

        public EditorWorldMap(string uid) : base()
        {
            _uid = uid;
            _textureUID = "world_map";
            _texture = ResourceManager.getTexture(Loader.loadString(data.Attribute("texture_uid"), "world_map"));
        }

        public EditorWorldMap(XElement data) : base(data)
        {
            _uid = Loader.loadString(data.Attribute("uid"), "");
            _textureUID = Loader.loadString(data.Attribute("texture_uid"), "world_map");
        }

        public override string ToString()
        {
            return _uid;
        }
    }
}
