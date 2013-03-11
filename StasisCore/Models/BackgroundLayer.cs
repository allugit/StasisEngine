using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Controllers;

namespace StasisCore.Models
{
    public class BackgroundLayer
    {
        protected Texture2D _texture;
        protected string _textureUID;
        protected Vector2 _speedScale;
        protected float _layerDepth;
        protected Vector2 _initialOffset;
        protected bool _fitToScreen;
        protected float _scale = 1f;

        virtual public Texture2D texture { get { return _texture; } set { _texture = value; } }
        public string textureUID { get { return _textureUID; } set { _textureUID = value; } }
        public Vector2 speedScale { get { return _speedScale; } set { _speedScale = value; } }
        public float layerDepth { get { return _layerDepth; } set { _layerDepth = value; } }
        public Vector2 initialOffset { get { return _initialOffset; } set { _initialOffset = value; } }
        public bool fitToScreen { get { return _fitToScreen; } set { _fitToScreen = value; } }
        public float scale { get { return _scale; } set { _scale = value; } }
        virtual public XElement data
        {
            get
            {
                return new XElement("BackgroundLayer",
                    new XAttribute("texture_uid", _textureUID),
                    new XAttribute("initial_offset", _initialOffset),
                    new XAttribute("speed_scale", _speedScale),
                    new XAttribute("layer_depth", _layerDepth),
                    new XAttribute("fit_to_screen", _fitToScreen));
            }
        }

        public BackgroundLayer(XElement data)
        {
            _textureUID = Loader.loadString(data.Attribute("texture_uid"), "default");
            _speedScale = Loader.loadVector2(data.Attribute("speed_scale"), Vector2.Zero);
            _layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0f);
            _initialOffset = Loader.loadVector2(data.Attribute("initial_offset"), Vector2.Zero);
            _fitToScreen = Loader.loadBool(data.Attribute("fit_to_screen"), false);
        }

        public BackgroundLayer()
        {
            _texture = null;
            _textureUID = "default";
            _initialOffset = Vector2.Zero;
            _speedScale = Vector2.Zero;
            _layerDepth = 1f;
            _fitToScreen = false;
        }

        public void loadTexture()
        {
            if (_textureUID != null && _textureUID != "")
            {
                _texture = ResourceController.getTexture(_textureUID);

                if (fitToScreen)
                {
                    int screenWidth = ResourceController.graphicsDevice.Viewport.Width;
                    int screenHeight = ResourceController.graphicsDevice.Viewport.Height;
                    int widthDifference = texture.Width - screenWidth;
                    int heightDifference = texture.Height - screenHeight;

                    if (Math.Abs(widthDifference) < Math.Abs(heightDifference))
                    {
                        scale = (float)screenWidth / (float)texture.Width;
                    }
                    else
                    {
                        scale = (float)screenHeight / (float)texture.Height;
                    }
                }
            }
        }
    }
}
