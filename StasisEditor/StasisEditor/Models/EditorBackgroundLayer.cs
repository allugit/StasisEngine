using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor.Models
{
    using UITypeEditor = System.Drawing.Design.UITypeEditor;

    public class EditorBackgroundLayer : BackgroundLayer
    {
        [EditorAttribute(typeof(Vector2Editor), typeof(UITypeEditor))]
        public override Vector2 initialOffset { get { return base.initialOffset; } set { base.initialOffset = value; } }

        [EditorAttribute(typeof(Vector2Editor), typeof(UITypeEditor))]
        public override Vector2 speedScale { get { return base.speedScale; } set { base.speedScale = value; } }

        [Browsable(false)]
        public override XElement data { get { return base.data; } }
        
        [Browsable(false)]
        public override Texture2D texture { get { return base.texture; } set { base.texture = value; } }

        public EditorBackgroundLayer() : base()
        {
        }

        public EditorBackgroundLayer(XElement data) : base(data)
        {
        }

        public override string ToString()
        {
            return _textureUID;
        }
    }
}
