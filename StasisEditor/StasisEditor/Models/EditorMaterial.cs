using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorMaterial : Material
    {
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
    }
}
