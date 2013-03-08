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

        public Texture2D texture { get { return _texture; } set { _texture = value; } }
        public string textureUID { get { return _textureUID; } set { _textureUID = value; } }
        public Vector2 speedScale { get { return _speedScale; } set { _speedScale = value; } }
        public float layerDepth { get { return _layerDepth; } set { _layerDepth = value; } }
        public Vector2 initialOffset { get { return _initialOffset; } set { _initialOffset = value; } }

        public BackgroundLayer(XElement data)
        {
            _textureUID = Loader.loadString(data.Attribute("texture_uid"), "default");
            _speedScale = Loader.loadVector2(data.Attribute("speed_scale").Value, Vector2.Zero);
            _layerDepth = Loader.loadFloat(data.Attribute("layer_depth").Value, 0f);
            _initialOffset = Loader.loadVector2(data.Attribute("initial_offset").Value, Vector2.Zero);
        }

        public BackgroundLayer()
        {
            _texture = null;
            _textureUID = "default";
            _initialOffset = Vector2.Zero;
            _speedScale = Vector2.Zero;
            _layerDepth = 1f;
        }

        public void loadTexture()
        {
            if (_textureUID != null && _textureUID != "")
                _texture = ResourceController.getTexture(_textureUID);
        }
    }
}
