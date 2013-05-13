using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        private Vector2 _spawnPosition;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Player; } }
        public int playerId { get { return _playerId; } set { _playerId = value; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public EntityManager entityManager { get { return _entityManager; } }
        public Vector2 spawnPosition { get { return _spawnPosition; } set { _spawnPosition = value; } }

        public PlayerSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        // createPlayer
        public void createPlayer()
        {
            playerId = _entityManager.createEntity(PLAYER_ID);
        }

        // initializePlayerInventory -- Creates the player's inventory components
        public void initializePlayerInventory()
        {
            _entityManager.initializePlayerInventory(_playerId, DataManager.playerData.inventoryData);
            _entityManager.initializePlayerToolbar(
                _playerId,
                (InventoryComponent)_entityManager.getComponent(_playerId, ComponentType.Inventory),
                DataManager.playerData.toolbarData);
        }

        // softKillPlayer -- Doesn't "kill" the player entity, just resets certain aspects of the entity to the last saved state
        public void softKillPlayer()
        {
            List<IComponent> components = new List<IComponent>(_entityManager.getEntityComponents(_playerId));  // create a copy of the list since we'll need to modify the original

            for (int i = 0; i < components.Count; i++)
            {
                // Exclude certain components here if they need to persist through death, otherwise remove them.
                _entityManager.removeComponent(_playerId, components[i]);
            }

            initializePlayerInventory();
        }

        // update
        public void update()
        {
            PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(_playerId, ComponentType.Physics);

            if (physicsComponent != null)   // If there is a physics component, then we're in a level.
            {
                InputComponent inputComponent = _entityManager.getComponent(_playerId, ComponentType.Input) as InputComponent;
                CharacterMovementComponent characterMovementComponent = _entityManager.getComponent(_playerId, ComponentType.CharacterMovement) as CharacterMovementComponent;
                RopeGrabComponent ropeGrabComponent = _entityManager.getComponent(_playerId, ComponentType.RopeGrab) as RopeGrabComponent;

                if (inputComponent.usingGamepad)
                {
                    if (inputComponent.newGamepadState.ThumbSticks.Left.X < 0)
                    {
                        characterMovementComponent.walkSpeedModifier = Math.Abs(inputComponent.newGamepadState.ThumbSticks.Left.X);
                        characterMovementComponent.walkLeft = true;
                    }
                    else if (inputComponent.newGamepadState.DPad.Left == ButtonState.Pressed)
                        characterMovementComponent.walkLeft = true;
                    else
                        characterMovementComponent.walkLeft = false;

                    if (inputComponent.newGamepadState.ThumbSticks.Left.X > 0)
                    {
                        characterMovementComponent.walkSpeedModifier = inputComponent.newGamepadState.ThumbSticks.Left.X;
                        characterMovementComponent.walkRight = true;
                    }
                    else if (inputComponent.newGamepadState.DPad.Right == ButtonState.Pressed)
                        characterMovementComponent.walkRight = true;
                    else
                        characterMovementComponent.walkRight = false;

                    characterMovementComponent.climbAmount = 0f;
                    characterMovementComponent.climbDown = false;
                    characterMovementComponent.climbUp = false;
                    if (inputComponent.newGamepadState.ThumbSticks.Left.Y > 0)
                    {
                        characterMovementComponent.climbUp = true;
                        characterMovementComponent.climbAmount = Math.Abs(inputComponent.newGamepadState.ThumbSticks.Left.Y);
                    }
                    else if (inputComponent.newGamepadState.ThumbSticks.Left.Y < 0)
                    {
                        characterMovementComponent.climbDown = true;
                        characterMovementComponent.climbAmount = Math.Abs(inputComponent.newGamepadState.ThumbSticks.Left.Y);
                    }

                    characterMovementComponent.jump = inputComponent.newGamepadState.Buttons.A == ButtonState.Pressed;
                }
                else
                {
                    characterMovementComponent.walkSpeedModifier = 1f;
                    characterMovementComponent.walkLeft = inputComponent.newKeyState.IsKeyDown(Keys.A) || inputComponent.newKeyState.IsKeyDown(Keys.Left);
                    characterMovementComponent.walkRight = inputComponent.newKeyState.IsKeyDown(Keys.D) || inputComponent.newKeyState.IsKeyDown(Keys.Right);
                    characterMovementComponent.jump = inputComponent.newKeyState.IsKeyDown(Keys.Space);
                    characterMovementComponent.climbUp = inputComponent.newKeyState.IsKeyDown(Keys.W);
                    characterMovementComponent.climbDown = inputComponent.newKeyState.IsKeyDown(Keys.S);
                    characterMovementComponent.climbAmount = 1f;
                    characterMovementComponent.doRopeGrab = inputComponent.newKeyState.IsKeyDown(Keys.E);
                    characterMovementComponent.allowRopeGrab = characterMovementComponent.allowRopeGrab ? true : (inputComponent.newKeyState.IsKeyUp(Keys.E) && inputComponent.oldKeyState.IsKeyDown(Keys.E));

                }
            }
        }
    }
}
