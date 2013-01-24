using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore.Models;
using StasisCore.Controllers;

namespace StasisEditor.Models
{
    public class EditorMaterial : Material
    {
        [Browsable(false)]
        public override XElement data { get { return base.data; } }

        [Browsable(false)]
        public override MaterialGroupLayer rootLayer { get { return base.rootLayer; } }

        public EditorMaterial(string uid)
            : base(uid)
        {
        }

        public EditorMaterial(XElement data)
            : base(data)
        {
        }

        protected override void loadRootLayer(XElement data)
        {
            _rootLayer = EditorMaterialLayer.load(data.Element("Layer")) as EditorMaterialGroupLayer;
        }

        public override string ToString()
        {
            return _uid;
        }

        public EditorMaterial clone()
        {
            EditorMaterial material = new EditorMaterial(data);
            material.rootLayer.type = "root";
            return material;
        }
    }
}
