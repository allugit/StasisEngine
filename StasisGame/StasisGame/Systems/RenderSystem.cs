using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using StasisCore;
using StasisCore.Models;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class RenderSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private MaterialRenderer _materialRenderer;
        private ContentManager _contentManager;

        public SystemType systemType { get { return SystemType.Material; } }
        public MaterialRenderer materialRenderer { get { return _materialRenderer; } }

        public RenderSystem(LoderGame game, SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;

            _contentManager = new ContentManager(game.Services, "Content");
            _materialRenderer = new MaterialRenderer(game.GraphicsDevice, _contentManager, game.spriteBatch);
        }

        ~RenderSystem()
        {
            _contentManager.Unload();
        }
    }
}
