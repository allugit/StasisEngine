﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Box2D.XNA;
using StasisCore;
using StasisCore.Models;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    using Settings = StasisCore.Settings;

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
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private Effect _primitivesEffect;
        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;
        private Vector2 _screenCenter;

        public int defaultPriority { get { return 90; } }
        public SystemType systemType { get { return SystemType.Render; } }
        public MaterialRenderer materialRenderer { get { return _materialRenderer; } }
        public Vector2 screenCenter { get { return _screenCenter; } }
        public float scale { get { return _scale; } }
        public int screenWidth { get { return _graphicsDevice.Viewport.Width; } }
        public int screenHeight { get { return _graphicsDevice.Viewport.Height; } }

        public RenderSystem(LoderGame game, SystemManager systemManager, EntityManager entityManager)
        {
            _game = game;
            _systemManager = systemManager;
            _entityManager = entityManager;
            _graphicsDevice = game.GraphicsDevice;
            _spriteBatch = game.spriteBatch;

            _contentManager = new ContentManager(game.Services, "Content");
            _coreContentManager = new ContentManager(game.Services, "StasisCoreContent");
            _materialRenderer = new MaterialRenderer(game.GraphicsDevice, _contentManager, game.spriteBatch);
            _primitivesEffect = _coreContentManager.Load<Effect>("effects\\primitives");
            _pixel = new Texture2D(_graphicsDevice, 1, 1);
            _pixel.SetData<Color>(new [] { Color.White });
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
            FluidSystem fluidSystem = (FluidSystem)_systemManager.getSystem(SystemType.Fluid);
            List<int> bodyRenderEntities = _entityManager.getEntitiesPosessing(ComponentType.BodyRender);
            List<int> ropeRenderEntities = _entityManager.getEntitiesPosessing(ComponentType.RopeRender);
            List<int> worldItemRenderEntities = _entityManager.getEntitiesPosessing(ComponentType.WorldItemRender);
            List<int> characterRenderEntities = _entityManager.getEntitiesPosessing(ComponentType.CharacterRender);
            List<int> characterMovementEntities = _entityManager.getEntitiesPosessing(ComponentType.CharacterMovement);

            _viewMatrix = Matrix.CreateScale(new Vector3(_scale, -_scale, 1f));
            _projectionMatrix = Matrix.CreateOrthographic(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, 0, 1);
            _primitivesEffect.Parameters["view"].SetValue(_viewMatrix);
            _primitivesEffect.Parameters["projection"].SetValue(_projectionMatrix);

            for (int i = 0; i < bodyRenderEntities.Count; i++)
            {
                int entityId = bodyRenderEntities[i];
                BodyRenderComponent bodyRenderComponent = (BodyRenderComponent)_entityManager.getComponent(entityId, ComponentType.BodyRender);
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(entityId, ComponentType.Physics);

                bodyRenderComponent.worldMatrix = Matrix.CreateRotationZ(physicsComponent.body.GetAngle()) * Matrix.CreateTranslation(new Vector3(physicsComponent.body.GetPosition(), 0));

                _graphicsDevice.Textures[0] = bodyRenderComponent.texture;
                _primitivesEffect.Parameters["world"].SetValue(bodyRenderComponent.worldMatrix);
                _primitivesEffect.CurrentTechnique.Passes["textured_primitives"].Apply();
                _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bodyRenderComponent.vertices, 0, bodyRenderComponent.primitiveCount, CustomVertexFormat.VertexDeclaration);
            }

            for (int i = 0; i < ropeRenderEntities.Count; i++)
            {
                int entityId = ropeRenderEntities[i];
                RopePhysicsComponent ropePhysicsComponent = (RopePhysicsComponent)_entityManager.getComponent(entityId, ComponentType.RopePhysics);
                RopeNode current = ropePhysicsComponent.head;
                while (current != null)
                {
                    _spriteBatch.Draw(_pixel, current.body.GetPosition() * _scale + new Vector2(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height) / 2, new Rectangle(0, 0, 16, 4), Color.Red, current.body.GetAngle(), new Vector2(8, 2), 1f, SpriteEffects.None, 0);
                    current = current.next;
                }
            }

            if (fluidSystem != null)
            {
                for (int i = 0; i < fluidSystem.numActiveParticles; i++)
                {
                    Particle particle = fluidSystem.liquid[fluidSystem.activeParticles[i]];
                    _spriteBatch.Draw(_pixel, particle.position * Settings.BASE_SCALE + new Vector2(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height) / 2, new Rectangle(0, 0, 4, 4), Color.Blue, 0, new Vector2(2, 2), 1f, SpriteEffects.None, 0);
                }
            }

            for (int i = 0; i < worldItemRenderEntities.Count; i++)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(worldItemRenderEntities[i], ComponentType.Physics);
                WorldItemRenderComponent renderComponent = (WorldItemRenderComponent)_entityManager.getComponent(worldItemRenderEntities[i], ComponentType.WorldItemRender);

                _spriteBatch.Draw(renderComponent.worldTexture, physicsComponent.body.GetPosition() * _scale + new Vector2(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height) / 2, renderComponent.worldTexture.Bounds, Color.White, physicsComponent.body.GetAngle(), new Vector2(renderComponent.worldTexture.Width, renderComponent.worldTexture.Height) / 2f, 1f, SpriteEffects.None, 0);
            }

            for (int i = 0; i < characterRenderEntities.Count; i++)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(characterRenderEntities[i], ComponentType.Physics);
                PolygonShape bodyShape = physicsComponent.body.GetFixtureList().GetNext().GetShape() as PolygonShape;
                float shapeWidth = bodyShape._vertices[2].X - bodyShape._vertices[0].X;
                float shapeHeight = bodyShape._vertices[3].Y - bodyShape._vertices[0].Y;
                Rectangle source = new Rectangle(0, 0, (int)(shapeWidth * _scale), (int)(shapeHeight * _scale));
                Vector2 origin = new Vector2(source.Width / 2f, source.Height / 2f);

                _spriteBatch.Draw(_pixel, physicsComponent.body.GetPosition() * _scale + new Vector2(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height) / 2, source, Color.White, 0, origin, 1f, SpriteEffects.None, 0);
            }

            for (int i = 0; i < characterMovementEntities.Count; i++)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(characterMovementEntities[i], ComponentType.Physics);
                CharacterMovementComponent characterMovementComponent = (CharacterMovementComponent)_entityManager.getComponent(characterMovementEntities[i], ComponentType.CharacterMovement);
                Vector2 movementNormal = characterMovementComponent.movementNormal;
                Rectangle source = new Rectangle(0, 0, (int)(movementNormal.Length() * _scale), 2);
                float angle = characterMovementComponent.movementAngle;

                _spriteBatch.Draw(_pixel, physicsComponent.body.GetPosition() * _scale + new Vector2(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height) / 2, source, Color.Yellow, angle, new Vector2(0, 1), 1f, SpriteEffects.None, 0);
            }
        }
    }
}
