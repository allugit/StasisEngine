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
        private bool _paused;
        private bool _singleStep;
        private KeyboardState _newKeyState;
        private KeyboardState _oldKeyState;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Player; } }
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
            _entityManager.createEntity("global", PLAYER_ID);
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

            body = BodyFactory.CreateBody(world, position, PLAYER_ID);
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

            _entityManager.createEntity(levelUid, PLAYER_ID);
            _entityManager.addComponent(levelUid, PLAYER_ID, _entityManager.getComponent("global", PLAYER_ID, ComponentType.Toolbar));
            _entityManager.addComponent(levelUid, PLAYER_ID, _entityManager.getComponent("global", PLAYER_ID, ComponentType.Inventory));
            _entityManager.addComponent(levelUid, PLAYER_ID, new PhysicsComponent(body));
            _entityManager.addComponent(levelUid, PLAYER_ID, new CharacterMovementComponent(feetFixture, 7f));
            _entityManager.addComponent(levelUid, PLAYER_ID, new CharacterRenderComponent("main_character"));
            _entityManager.addComponent(levelUid, PLAYER_ID, new BodyFocusPointComponent(body, new Vector2(0, -5f), FocusType.Multiple));
            _entityManager.addComponent(levelUid, PLAYER_ID, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(levelUid, PLAYER_ID, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(levelUid, PLAYER_ID, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(levelUid, PLAYER_ID, new SkipFluidResolutionComponent());
            _entityManager.addComponent(levelUid, PLAYER_ID, new ParticleInfluenceComponent(ParticleInfluenceType.Character));
        }

        // removePlayerFromLevel -- Removes the player entity from a specific level
        public void removePlayerFromLevel(string levelUid)
        {
            _entityManager.killEntity(levelUid, PLAYER_ID);
        }

        public void reloadInventory()
        {
            throw new NotImplementedException();
            //_entityManager.removeComponent(PLAYER_ID, ComponentType.Inventory);
            //_entityManager.removeComponent(PLAYER_ID, ComponentType.Toolbar);
            //DataManager.loadPlayerInventory();
            //DataManager.loadPlayerToolbar();
        }

        private void handleDialogue(string levelUid, PhysicsComponent playerPhysicsComponent)
        {
            DialogueSystem dialogueSystem = _systemManager.getSystem(SystemType.Dialogue) as DialogueSystem;
            List<int> dialogueEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.CharacterDialogue);
            bool inDialogue = _entityManager.getComponent(levelUid, PLAYER_ID, ComponentType.InDialogue) != null;

            for (int i = 0; i < dialogueEntities.Count; i++)
            {
                PhysicsComponent otherPhysicsComponent = _entityManager.getComponent(levelUid, dialogueEntities[i], ComponentType.Physics) as PhysicsComponent;
                Vector2 relative = playerPhysicsComponent.body.Position - otherPhysicsComponent.body.Position;
                float distanceSq = relative.LengthSquared();

                if (_newKeyState.IsKeyDown(Keys.E) && _oldKeyState.IsKeyUp(Keys.E))
                {
                    if (!inDialogue && distanceSq <= 1f)
                    {
                        CharacterDialogueComponent dialogueComponent = _entityManager.getComponent(levelUid, dialogueEntities[i], ComponentType.CharacterDialogue) as CharacterDialogueComponent;

                        dialogueSystem.beginDialogue(levelUid, PlayerSystem.PLAYER_ID, dialogueEntities[i], dialogueComponent);
                    }
                }
                else
                {
                    TooltipComponent tooltipComponent = _entityManager.getComponent(levelUid, dialogueEntities[i], ComponentType.Tooltip) as TooltipComponent;

                    if (tooltipComponent == null)
                    {
                        if (!inDialogue && distanceSq <= 1f)
                        {
                            _entityManager.addComponent(levelUid, dialogueEntities[i], new TooltipComponent("[Use] Talk", otherPhysicsComponent.body, 1f));
                        }
                    }
                    else
                    {
                        if (inDialogue || distanceSq > 1f)
                        {
                            _entityManager.removeComponent(levelUid, dialogueEntities[i], ComponentType.Tooltip);
                        }
                    }
                }
            }
        }

        // update
        public void update(GameTime gameTime)
        {
            if (!_paused || _singleStep)
            {
                string levelUid = LevelSystem.currentLevelUid;
                LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;

                if (levelSystem.finalized)
                {
                    PhysicsComponent playerPhysicsComponent = (PhysicsComponent)_entityManager.getComponent(levelUid, PLAYER_ID, ComponentType.Physics);

                    _oldKeyState = _newKeyState;
                    _newKeyState = Keyboard.GetState();

                    // Handle dialogue
                    handleDialogue(levelUid, playerPhysicsComponent);

                    // Handle character movement
                    if (playerPhysicsComponent != null)
                    {
                        CharacterMovementComponent characterMovementComponent = _entityManager.getComponent(levelUid, PLAYER_ID, ComponentType.CharacterMovement) as CharacterMovementComponent;
                        RopeGrabComponent ropeGrabComponent = _entityManager.getComponent(levelUid, PLAYER_ID, ComponentType.RopeGrab) as RopeGrabComponent;

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

                            characterMovementComponent.attemptJump = InputSystem.newGamepadState.Buttons.A == ButtonState.Pressed;
                        }
                        else
                        {
                            //characterMovementComponent.walkSpeedModifier = 1f;
                            characterMovementComponent.walkLeft = InputSystem.newKeyState.IsKeyDown(Keys.A) || InputSystem.newKeyState.IsKeyDown(Keys.Left);
                            characterMovementComponent.walkRight = InputSystem.newKeyState.IsKeyDown(Keys.D) || InputSystem.newKeyState.IsKeyDown(Keys.Right);
                            characterMovementComponent.attemptJump = InputSystem.newKeyState.IsKeyDown(Keys.Space) && InputSystem.oldKeyState.IsKeyUp(Keys.Space);
                            characterMovementComponent.swimUp = InputSystem.newKeyState.IsKeyDown(Keys.Space);
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
