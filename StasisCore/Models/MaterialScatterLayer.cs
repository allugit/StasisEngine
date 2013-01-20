using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Resources;

namespace StasisCore.Models
{
    abstract public class MaterialScatterLayer : MaterialLayer
    {
        protected List<string> _textureUIDs;

        virtual public List<string> textureUIDs { get { return _textureUIDs; } }

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                foreach (string textureUID in _textureUIDs)
                    d.Add(new XElement("Texture", new XAttribute("uid", textureUID)));

                return d;
            }
        }

        // Create new
        public MaterialScatterLayer(string type)
            : base(type, true)
        {
            _textureUIDs = new List<string>();
        }

        // Create from xml
        public MaterialScatterLayer(XElement data) : base(data)
        {
            _textureUIDs = new List<string>();
            foreach (XElement textureXml in data.Elements("Texture"))
                _textureUIDs.Add(textureXml.Attribute("uid").Value);
        }
    }
}
