using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    using UITypeEditor = System.Drawing.Design.UITypeEditor;

    public class EditorMaterialLeavesLayer : MaterialLeavesLayer
    {
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(UITypeEditor))]
        public override List<string> textureUIDs { get { return base.textureUIDs; } }

        [Browsable(false)]
        public override string type { get { return base.type; } set { base.type = value; } }

        [Browsable(false)]
        public override bool enabled { get { return base.enabled; } set { base.enabled = value; } }

        [Browsable(false)]
        public override XElement data { get { return base.data; } }

        public EditorMaterialLeavesLayer()
            : base()
        {
        }

        public EditorMaterialLeavesLayer(XElement data)
            : base(data)
        {
        }

        public override MaterialLayer clone()
        {
            return new EditorMaterialLeavesLayer(data);
        }
    }
}
