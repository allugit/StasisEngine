﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
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
        private CameraSystem _cameraSystem;
        private MaterialRenderer _materialRenderer;
        private float _scale = Settings.BASE_SCALE;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private Texture2D _circle;
        private Effect _primitivesEffect;
        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;
        private Vector2 _halfScreen;
        private RenderablePrimitiveNode _headNode;
        private Texture2D _reticle;
        private bool _paused;
        private bool _singleStep;

        private BackgroundRenderer _backgroundRenderer;
        private RenderTarget2D _fluidRenderTarget;
        private RenderTarget2D _renderedFluid;
        private Texture2D _fluidParticleTexture;
        private Effect _fluidEffect;

        public int defaultPriority { get { return 85; } }
        public SystemType systemType { get { return SystemType.Render; } }
        public MaterialRenderer materialRenderer { get { return _materialRenderer; } }
        public float scale { get { return _scale; } }
        public int screenWidth { get { return _graphicsDevice.Viewport.Width; } }
        public int screenHeight { get { return _graphicsDevice.Viewport.Height; } }
        public Vector2 halfScreen { get { return _halfScreen; } }
        public Vector2 screenCenter { get { return _cameraSystem.screenCenter; } }
        public SpriteBatch spriteBatch { get { return _spriteBatch; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public Texture2D pixel { get { return _pixel; } }

        public RenderSystem(LoderGame game, SystemManager systemManager, EntityManager entityManager)
        {
            _game = game;
            _systemManager = systemManager;
            _entityManager = entityManager;
            //_sortedRenderablePrimitives = new SortedDictionary<float, List<IRenderablePrimitive>>();
            _cameraSystem = _systemManager.getSystem(SystemType.Camera) as CameraSystem;
            _graphicsDevice = game.GraphicsDevice;
            _spriteBatch = game.spriteBatch;
            _backgroundRenderer = new BackgroundRenderer(_spriteBatch);
            _fluidRenderTarget = new RenderTarget2D(_graphicsDevice, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
            _renderedFluid = new RenderTarget2D(_graphicsDevice, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);

            _contentManager = new ContentManager(game.Services);
            _contentManager.RootDirectory = "Content";
            _fluidEffect = _contentManager.Load<Effect>("fluid_effect");
            _fluidParticleTexture = _contentManager.Load<Texture2D>("fluid_particle");
            _reticle = _contentManager.Load<Texture2D>("reticle");
            _materialRenderer = new MaterialRenderer(game.GraphicsDevice, _contentManager, game.spriteBatch);
            _primitivesEffect = _contentManager.Load<Effect>("effects/primitives");
            _pixel = new Texture2D(_graphicsDevice, 1, 1);
            _pixel.SetData<Color>(new [] { Color.White });
            _circle = _contentManager.Load<Texture2D>("circle");
        }

        ~RenderSystem()
        {
            _contentManager.Unload();
        }

        public void setBackground(Background background)
        {
            _backgroundRenderer.background = background;
        }

        public void addRenderablePrimitive(IRenderablePrimitive renderablePrimitive)
        {
            RenderablePrimitiveNode node = new RenderablePrimitiveNode(renderablePrimitive);

            if (_headNode == null)
            {
                // This is the first node, so make it the head
                _headNode = node;
            }
            else if (_headNode.renderablePrimitive.layerDepth < node.renderablePrimitive.layerDepth)
            {
                // The head node has a layer depth lower than the node being added, so move it down the list
                node.next = _headNode;
                _headNode = node;
            }
            else
            {
                RenderablePrimitiveNode current = _headNode;

                while (current != null)
                {
                    if (current.next == null)
                    {
                        current.next = node;
                        return;
                    }
                    else if (current.next.renderablePrimitive.layerDepth <= node.renderablePrimitive.layerDepth)
                    {
                        node.next = current.next;
                        current.next = node;
                        return;
                    }
                    current = current.next;
                }
            }
        }

        public void drawRenderablePrimitives()
        {
            RenderablePrimitiveNode current = _headNode;

            while (current != null)
            {
                _graphicsDevice.Textures[0] = current.renderablePrimitive.texture;
                _primitivesEffect.Parameters["world"].SetValue(current.renderablePrimitive.worldMatrix);
                _primitivesEffect.CurrentTechnique.Passes["textured_primitives"].Apply();
                _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, current.renderablePrimitive.vertices, 0, current.renderablePrimitive.primitiveCount, CustomVertexFormat.VertexDeclaration);
                //_graphicsDevice.DrawUserPrimitives<CustomVertexFormat>(PrimitiveType.TriangleList, current.renderablePrimitive.vertices, 0, current.renderablePrimitive.primitiveCount);

                current = current.next;
            }

            _headNode = null;
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
            List<int> treeEntities = _entityManager.getEntitiesPosessing(ComponentType.Tree);
            List<int> aimEntities = _entityManager.getEntitiesPosessing(ComponentType.Aim);
            List<int> explosionEntities = _entityManager.getEntitiesPosessing(ComponentType.Explosion);
            List<RopeGrabComponent> ropeGrabComponents = _entityManager.getComponents<RopeGrabComponent>(ComponentType.RopeGrab);
            Vector2 screenCenter = _cameraSystem.screenCenter;

            // Pre render fluid
            if (fluidSystem != null)
            {
                // Draw on render target
                _graphicsDevice.SetRenderTarget(_fluidRenderTarget);
                _graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin();
                int limit = fluidSystem.numActiveParticles;
                for (int i = 0; i < limit; i++)
                {
                    // Current particle
                    Particle particle = fluidSystem.liquid[fluidSystem.activeParticles[i]];
                    Color color = new Color(1, particle.velocity.X < 0 ? -particle.velocity.X : particle.velocity.X, particle.velocity.Y < 0 ? -particle.velocity.Y : particle.velocity.Y);
                    spriteBatch.Draw(_fluidParticleTexture, (particle.position - _cameraSystem.screenCenter) * scale + _halfScreen, _fluidParticleTexture.Bounds, color, 0, new Vector2(12, 12), 1, SpriteEffects.None, 0);
                }
                spriteBatch.End();
                _graphicsDevice.SetRenderTarget(_renderedFluid);
                _graphicsDevice.Clear(Color.Transparent);

                // Draw post-processed render target to screen
                _fluidEffect.Parameters["renderSize"].SetValue(new Vector2(_renderedFluid.Width, _renderedFluid.Height));
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, _fluidEffect);
                spriteBatch.Draw(_fluidRenderTarget, Vector2.Zero, Color.DarkBlue);
                spriteBatch.End();
                _graphicsDevice.SetRenderTarget(null);
            }

            // Draw background
            if (_backgroundRenderer.background != null)
            {
                _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                _backgroundRenderer.draw(-screenCenter);
                _spriteBatch.End();
            }

            // Draw fluid
            if (fluidSystem != null)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_renderedFluid, _renderedFluid.Bounds, Color.White);
                _spriteBatch.End();
            }

            // Begin ordered drawing
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            _halfScreen = new Vector2(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height) / 2;
            _viewMatrix = Matrix.CreateTranslation(new Vector3(-screenCenter, 0)) * Matrix.CreateScale(new Vector3(_scale, -_scale, 1f));
            _projectionMatrix = Matrix.CreateOrthographic(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, 0, 1);
            _primitivesEffect.Parameters["view"].SetValue(_viewMatrix);
            _primitivesEffect.Parameters["projection"].SetValue(_projectionMatrix);

            // Body render components
            for (int i = 0; i < bodyRenderEntities.Count; i++)
            {
                int entityId = bodyRenderEntities[i];
                BodyRenderComponent bodyRenderComponent = (BodyRenderComponent)_entityManager.getComponent(entityId, ComponentType.BodyRender);
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(entityId, ComponentType.Physics);

                // Update world matrix
                bodyRenderComponent.worldMatrix = Matrix.CreateRotationZ(physicsComponent.body.Rotation) * Matrix.CreateTranslation(new Vector3(physicsComponent.body.Position, 0));

                // Update body's collection of vertices
                int index = 0;
                for (int j = 0; j < bodyRenderComponent.renderableTriangles.Count; j++)
                {
                    bodyRenderComponent.vertices[index++] = bodyRenderComponent.renderableTriangles[j].vertices[0];
                    bodyRenderComponent.vertices[index++] = bodyRenderComponent.renderableTriangles[j].vertices[1];
                    bodyRenderComponent.vertices[index++] = bodyRenderComponent.renderableTriangles[j].vertices[2];
                }

                addRenderablePrimitive(bodyRenderComponent);
            }

            // Rope rendering
            for (int i = 0; i < ropeRenderEntities.Count; i++)
            {
                int entityId = ropeRenderEntities[i];
                RopePhysicsComponent ropePhysicsComponent = (RopePhysicsComponent)_entityManager.getComponent(entityId, ComponentType.RopePhysics);

                for (int j = 0; j < ropePhysicsComponent.segmentHeads.Count; j++)
                {
                    RopeNode current = ropePhysicsComponent.segmentHeads[j];
                    RopeNode head = current;
                    RopeNode tail = head.tail;
                    //Color color = Color.Red;
                    while (current != null)
                    {
                        /*
                        color = Color.Red;
                        if (current == head)
                            color = Color.White;
                        else if (current == tail)
                            color = Color.Black;
                        */

                        _spriteBatch.Draw(_pixel, (current.body.Position - screenCenter) * _scale + _halfScreen, new Rectangle(0, 0, 16, 4), Color.Tan, current.body.Rotation, new Vector2(8, 2), 1f, SpriteEffects.None, 0.1f);
                        current = current.next;
                    }
                }
            }

            for (int i = 0; i < worldItemRenderEntities.Count; i++)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(worldItemRenderEntities[i], ComponentType.Physics);
                WorldItemRenderComponent renderComponent = (WorldItemRenderComponent)_entityManager.getComponent(worldItemRenderEntities[i], ComponentType.WorldItemRender);

                _spriteBatch.Draw(renderComponent.worldTexture, (physicsComponent.body.Position - screenCenter) * _scale + _halfScreen, renderComponent.worldTexture.Bounds, Color.White, physicsComponent.body.Rotation, new Vector2(renderComponent.worldTexture.Width, renderComponent.worldTexture.Height) / 2f, 1f, SpriteEffects.None, 0);
            }

            for (int i = 0; i < characterRenderEntities.Count; i++)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(characterRenderEntities[i], ComponentType.Physics);
                PolygonShape bodyShape = physicsComponent.body.FixtureList[0].Shape as PolygonShape;
                float shapeWidth = bodyShape.Vertices[2].X - bodyShape.Vertices[0].X;
                float shapeHeight = bodyShape.Vertices[3].Y - bodyShape.Vertices[0].Y;
                Rectangle source = new Rectangle(0, 0, (int)(shapeWidth * _scale), (int)(shapeHeight * _scale));
                Vector2 origin = new Vector2(source.Width / 2f, source.Height / 2f);

                _spriteBatch.Draw(_pixel, (physicsComponent.body.Position - screenCenter) * _scale + _halfScreen, source, Color.White, 0, origin, 1f, SpriteEffects.None, 0.1f);
            }

            for (int i = 0; i < characterMovementEntities.Count; i++)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(characterMovementEntities[i], ComponentType.Physics);
                CharacterMovementComponent characterMovementComponent = (CharacterMovementComponent)_entityManager.getComponent(characterMovementEntities[i], ComponentType.CharacterMovement);
                Vector2 movementNormal = characterMovementComponent.movementNormal;
                Rectangle source = new Rectangle(0, 0, (int)(movementNormal.Length() * _scale), 2);
                float angle = characterMovementComponent.movementAngle;

                _spriteBatch.Draw(_pixel, (physicsComponent.body.Position - screenCenter) * _scale + _halfScreen, source, Color.Yellow, angle, new Vector2(0, 1), 1f, SpriteEffects.None, 0);
            }

            // Tree
            _primitivesEffect.Parameters["world"].SetValue(Matrix.Identity);
            for (int i = 0; i < treeEntities.Count; i++)
            {
                TreeComponent treeComponent = _entityManager.getComponent(treeEntities[i], ComponentType.Tree) as TreeComponent;

                if (treeComponent.tree.active)
                {
                    addRenderablePrimitive(treeComponent.tree);
                    treeComponent.tree.rootMetamer.draw(this);
                }
            }

            drawRenderablePrimitives();

            for (int i = 0; i < aimEntities.Count; i++)
            {
                AimComponent aimComponent = (AimComponent)_entityManager.getComponent(aimEntities[i], ComponentType.Aim);
                Vector2 worldPosition = (_entityManager.getComponent(aimEntities[i], ComponentType.WorldPosition) as WorldPositionComponent).position;
                float length = aimComponent.length;

                _spriteBatch.Draw(_reticle, (worldPosition - screenCenter + new Vector2((float)Math.Cos(aimComponent.angle), (float)Math.Sin(aimComponent.angle)) * length) * _scale + _halfScreen, _reticle.Bounds, Color.Red, aimComponent.angle, new Vector2(_reticle.Width, _reticle.Height) / 2f, 1f, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < ropeGrabComponents.Count; i++)
            {
                foreach (KeyValuePair<Body, RevoluteJoint> pair in ropeGrabComponents[i].joints)
                {
                    Vector2 pointA = pair.Value.BodyA.Position;
                    Vector2 pointB = pair.Value.BodyB.Position;
                    Vector2 relative = pointB - pointA;
                    float angle = (float)Math.Atan2(relative.Y, relative.X);

                    _spriteBatch.Draw(_pixel, (pointA - screenCenter) * _scale + _halfScreen, new Rectangle(0, 0, (int)(relative.Length() * _scale), 2), Color.Green, angle, new Vector2(0, 1), 1f, SpriteEffects.None, 0f);
                }
            }

            // Draw explosions
            for (int i = 0; i < explosionEntities.Count; i++)
            {
                ExplosionComponent explosionComponent = (ExplosionComponent)_entityManager.getComponent(explosionEntities[i], ComponentType.Explosion);
                _spriteBatch.Draw(_circle, (explosionComponent.position - screenCenter) * _scale + _halfScreen, _circle.Bounds, Color.Red, 0f, new Vector2(_circle.Width, _circle.Height) / 2f, ((explosionComponent.radius * _scale) / (_circle.Width / 2f)), SpriteEffects.None, 0f);
            }

            _spriteBatch.End();

            // Particle debug
            if (LoderGame.debug)
            {
                _spriteBatch.Begin();
                int limit = fluidSystem.numActiveParticles;
                for (int i = 0; i < limit; i++)
                {
                    // Current particle
                    Particle particle = fluidSystem.liquid[fluidSystem.activeParticles[i]];
                    //Color color = new Color(1, particle.velocity.X < 0 ? -particle.velocity.X : particle.velocity.X, particle.velocity.Y < 0 ? -particle.velocity.Y : particle.velocity.Y);
                    spriteBatch.Draw(_pixel, (particle.position - _cameraSystem.screenCenter) * scale + _halfScreen, new Rectangle(0, 0, 2, 2), Color.White, 0, new Vector2(1, 1), 1, SpriteEffects.None, 0);
                }
                _spriteBatch.End();
            }
        }
    }
}
