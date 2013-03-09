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
    public class EditorBackgroundLayer : BackgroundLayer
    {
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
