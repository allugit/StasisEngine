using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class Material
    {
        protected string _uid;
        protected MaterialGroupLayer _rootLayer;

        public string uid { get { return _uid; } set { _uid = value; } }
        virtual public MaterialGroupLayer rootLayer { get { return _rootLayer; } }

        virtual public XElement data
        {
            get
            {
                return new XElement("Material",
                    new XAttribute("uid", _uid),
                    _rootLayer.data);
            }
        }

        // Create new
        public Material(string uid)
        {
            _uid = uid;
            createRootLayer();
        }

        virtual public void createRootLayer()
        {
            _rootLayer = new MaterialGroupLayer("root", true);
        }

        // Create from xml
        public Material(XElement data)
        {
            _uid = data.Attribute("uid").Value;
            loadRootLayer(data);
        }

        // Load root layer
        virtual protected void loadRootLayer(XElement data)
        {
            _rootLayer = MaterialLayer.load(data.Element("Layer")) as MaterialGroupLayer;
        }
    }
}
