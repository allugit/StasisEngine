using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Models
{
    using UITypeEditor = System.Drawing.Design.UITypeEditor;

    public class EditorMaterialTextureLayer : MaterialTextureLayer
    {
        [EditorAttribute(typeof(XNAColorEditor), typeof(UITypeEditor))]
        public override Color baseColor { get { return base.baseColor; } set { base.baseColor = value; } }

        [Browsable(false)]
        public override string type { get { return base.type; } set { base.type = value; } }

        [Browsable(false)]
        public override bool enabled { get { return base.enabled; } set { base.enabled = value; } }

        [Browsable(false)]
        public override XElement data { get { return base.data; } }

        public EditorMaterialTextureLayer()
            : base()
        {
        }

        public EditorMaterialTextureLayer(XElement data)
            : base(data)
        {
        }

        public override MaterialLayer clone()
        {
            return new EditorMaterialTextureLayer(data);
        }
    }
}
