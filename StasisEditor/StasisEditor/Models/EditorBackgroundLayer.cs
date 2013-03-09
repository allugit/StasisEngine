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
