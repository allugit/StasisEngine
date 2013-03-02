using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore
{
    public class BackgroundLayer
    {
        private Texture2D _texture;
        private Vector2 _speedScale;
        private float _layerDepth;
        private Vector2 _initialOffset;

        public Texture2D texture { get { return _texture; } }
        public Vector2 speedScale { get { return _speedScale; } }
        public float layerDepth { get { return _layerDepth; } }
        public Vector2 initialOffset { get { return _initialOffset; } set { _initialOffset = value; } }

        public BackgroundLayer(Texture2D texture, Vector2 initialOffset, Vector2 speedScale, float layerDepth)
        {
            _texture = texture;
            _initialOffset = initialOffset;
            _speedScale = speedScale;
            _layerDepth = layerDepth;
        }
    }
}
