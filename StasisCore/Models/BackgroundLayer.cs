using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        protected bool _tile = true;
        protected Vector2 _position;
        protected Color _baseColor;

        virtual public Texture2D texture { get { return _texture; } set { _texture = value; } }
        public string textureUID { get { return _textureUID; } set { _textureUID = value; } }
        virtual public Vector2 speedScale { get { return _speedScale; } set { _speedScale = value; } }
        public float layerDepth { get { return _layerDepth; } set { _layerDepth = value; } }
        virtual public Vector2 initialOffset { get { return _initialOffset; } set { _initialOffset = value; } }
        public bool fitToScreen { get { return _fitToScreen; } set { _fitToScreen = value; } }
        public float scale { get { return _scale; } set { _scale = value; } }
        public bool tile { get { return _tile; } set { _tile = value; } }
        virtual public Vector2 position { get { return _position; } set { _position = value; } }
        virtual public Color baseColor { get { return _baseColor; } set { _baseColor = value; } }
        virtual public XElement data
        {
            get
            {
                return new XElement("BackgroundLayer",
                    new XAttribute("texture_uid", _textureUID),
                    new XAttribute("initial_offset", _initialOffset),
                    new XAttribute("speed_scale", _speedScale),
                    new XAttribute("layer_depth", _layerDepth),
                    new XAttribute("fit_to_screen", _fitToScreen),
                    new XAttribute("tile", _tile),
                    new XAttribute("base_color", _baseColor));
            }
        }

        public BackgroundLayer(XElement data)
        {
            _textureUID = Loader.loadString(data.Attribute("texture_uid"), "default");
            _speedScale = Loader.loadVector2(data.Attribute("speed_scale"), Vector2.Zero);
            _layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0f);
            _initialOffset = Loader.loadVector2(data.Attribute("initial_offset"), Vector2.Zero);
            _fitToScreen = Loader.loadBool(data.Attribute("fit_to_screen"), false);
            _tile = Loader.loadBool(data.Attribute("tile"), true);
            _baseColor = Loader.loadColor(data.Attribute("base_color"), Color.White);
        }

        public BackgroundLayer()
        {
            _texture = null;
            _textureUID = "default";
            _initialOffset = Vector2.Zero;
            _speedScale = Vector2.Zero;
            _layerDepth = 1f;
            _fitToScreen = false;
            _tile = true;
            _baseColor = Color.White;
        }

        public void loadTexture()
        {
            if (_textureUID != null && _textureUID != "")
            {
                _texture = ResourceManager.getTexture(_textureUID);
                calculateScale();
            }
        }

        public void calculateScale()
        {
            if (_fitToScreen)
            {
                _scale = (float)ResourceManager.graphicsDevice.Viewport.Height / (float)texture.Height;

                if ((float)_texture.Width * _scale < (float)ResourceManager.graphicsDevice.Viewport.Width)
                    _scale = (float)ResourceManager.graphicsDevice.Viewport.Width / (float)texture.Width;
            }
        }
    }
}
