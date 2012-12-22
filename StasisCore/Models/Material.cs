using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Resources;

namespace StasisCore.Models
{
    public class Material
    {
        private string _uid;
        private MaterialGroupLayer _rootLayer;

        public string uid { get { return _uid; } set { _uid = value; } }
        public MaterialGroupLayer rootLayer { get { return _rootLayer; } }

        // Create new
        public Material(string uid)
        {
            _uid = uid;
            _rootLayer = new MaterialGroupLayer("root");
        }

        // Create from xml
        public Material(ResourceObject resource)
        {
            _uid = resource.uid;
            _rootLayer = MaterialLayer.load(resource.data.Element("Layer")) as MaterialGroupLayer;
        }
    }
}
