using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorMaterialGroupLayer : MaterialGroupLayer
    {
        public EditorMaterialGroupLayer()
            : base("group", true)
        {
        }

        public EditorMaterialGroupLayer(XElement data)
            : base(data)
        {
        }

        protected override void loadLayers(XElement data)
        {
            _layers = new List<MaterialLayer>();
            foreach (XElement layerXml in data.Elements("Layer"))
                _layers.Add(EditorMaterialLayer.load(layerXml) as MaterialLayer);
        }

        public override MaterialLayer clone()
        {
            EditorMaterialGroupLayer layer = new EditorMaterialGroupLayer(data);
            layer.type = "group";
            return layer;
        }
    }
}
