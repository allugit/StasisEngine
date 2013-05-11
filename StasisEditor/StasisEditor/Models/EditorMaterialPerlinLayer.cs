using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Models
{
    using UITypeEditor = System.Drawing.Design.UITypeEditor;

    public class EditorMaterialPerlinLayer : MaterialPerlinLayer
    {
        [EditorAttribute(typeof(XNAColorEditor), typeof(UITypeEditor))]
        public override Color colorLow { get { return base.colorLow; } set { base.colorLow = value; } }

        [EditorAttribute(typeof(XNAColorEditor), typeof(UITypeEditor))]
        public override Color colorHigh { get { return base.colorHigh; } set { base.colorHigh = value; } }

        [Browsable(false)]
        public override string type { get { return base.type; } set { base.type = value; } }

        [Browsable(false)]
        public override bool enabled { get { return base.enabled; } set { base.enabled = value; } }

        [Browsable(false)]
        public override XElement data { get { return base.data; } }

        public EditorMaterialPerlinLayer()
            : base()
        {
        }

        public EditorMaterialPerlinLayer(XElement data)
            : base(data)
        {
        }

        public override MaterialLayer clone()
        {
            return new EditorMaterialPerlinLayer(data);
        }
    }
}
