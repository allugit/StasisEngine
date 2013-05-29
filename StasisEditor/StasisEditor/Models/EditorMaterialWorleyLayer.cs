using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Models
{
    using UITypeEditor = System.Drawing.Design.UITypeEditor;

    public class EditorMaterialWorleyLayer : MaterialWorleyLayer
    {
        [EditorAttribute(typeof(Vector2Editor), typeof(UITypeEditor))]
        public override Vector2 position { get { return base.position; } set { base.position = value; } }

        [EditorAttribute(typeof(Vector2Editor), typeof(UITypeEditor))]
        public override Vector2 fbmOffset { get { return base.fbmOffset; } set { base.fbmOffset = value; } }

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

        public EditorMaterialWorleyLayer()
            : base()
        {
        }

        public EditorMaterialWorleyLayer(XElement data)
            : base(data)
        {
        }

        public override MaterialLayer clone()
        {
            return new EditorMaterialWorleyLayer(data);
        }
    }
}
