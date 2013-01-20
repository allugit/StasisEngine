using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorMaterialUniformScatterLayer : MaterialUniformScatterLayer
    {
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(System.Drawing.Design.UITypeEditor))]
        public override List<string> textureUIDs { get { return base.textureUIDs; } }

        public EditorMaterialUniformScatterLayer(float horizontalSpacing = 32f, float verticalSpacing = 32f, float jitter = 8f)
            : base(horizontalSpacing, verticalSpacing, jitter)
        {
        }

        public EditorMaterialUniformScatterLayer(XElement data)
            : base(data)
        {
        }
    }
}
