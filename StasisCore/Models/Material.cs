using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Resources;

namespace StasisCore.Models
{
    public class Material
    {
        private string _uid;
        private MaterialRootLayer _rootLayer;

        public string uid { get { return uid; } set { _uid = value; } }
        public MaterialRootLayer rootLayer { get { return _rootLayer; } }

        public Material(ResourceObject resource)
        {
            _uid = resource.uid;
            _rootLayer = MaterialLayer.load(resource.data.Element("Layer")) as MaterialRootLayer;
        }
    }
}
