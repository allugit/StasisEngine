using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorBackground : Background
    {
        private string _uid;

        public string uid { get { return _uid; } set { _uid = value; } }
        public XElement data
        {
            get
            {
                List<XElement> layerData = new List<XElement>();
                foreach (EditorBackgroundLayer layer in _layers)
                    layerData.Add(layer.data);
                return new XElement("Background",
                    new XAttribute("uid", _uid),
                    layerData);
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
    }
}
