using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorBackground
    {
        private string _uid;
        private List<EditorBackgroundLayer> _layers;

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

        public EditorBackground(string uid)
        {
            _uid = uid;
            _layers = new List<EditorBackgroundLayer>();
        }

        public EditorBackground(XElement data)
        {
            _layers = new List<EditorBackgroundLayer>();
            _uid = Loader.loadString(data.Attribute("uid"), "");

            foreach (XElement layerData in data.Elements("BackgroundLayer"))
                _layers.Add(new EditorBackgroundLayer(layerData));
        }

        public override string ToString()
        {
            return _uid;
        }
    }
}
