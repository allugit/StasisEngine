using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using StasisGame.Components;
using StasisGame.Managers;
using StasisCore;

namespace StasisGame.Systems
{
    public class PlayerSystem : ISystem
    {
        public const int PLAYER_ID = 9999;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private int _playerId;
        private bool _paused;
        private bool _singleStep;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Player; } }
        public int playerId { get { return _playerId; } set { _playerId = value; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public EntityManager entityManager { get { return _entityManager; } }

        public PlayerSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        // createPlayer
        public void createPlayer()
        {
            playerId = _entityManager.createEntity("global", PLAYER_ID);
        }

        // Add level-specific components for the player
        public void addPlayerToLevel(string levelUid, Vector2 position)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).getWorld(levelUid);
            Body body;
            Fixture fixture;
            Fixture feetFixture;
            PolygonShape polygonShape = new PolygonShape(1f);
            CircleShape feetShape = new CircleShape(0.18f, 0.1f);

            body = BodyFactory.CreateBody(world, position, _playerId);
            body.IsBullet = true;
            body.FixedRotation = true;
            body.BodyType = BodyType.Dynamic;
            body.SleepingAllowed = false;
            polygonShape.SetAsBox(0.18f, 0.27f);
            fixture = body.CreateFixture(polygonShape);
            fixture.Friction = 0f;
            fixture.Restitution = 0f;
            fixture.CollisionCategories = (ushort)CollisionCategory.Player;
            fixture.CollidesWith = 
                (ushort)CollisionCategory.DynamicGeometry |
                (ushort)CollisionCategory.Item |
                (ushort)CollisionCategory.Rope |
                (ushort)CollisionCategory.StaticGeometry |
                (ushort)CollisionCategory.Explosion;

            feetShape.Position = new Vector2(0, 0.27f);
            feetShape.Density = 0.1f;
            feetFixture = body.CreateFixture(feetShape);
            feetFixture.Friction = 0.1f;
            feetFixture.CollisionCategories = fixture.CollisionCategories;
            feetFixture.CollidesWith = fixture.CollidesWith;

            _entityManager.createEntity(levelUid, playerId);
            _entityManager.addComponent(levelUid, playerId, _entityManager.getComponent("global", playerId, ComponentType.Toolbar));
            _entityManager.addComponent(levelUid, playerId, _entityManager.getComponent("global", playerId, ComponentType.Inventory));
            _entityManager.addComponent(levelUid, playerId, new PhysicsComponent(body));
            _entityManager.addComponent(levelUid, playerId, new CharacterMovementComponent(feetFixture, 7f));
            _entityManager.addComponent(levelUid, playerId, new CharacterRenderComponent());
            _entityManager.addComponent(levelUid, playerId, new BodyFocusPointComponent(body, new Vector2(0, -5f), FocusType.Multiple));
            _entityManager.addComponent(levelUid, playerId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(levelUid, playerId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(levelUid, playerId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(levelUid, playerId, new SkipFluidResolutionComponent());
            _entityManager.addComponent(levelUid, playerId, new ParticleInfluenceComponent(ParticleInfluenceType.Character));
        }

        // removePlayerFromLevel -- Removes the player entity from a specific level
        public void removePlayerFromLevel(string levelUid)
        {
            _entityManager.killEntity(levelUid, playerId);
        }

        public void reloadInventory()
        {
            throw new NotImplementedException();
            //_entityManager.removeComponent(_playerId, ComponentType.Inventory);
            //_entityManager.removeComponent(_playerId, ComponentType.Toolbar);
            //DataManager.loadPlayerInventory();
            //DataManager.loadPlayerToolbar();
        }

        // update
        public void update()
        {
            if (!_paused || _singleStep)
            {
                string levelUid = LevelSystem.currentLevelUid;
                LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;

                if (levelSystem.finalized)
                {
                    PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(levelUid, _playerId, ComponentType.Physics);

                    if (physicsComponent != null)   // If there is a physics component, then we're in a level.
                    {
                        CharacterMovementComponent characterMovementComponent = _entityManager.getComponent(levelUid, _playerId, ComponentType.CharacterMovement) as CharacterMovementComponent;
                        RopeGrabComponent ropeGrabComponent = _entityManager.getComponent(levelUid, _playerId, ComponentType.RopeGrab) as RopeGrabComponent;

                        if (InputSystem.usingGamepad)
                        {
                            if (InputSystem.newGamepadState.ThumbSticks.Left.X < 0)
                            {
                                //characterMovementComponent.walkSpeedModifier = Math.Abs(InputSystem.newGamepadState.ThumbSticks.Left.X);
                                characterMovementComponent.walkLeft = true;
                            }
                            else if (InputSystem.newGamepadState.DPad.Left == ButtonState.Pressed)
                                characterMovementComponent.walkLeft = true;
                            else
                                characterMovementComponent.walkLeft = false;

                            if (InputSystem.newGamepadState.ThumbSticks.Left.X > 0)
                            {
                                //characterMovementComponent.walkSpeedModifier = InputSystem.newGamepadState.ThumbSticks.Left.X;
                                characterMovementComponent.walkRight = true;
                            }
                            else if (InputSystem.newGamepadState.DPad.Right == ButtonState.Pressed)
                                characterMovementComponent.walkRight = true;
                            else
                                characterMovementComponent.walkRight = false;

                            characterMovementComponent.climbAmount = 0f;
                            characterMovementComponent.climbDown = false;
                            characterMovementComponent.climbUp = false;
                            if (InputSystem.newGamepadState.ThumbSticks.Left.Y > 0)
                            {
                                characterMovementComponent.climbUp = true;
                                characterMovementComponent.climbAmount = Math.Abs(InputSystem.newGamepadState.ThumbSticks.Left.Y);
                            }
                            else if (InputSystem.newGamepadState.ThumbSticks.Left.Y < 0)
                            {
                                characterMovementComponent.climbDown = true;
                                characterMovementComponent.climbAmount = Math.Abs(InputSystem.newGamepadState.ThumbSticks.Left.Y);
                            }

                            characterMovementComponent.jump = InputSystem.newGamepadState.Buttons.A == ButtonState.Pressed;
                        }
                        else
                        {
                            //characterMovementComponent.walkSpeedModifier = 1f;
                            characterMovementComponent.walkLeft = InputSystem.newKeyState.IsKeyDown(Keys.A) || InputSystem.newKeyState.IsKeyDown(Keys.Left);
                            characterMovementComponent.walkRight = InputSystem.newKeyState.IsKeyDown(Keys.D) || InputSystem.newKeyState.IsKeyDown(Keys.Right);
                            characterMovementComponent.jump = InputSystem.newKeyState.IsKeyDown(Keys.Space);
                            characterMovementComponent.climbUp = InputSystem.newKeyState.IsKeyDown(Keys.W);
                            characterMovementComponent.climbDown = InputSystem.newKeyState.IsKeyDown(Keys.S);
                            characterMovementComponent.climbAmount = 1f;
                            characterMovementComponent.doRopeGrab = InputSystem.newKeyState.IsKeyDown(Keys.E) && InputSystem.oldKeyState.IsKeyUp(Keys.E);
                            characterMovementComponent.allowRopeGrab = characterMovementComponent.allowRopeGrab ? true : (InputSystem.newKeyState.IsKeyUp(Keys.E) && InputSystem.oldKeyState.IsKeyDown(Keys.E));
                        }
                    }
                }
            }
            _singleStep = false;
        }
    }
}
