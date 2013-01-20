using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorMaterialUniformScatterLayer : MaterialUniformScatterLayer
    {
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(UITypeEditor))]
        public override List<string> textureUIDs { get { return base.textureUIDs; } }

        [EditorAttribute(typeof(XNAColorEditor), typeof(UITypeEditor))]
        public override Color baseColor { get { return base.baseColor; } set { base.baseColor = value; } } 

        public EditorMaterialUniformScatterLayer(Color baseColor, float horizontalSpacing = 32f, float verticalSpacing = 32f, float jitter = 8f, int randomRed = 0, int randomGreen = 0, int randomBlue = 0, int randomAlpha = 0)
            : base(baseColor, horizontalSpacing, verticalSpacing, jitter, randomRed, randomGreen, randomBlue, randomAlpha)
        {
        }

        public EditorMaterialUniformScatterLayer(XElement data)
            : base(data)
        {
        }
    }
}
