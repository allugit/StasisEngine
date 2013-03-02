using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorBackgroundLayer
    {
        private string _textureUID;
        private Vector2 _initialOffset;
        private Vector2 _speedScale;
        private float _layerDepth;

        public string textureUID { get { return _textureUID; } set { _textureUID = value; } }
        public Vector2 speedScale { get { return _speedScale; } set { _speedScale = value; } }
        public Vector2 initialOffset { get { return _initialOffset; } set { _initialOffset = value; } }
        public float layerDepth { get { return _layerDepth; } set { _layerDepth = value; } }
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

        public EditorBackgroundLayer()
        {
            _textureUID = "default";
            _layerDepth = 1f;
        }

        public EditorBackgroundLayer(XElement data)
        {
            _textureUID = Loader.loadString(data.Attribute("texture_uid"), "default");
            _initialOffset = Loader.loadVector2(data.Attribute("initial_offset"), Vector2.Zero);
            _speedScale = Loader.loadVector2(data.Attribute("speed_scale"), Vector2.Zero);
            _layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 1f);
        }
    }
}
