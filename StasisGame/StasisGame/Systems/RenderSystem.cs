using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
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
        private AnimationManager _animationManager;
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
        private SpriteFont _tooltipFont;
        private bool _paused;
        private bool _singleStep;
        private bool _enlargeDebugFuild;

        private BackgroundRenderer _backgroundRenderer;
        private RenderTarget2D _fluidRenderTarget;
        private RenderTarget2D _renderedFluid;
        private RenderTarget2D _debugFluid;
        private RenderTarget2D _postSourceUnder;
        private RenderTarget2D _postSourceOver;
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
            _animationManager = game.animationManager;
            //_sortedRenderablePrimitives = new SortedDictionary<float, List<IRenderablePrimitive>>();
            _cameraSystem = _systemManager.getSystem(SystemType.Camera) as CameraSystem;
            _graphicsDevice = game.GraphicsDevice;
            _spriteBatch = game.spriteBatch;
            _backgroundRenderer = new BackgroundRenderer(_spriteBatch);
            _fluidRenderTarget = new RenderTarget2D(_graphicsDevice, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
            _renderedFluid = new RenderTarget2D(_graphicsDevice, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
            _debugFluid = new RenderTarget2D(_graphicsDevice, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
            _postSourceUnder = new RenderTarget2D(_graphicsDevice, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
            _postSourceOver = new RenderTarget2D(_graphicsDevice, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);

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
            _tooltipFont = _contentManager.Load<SpriteFont>("shared_ui/tooltip_font");
        }

        ~RenderSystem()
        {
            _contentManager.Unload();
        }

        // Set the background to render
        public void setBackground(Background background)
        {
            _backgroundRenderer.background = background;
        }

        // createSpriteRenderObject -- Creates a primitive render object that can be used in place of sprite batch
        public PrimitiveRenderObject createSpritePrimitiveObject(Texture2D texture, Vector2 position, Color color, Vector2 origin, float angle, float scale, float layerDepth)
        {
            // a-----------d
            // |         / |
            // |     /     |
            // | /         |
            // b-----------c
            float gameScale = Settings.BASE_SCALE;
            Vector3 a = Vector3.Zero;
            Vector3 b = new Vector3(0, (float)texture.Height / gameScale, 0);
            Vector3 c = new Vector3((float)texture.Width / gameScale, (float)texture.Height / gameScale, 0);
            Vector3 d = new Vector3((float)texture.Width / gameScale, 0, 0);
            List<RenderableTriangle> renderableTriangles = new List<RenderableTriangle>();
            PrimitiveRenderObject primitiveRenderObject;

            renderableTriangles.Add(
                new RenderableTriangle(
                    new VertexPositionColorTexture(a, color, Vector2.Zero),
                    new VertexPositionColorTexture(d, color, new Vector2(1, 0)),
                    new VertexPositionColorTexture(b, color, new Vector2(0, 1))));

            renderableTriangles.Add(
                new RenderableTriangle(
                    new VertexPositionColorTexture(b, color, new Vector2(0, 1)),
                    new VertexPositionColorTexture(d, color, new Vector2(1, 0)),
                    new VertexPositionColorTexture(c, color, new Vector2(1, 1))));

            primitiveRenderObject = new PrimitiveRenderObject(texture, renderableTriangles, layerDepth);
            primitiveRenderObject.originMatrix = Matrix.CreateTranslation(new Vector3(-origin / gameScale, 0));
            primitiveRenderObject.worldMatrix = primitiveRenderObject.originMatrix * Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(new Vector3(position, 0));
            return primitiveRenderObject;
        }

        // Add a renderable primitive to the draw list
        // TODO: This creates a lot of garbage... should consider refactoring.
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

        // drawRenderablePrimitives -- Draws the primitives
        public void drawRenderablePrimitives()
        {
            RenderablePrimitiveNode current = _headNode;

            while (current != null)
            {
                _graphicsDevice.Textures[0] = current.renderablePrimitive.texture;
                _primitivesEffect.Parameters["world"].SetValue(current.renderablePrimitive.worldMatrix);
                _primitivesEffect.CurrentTechnique.Passes["textured_primitives"].Apply();
                _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, current.renderablePrimitive.vertices, 0, current.renderablePrimitive.primitiveCount, VertexPositionColorTexture.VertexDeclaration);
                //_graphicsDevice.DrawUserPrimitives<CustomVertexFormat>(PrimitiveType.TriangleList, current.renderablePrimitive.vertices, 0, current.renderablePrimitive.primitiveCount);

                current = current.next;
            }

            _headNode = null;
        }

        // update
        public void update(GameTime gameTime)
        {
            LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;

            if (InputSystem.newKeyState.IsKeyDown(Keys.F4) && InputSystem.oldKeyState.IsKeyUp(Keys.F4))
            {
                _enlargeDebugFuild = !_enlargeDebugFuild;
            }

            if (levelSystem.finalized)
            {
                _backgroundRenderer.update(_scale, -screenCenter);
            }
        }

        // draw
        public void draw(GameTime gameTime)
        {
            string levelUid = LevelSystem.currentLevelUid;
            FluidSystem fluidSystem = (FluidSystem)_systemManager.getSystem(SystemType.Fluid);
            List<int> primitiveRenderEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.PrimitivesRender);
            List<int> ropeEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Rope);
            List<int> characterRenderEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.CharacterRender);
            List<int> characterMovementEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.CharacterMovement);
            List<int> treeEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Tree);
            List<int> aimEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Aim);
            List<int> explosionEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Explosion);
            List<RopeGrabComponent> ropeGrabComponents = _entityManager.getComponents<RopeGrabComponent>(levelUid, ComponentType.RopeGrab);
            List<TooltipComponent> tooltipComponents = _entityManager.getComponents<TooltipComponent>(levelUid, ComponentType.Tooltip);
            Vector2 screenCenter = _cameraSystem.screenCenter;

            // Temporary debug draw
            if (LoderGame.debug)
            {
                Vector2 debugOffset = new Vector2(0f, 5f);
                _graphicsDevice.SetRenderTarget(_debugFluid);
                _graphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin();

                // Cells
                foreach (KeyValuePair<int, Dictionary<int, List<int>>> row1Pair in fluidSystem.fluidGrid)
                {
                    foreach (KeyValuePair<int, List<int>> row2Pair in row1Pair.Value)
                    {
                        int gridSize = (int)(FluidSystem.CELL_SPACING * scale) - 1;
                        Vector2 position = new Vector2((float)row1Pair.Key * FluidSystem.CELL_SPACING, (float)row2Pair.Key * FluidSystem.CELL_SPACING);
                        _spriteBatch.Draw(_pixel, (position - debugOffset) * scale + _halfScreen, new Rectangle(0, 0, gridSize, gridSize), Color.DarkBlue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }

                // Particle pressures
                for (int i = 0; i < FluidSystem.MAX_PARTICLES; i++)
                {
                    Particle particle = fluidSystem.liquid[i];
                    _spriteBatch.Draw(_pixel, (particle.position - debugOffset) * scale + _halfScreen, new Rectangle(0, 0, 16, 16), Color.Red * 0.5f, 0f, new Vector2(8, 8), Math.Abs(particle.pressure) / FluidSystem.MAX_PRESSURE, SpriteEffects.None, 0f);
                }

                // Particle near pressures
                for (int i = 0; i < FluidSystem.MAX_PARTICLES; i++)
                {
                    Particle particle = fluidSystem.liquid[i];
                    _spriteBatch.Draw(_pixel, (particle.position - debugOffset) * scale + _halfScreen, new Rectangle(0, 0, 16, 16), Color.Orange * 0.5f, 0f, new Vector2(8, 8), Math.Abs(particle.pressureNear) / FluidSystem.MAX_PRESSURE_NEAR, SpriteEffects.None, 0f);
                }

                // Particle positions
                for (int i = 0; i < FluidSystem.MAX_PARTICLES; i++)
                {
                    Particle particle = fluidSystem.liquid[i];
                    Color color = particle.active ? Color.White : Color.DarkGray;
                    _spriteBatch.Draw(_pixel, (particle.position - debugOffset) * scale + _halfScreen, new Rectangle(0, 0, 4, 4), color, 0, new Vector2(2, 2), 1, SpriteEffects.None, 0);
                }

                // Simulation AABB
                Vector2[] vertices = new Vector2[4];
                vertices[0] = fluidSystem.simulationAABB.LowerBound;
                vertices[1] = new Vector2(fluidSystem.simulationAABB.UpperBound.X, fluidSystem.simulationAABB.LowerBound.Y);
                vertices[2] = fluidSystem.simulationAABB.UpperBound;
                vertices[3] = new Vector2(fluidSystem.simulationAABB.LowerBound.X, fluidSystem.simulationAABB.UpperBound.Y);
                for (int i = 0; i < 4; i++)
                {
                    Vector2 a = vertices[i];
                    Vector2 b = vertices[i == 3 ? 0 : i + 1];
                    Vector2 relative = b - a;
                    float angle = (float)Math.Atan2(relative.Y, relative.X);
                    Rectangle rect = new Rectangle(0, 0, (int)(relative.Length() * scale), 2);
                    _spriteBatch.Draw(_pixel, (a - debugOffset) * scale + _halfScreen, rect, Color.Lime, angle, new Vector2(0, 1), 1f, SpriteEffects.None, 0);
                }

                _spriteBatch.End();
                _graphicsDevice.SetRenderTarget(null);
            }

            // Begin drawing a source for post effects under the player's level
            _graphicsDevice.SetRenderTarget(_postSourceUnder);
            _graphicsDevice.Clear(Color.Black);

            // Draw background
            if (_backgroundRenderer.background != null)
            {
                _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                _backgroundRenderer.drawFirstHalf();
                _spriteBatch.End();
            }

            // Begin ordered drawing
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            _halfScreen = new Vector2(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height) / 2;
            _viewMatrix = Matrix.CreateTranslation(new Vector3(-screenCenter, 0)) * Matrix.CreateScale(new Vector3(_scale, -_scale, 1f));
            _projectionMatrix = Matrix.CreateOrthographic(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, 0, 1);
            _primitivesEffect.Parameters["view"].SetValue(_viewMatrix);
            _primitivesEffect.Parameters["projection"].SetValue(_projectionMatrix);

            // Primitive rendering
            for (int i = 0; i < primitiveRenderEntities.Count; i++)
            {
                int entityId = primitiveRenderEntities[i];
                PrimitivesRenderComponent primitiveRenderComponent = (PrimitivesRenderComponent)_entityManager.getComponent(levelUid, entityId, ComponentType.PrimitivesRender);

                for (int j = 0; j < primitiveRenderComponent.primitiveRenderObjects.Count; j++)
                {
                    PrimitiveRenderObject primitiveRenderObject = primitiveRenderComponent.primitiveRenderObjects[j];
                    PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(levelUid, entityId, ComponentType.Physics);
                    IComponent component;

                    // Update world matrix
                    if (physicsComponent != null)
                    {
                        primitiveRenderObject.worldMatrix = primitiveRenderObject.originMatrix * Matrix.CreateRotationZ(physicsComponent.body.Rotation) * Matrix.CreateTranslation(new Vector3(physicsComponent.body.Position, 0));
                    }
                    else if (_entityManager.tryGetComponent(levelUid, entityId, ComponentType.FollowMetamer, out component))
                    {
                        FollowMetamerComponent followMetamerComponent = component as FollowMetamerComponent;
                        primitiveRenderObject.worldMatrix = primitiveRenderObject.originMatrix * Matrix.CreateRotationZ(followMetamerComponent.metamer.currentAngle + StasisMathHelper.halfPi) * Matrix.CreateTranslation(new Vector3(followMetamerComponent.metamer.position, 0));
                    }

                    // Update vertices
                    primitiveRenderObject.updateVertices();

                    addRenderablePrimitive(primitiveRenderObject);
                }
            }

            // Rope rendering
            for (int i = 0; i < ropeEntities.Count; i++)
            {
                int entityId = ropeEntities[i];
                RopeComponent ropeComponent = _entityManager.getComponent(levelUid, entityId, ComponentType.Rope) as RopeComponent;
                RopeNode current = ropeComponent.ropeNodeHead;
                RopeNode head = current;
                RopeNode tail = head.tail;
                Vector2 position;
                float muIncrement = 1f / (float)ropeComponent.interpolationCount;

                while (current != null)
                {
                    float mu = 0f;
                    for (int j = 0; j < ropeComponent.interpolationCount; j++)
                    {
                        Texture2D texture = current.ropeNodeTextures[j].texture;
                        Vector2 a;
                        Vector2 b = current.body.GetWorldPoint(new Vector2(current.halfLength, 0));
                        Vector2 c = current.body.GetWorldPoint(new Vector2(-current.halfLength, 0));
                        Vector2 d;

                        // Determine a's position
                        if (current.previous == null)
                            a = b + (b - c);
                        else
                            a = current.previous.body.GetWorldPoint(new Vector2(current.halfLength, 0));

                        // Determine d's position
                        if (current.next == null)
                            d = c + (c - b);
                        else
                            d = current.next.body.GetWorldPoint(new Vector2(-current.halfLength, 0));

                        StasisMathHelper.interpolate(ref a, ref b, ref c, ref d, mu, out position);
                        _spriteBatch.Draw(texture, (position - screenCenter) * _scale + _halfScreen, texture.Bounds, Color.White, current.body.Rotation + current.ropeNodeTextures[j].angleOffset, current.ropeNodeTextures[j].center, 1f, SpriteEffects.None, 0.1f);

                        mu += muIncrement;
                    }

                    current = current.next;
                }
            }

            // Character rendering
            for (int i = 0; i < characterRenderEntities.Count; i++)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(levelUid, characterRenderEntities[i], ComponentType.Physics);
                CharacterRenderComponent characterRenderComponent = _entityManager.getComponent(levelUid, characterRenderEntities[i], ComponentType.CharacterRender) as CharacterRenderComponent;
                Vector2 offset;
                Texture2D texture = _animationManager.getTexture(characterRenderComponent.character, characterRenderComponent.animation, characterRenderComponent.currentFrame, out offset);

                _spriteBatch.Draw(texture, (physicsComponent.body.Position - screenCenter) * _scale + _halfScreen, texture.Bounds, Color.White, 0, offset, 1f, SpriteEffects.None, 0.05f);
            }

            /*
            for (int i = 0; i < characterMovementEntities.Count; i++)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(levelUid, characterMovementEntities[i], ComponentType.Physics);
                CharacterMovementComponent characterMovementComponent = (CharacterMovementComponent)_entityManager.getComponent(levelUid, characterMovementEntities[i], ComponentType.CharacterMovement);
                Vector2 movementUnitVector = characterMovementComponent.movementUnitVector;
                Rectangle source = new Rectangle(0, 0, (int)(movementUnitVector.Length() * _scale), 2);
                float angle = (float)Math.Atan2(movementUnitVector.Y, movementUnitVector.X);

                _spriteBatch.Draw(_pixel, (physicsComponent.body.Position - screenCenter) * _scale + _halfScreen, source, Color.Yellow, angle, new Vector2(0, 1), 1f, SpriteEffects.None, 0);
            }*/

            // Tree
            _primitivesEffect.Parameters["world"].SetValue(Matrix.Identity);
            for (int i = 0; i < treeEntities.Count; i++)
            {
                TreeComponent treeComponent = _entityManager.getComponent(levelUid, treeEntities[i], ComponentType.Tree) as TreeComponent;

                if (treeComponent.tree.active)
                {
                    addRenderablePrimitive(treeComponent.tree);
                    treeComponent.tree.rootMetamer.draw(this);
                }
            }

            drawRenderablePrimitives();

            // Rope grab components (TEMPORARY)
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

            // Draw explosions (TEMPORARY)
            for (int i = 0; i < explosionEntities.Count; i++)
            {
                ExplosionComponent explosionComponent = (ExplosionComponent)_entityManager.getComponent(levelUid, explosionEntities[i], ComponentType.Explosion);
                _spriteBatch.Draw(_circle, (explosionComponent.position - screenCenter) * _scale + _halfScreen, _circle.Bounds, Color.Red, 0f, new Vector2(_circle.Width, _circle.Height) / 2f, ((explosionComponent.radius * _scale) / (_circle.Width / 2f)), SpriteEffects.None, 0f);
            }

            // Aim components
            for (int i = 0; i < aimEntities.Count; i++)
            {
                AimComponent aimComponent = (AimComponent)_entityManager.getComponent(levelUid, aimEntities[i], ComponentType.Aim);
                Vector2 worldPosition = (_entityManager.getComponent(levelUid, aimEntities[i], ComponentType.WorldPosition) as WorldPositionComponent).position;
                float length = aimComponent.length;

                _spriteBatch.Draw(_reticle, (worldPosition - screenCenter + new Vector2((float)Math.Cos(aimComponent.angle), (float)Math.Sin(aimComponent.angle)) * length) * _scale + _halfScreen, _reticle.Bounds, Color.Red, aimComponent.angle, new Vector2(_reticle.Width, _reticle.Height) / 2f, 1f, SpriteEffects.None, 0f);
            }

            _spriteBatch.End();

            // Begin drawing source for post effects over the player's layer
            _graphicsDevice.SetRenderTarget(_postSourceOver);
            _graphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            // Draw background's second half
            if (_backgroundRenderer.background != null)
            {
                _backgroundRenderer.drawSecondHalf();
            }

            // Draw tooltips
            for (int i = 0; i < tooltipComponents.Count; i++)
            {
                TooltipComponent tooltip = tooltipComponents[i];

                if (tooltip.draw)
                {
                    Vector2 tooltipPosition = (tooltip.position - screenCenter) * _scale + _halfScreen - new Vector2(0, 50f);

                    _spriteBatch.DrawString(_tooltipFont, tooltip.message, tooltipPosition + new Vector2(2, 2), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0001f);
                    _spriteBatch.DrawString(_tooltipFont, tooltip.message, tooltipPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    tooltip.draw = false;
                }
            }

            _spriteBatch.End();
            _graphicsDevice.SetRenderTarget(null);
            _graphicsDevice.Clear(Color.Transparent);


            // Render fluid
            _graphicsDevice.SetRenderTarget(_fluidRenderTarget);
            _graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            int limit = fluidSystem.numActiveParticles;
            for (int i = 0; i < limit; i++)
            {
                // Current particle
                Particle particle = fluidSystem.liquid[fluidSystem.activeParticles[i]];
                Color color = new Color(1, particle.velocity.X < 0 ? -particle.velocity.X : particle.velocity.X, particle.velocity.Y < 0 ? -particle.velocity.Y : particle.velocity.Y);
                spriteBatch.Draw(_fluidParticleTexture, (particle.position - _cameraSystem.screenCenter) * scale + _halfScreen, _fluidParticleTexture.Bounds, color, 0, new Vector2(16, 16), 1, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            _graphicsDevice.SetRenderTarget(_renderedFluid);
            _graphicsDevice.Clear(Color.Transparent);

            // Draw post-processed render target to screen
            _graphicsDevice.Textures[1] = _postSourceUnder;
            _fluidEffect.Parameters["renderSize"].SetValue(new Vector2(_renderedFluid.Width, _renderedFluid.Height));
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, _fluidEffect);
            spriteBatch.Draw(_fluidRenderTarget, Vector2.Zero, Color.DarkBlue);
            spriteBatch.End();
            _graphicsDevice.SetRenderTarget(null);

            // Draw post source under and over
            _spriteBatch.Begin();
            _spriteBatch.Draw(_postSourceUnder, _postSourceUnder.Bounds, Color.White);
            _spriteBatch.End();

            // Draw fluid
            if (fluidSystem != null)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_renderedFluid, _renderedFluid.Bounds, Color.White);
                _spriteBatch.End();
            }

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(_postSourceOver, _postSourceOver.Bounds, Color.White);
            _spriteBatch.End();

            // Particle debug
            if (LoderGame.debug)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_debugFluid, Vector2.Zero, _debugFluid.Bounds, Color.White, 0f, Vector2.Zero, _enlargeDebugFuild ? 1f : 0.25f, SpriteEffects.None, 0f);
                _spriteBatch.End();

                /*
                _spriteBatch.Begin();
                int limit = fluidSystem.numActiveParticles;
                for (int i = 0; i < limit; i++)
                {
                    // Current particle
                    Particle particle = fluidSystem.liquid[fluidSystem.activeParticles[i]];
                    spriteBatch.Draw(_pixel, (particle.position - _cameraSystem.screenCenter) * scale + _halfScreen, new Rectangle(0, 0, 2, 2), Color.White, 0, new Vector2(1, 1), 1, SpriteEffects.None, 0);
                }
                _spriteBatch.End();
                */
            }

            // AI Wander Behavior debug
            if (LoderGame.debug)
            {
                AIBehaviorSystem aiBehaviorSystem = _systemManager.getSystem(SystemType.AIBehavior) as AIBehaviorSystem;
                List<int> wanderBehaviorEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.AIWanderBehavior);

                for (int i =0; i < wanderBehaviorEntities.Count; i++)
                {
                    AIWanderBehaviorComponent wanderComponent = _entityManager.getComponent(levelUid, wanderBehaviorEntities[i], ComponentType.AIWanderBehavior) as AIWanderBehaviorComponent;
                    List<WaypointsComponent> waypointsComponents = _entityManager.getComponents<WaypointsComponent>(levelUid, ComponentType.Waypoints);
                    WaypointsComponent waypointsComponent = aiBehaviorSystem.getWaypointsComponent(wanderComponent.waypointsUid, waypointsComponents);
                    Vector2 waypointPosition = waypointsComponent.waypoints[wanderComponent.currentWaypointIndex];

                    _spriteBatch.Begin();
                    _spriteBatch.Draw(_circle, (waypointPosition - screenCenter) * _scale + _halfScreen, _circle.Bounds, Color.Red, 0f, new Vector2(_circle.Width, _circle.Height) / 2f, 0.02f, SpriteEffects.None, 0f);
                    _spriteBatch.End();
                }
            }
        }
    }
}
