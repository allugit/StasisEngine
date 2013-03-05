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
        public XElement data
        {
            get
            {
                return new XElement("BackgroundLayer",
                    new XAttribute("texture_uid", _textureUID),
                    new XAttribute("initial_offset", _initialOffset),
                    new XAttribute("speed_scale", _speedScale),
                    new XAttribute("layer_depth", _layerDepth));
            }
        }

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
