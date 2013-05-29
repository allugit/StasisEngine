using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.Components
{
    public class DecalRenderComponent : IComponent
    {
        private Texture2D _texture;
        private Vector2 _position;
        private float _angle;
        private Vector2 _origin;
        private float _layerDepth;

        public ComponentType componentType { get { return ComponentType.DecalRender; } }
        public Texture2D texture { get { return _texture; } set { _texture = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public Vector2 origin { get { return _origin; } set { _origin = value; } }
        public float angle { get { return _angle; } set { _angle = value; } }
        public float layerDepth { get { return _layerDepth; } set { _layerDepth = value; } }

        public DecalRenderComponent(Texture2D texture, Vector2 position, Vector2 origin, float angle, float layerDepth)
        {
            _texture = texture;
            _position = position;
            _origin = origin;
            _angle = angle;
            _layerDepth = layerDepth;
        }
    }
}
