using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.Components
{
    public class WorldItemRenderComponent : IComponent
    {
        private Texture2D _worldTexture;

        public ComponentType componentType { get { return ComponentType.WorldItemRender; } }
        public Texture2D worldTexture { get { return _worldTexture; } }

        public WorldItemRenderComponent(Texture2D worldTexture)
        {
            _worldTexture = worldTexture;
        }
    }
}
