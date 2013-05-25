using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Models
{
    public class RopeMaterialTexture
    {
        private Texture2D _texture;
        private Vector2 _center;
        private float _angleOffset;

        public Texture2D texture { get { return _texture; } }
        public Vector2 center { get { return _center; } }
        public float angleOffset { get { return _angleOffset; } }

        public RopeMaterialTexture(XElement data)
        {
            _texture = ResourceManager.getTexture(data.Attribute("uid").Value);
            _center = Loader.loadVector2(data.Attribute("center"), new Vector2(_texture.Width, _texture.Height) / 2f);
            _angleOffset = Loader.loadFloat(data.Attribute("angle_offset"), 0f);
        }
    }
}
