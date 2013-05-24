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

        public Texture2D texture { get { return _texture; } }
        public Vector2 center { get { return _center; } }

        public RopeMaterialTexture(XElement data)
        {
            _texture = ResourceManager.getTexture(data.Attribute("uid").Value);
            _center = Loader.loadVector2(data.Attribute("center"), new Vector2(_texture.Width, _texture.Height) / 2f);
        }
    }
}
