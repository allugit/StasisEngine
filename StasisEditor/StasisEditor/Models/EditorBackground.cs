using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorBackground : Background
    {
        private string _uid;

        public string uid { get { return _uid; } set { _uid = value; } }
        [Browsable(false)]
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("uid", _uid);
                return d;
            }
        }

        public EditorBackground(string uid) : base()
        {
            _uid = uid;
        }

        public EditorBackground(XElement data) : base(data)
        {
            _uid = Loader.loadString(data.Attribute("uid"), "");
        }

        public override void createLayers(XElement data)
        {
            foreach (XElement layerData in data.Elements("BackgroundLayer"))
                _layers.Add(new EditorBackgroundLayer(layerData));
        }

        public override string ToString()
        {
            return _uid;
        }

        override public Background clone()
        {
            EditorBackground copy = new EditorBackground(data);
            copy.loadTextures();

            return copy;
        }
    }
}
