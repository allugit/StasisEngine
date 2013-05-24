using System;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.Components
{
    public class RopeRenderComponent : IComponent
    {
        private Texture2D _texture;

        public ComponentType componentType { get { return ComponentType.RopeRender; } }
        public Texture2D texture { get { return _texture; } set { _texture = value; } }

        public RopeRenderComponent(Texture2D texture)
        {
            _texture = texture;
        }
    }
}
