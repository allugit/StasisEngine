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
        private LoderGame _game;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private MaterialRenderer _materialRenderer;
        private float _scale = Settings.BASE_SCALE;
        private ContentManager _contentManager;
        private ContentManager _coreContentManager;
        private GraphicsDevice _graphicsDevice;
        private Effect _primitivesEffect;
        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;

        public int defaultPriority { get { return 90; } }
        public SystemType systemType { get { return SystemType.Render; } }
        public MaterialRenderer materialRenderer { get { return _materialRenderer; } }

        public RenderSystem(LoderGame game, SystemManager systemManager, EntityManager entityManager)
        {
            _game = game;
            _systemManager = systemManager;
            _entityManager = entityManager;
            _graphicsDevice = game.GraphicsDevice;

            _contentManager = new ContentManager(game.Services, "Content");
            _coreContentManager = new ContentManager(game.Services, "StasisCoreContent");
            _materialRenderer = new MaterialRenderer(game.GraphicsDevice, _contentManager, game.spriteBatch);
            _primitivesEffect = _coreContentManager.Load<Effect>("effects\\primitives");
        }

        ~RenderSystem()
        {
            _contentManager.Unload();
            _coreContentManager.Unload();
        }

        public void update()
        {
        }

        public void draw()
        {
            _viewMatrix = Matrix.CreateScale(new Vector3(_scale, -_scale, 1f));
            _projectionMatrix = Matrix.CreateOrthographic(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, 0, 1);

            List<BodyRenderComponent> bodyRenderComponents = _entityManager.getComponents<BodyRenderComponent>(ComponentType.BodyRender);
            for (int i = 0; i < bodyRenderComponents.Count; i++)
            {
                _graphicsDevice.Textures[0] = bodyRenderComponents[i].texture;
                _primitivesEffect.Parameters["world"].SetValue(Matrix.Identity);
                _primitivesEffect.Parameters["view"].SetValue(_viewMatrix);
                _primitivesEffect.Parameters["projection"].SetValue(_projectionMatrix);
                _primitivesEffect.CurrentTechnique.Passes["textured_primitives"].Apply();
                _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bodyRenderComponents[i].vertices, 0, bodyRenderComponents[i].primitiveCount, CustomVertexFormat.VertexDeclaration);
            }
        }
    }
}
