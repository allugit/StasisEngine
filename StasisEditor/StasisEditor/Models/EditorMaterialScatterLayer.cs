using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorMaterialScatterLayer : MaterialScatterLayer
    {
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(System.Drawing.Design.UITypeEditor))]
        public override List<string> textureUIDs { get { return base.textureUIDs; } }

        public EditorMaterialScatterLayer()
            : base()
        {
        }

        public EditorMaterialScatterLayer(XElement data)
            : base(data)
        {
        }
    }
}
