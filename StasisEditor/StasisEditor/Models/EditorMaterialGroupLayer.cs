using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Models
{
    using UITypeEditor = System.Drawing.Design.UITypeEditor;

    public class EditorMaterialGroupLayer : MaterialGroupLayer
    {
        [EditorAttribute(typeof(XNAColorEditor), typeof(UITypeEditor))]
        public override Color baseColor { get { return base.baseColor; } set { base.baseColor = value; } }

        [Browsable(false)]
        public override string type { get { return base.type; } set { base.type = value; } }

        [Browsable(false)]
        public override bool enabled { get { return base.enabled; } set { base.enabled = value; } }

        [Browsable(false)]
        public override XElement data { get { return base.data; } }

        [Browsable(false)]
        public override List<MaterialLayer> layers { get { return base.layers; } }

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
