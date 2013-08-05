using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisGame.Components;
using StasisGame.Managers;
using StasisCore;
using StasisCore.Models;

namespace StasisGame.Systems
{
    public class EquipmentSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private bool _paused;
        private bool _singleStep;
        private RopeMaterial _defaultRopeMaterial;
        private Random _rng;

        public SystemType systemType { get { return SystemType.Equipment; } }
        public int defaultPriority { get { return 10; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public EquipmentSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _defaultRopeMaterial = new RopeMaterial(ResourceManager.getResource("default_rope_material"));
            _rng = new Random();
        }

        // assignItemToToolbar
        public void assignItemToToolbar(ItemComponent itemComponent, ToolbarComponent toolbarComponent, int toolbarSlot)
        {
            toolbarComponent.inventory[toolbarSlot] = itemComponent;
            selectToolbarSlot(toolbarComponent, toolbarComponent.selectedIndex);
        }

        // selectToolbarSlot
        public void selectToolbarSlot(ToolbarComponent toolbarComponent, int slot)
        {
            ItemComponent itemComponent = toolbarComponent.selectedItem;
            if (itemComponent != null && itemComponent.hasAiming)
            {
                _entityManager.removeComponent(toolbarComponent.entityId, ComponentType.Aim);
            }

            toolbarComponent.selectedIndex = slot;

            itemComponent = toolbarComponent.selectedItem;
            if (itemComponent != null && itemComponent.hasAiming)
            {
                Console.WriteLine("Adding aim component");
                _entityManager.addComponent(toolbarComponent.entityId, new AimComponent(new Vector2(10f, 0f), 0f, 10f));
            }
        }

        // update
        public void update()
        {
            if (_singleStep || !_paused)
            {
                PlayerSystem playerSystem = _systemManager.getSystem(SystemType.Player) as PlayerSystem;
                LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;
                RopeSystem ropeSystem = _systemManager.getSystem(SystemType.Rope) as RopeSystem;
                PhysicsComponent playerPhysicsComponent = _entityManager.getComponent(playerSystem.playerId, ComponentType.Physics) as PhysicsComponent;
                List<int> toolbarEntities = _entityManager.getEntitiesPosessing(ComponentType.Toolbar);

                // Player equipment
                if (playerSystem != null)
                {
                    int playerId = playerSystem.playerId;
                    ToolbarComponent playerToolbar = _entityManager.getComponent(playerId, ComponentType.Toolbar) as ToolbarComponent;
                    WorldPositionComponent playerPositionComponent = _entityManager.getComponent(playerId, ComponentType.WorldPosition) as WorldPositionComponent;
                    ItemComponent selectedItem = playerToolbar.selectedItem;

                    if (selectedItem != null)
                    {
                        selectedItem.primaryContinuousAction = InputSystem.newMouseState.LeftButton == ButtonState.Pressed;
                        selectedItem.primarySingleAction = selectedItem.primaryContinuousAction && InputSystem.oldMouseState.LeftButton == ButtonState.Released;
                        selectedItem.secondaryContinuousAction = InputSystem.newMouseState.RightButton == ButtonState.Pressed;
                        selectedItem.secondarySingleAction = selectedItem.secondaryContinuousAction && InputSystem.oldMouseState.RightButton == ButtonState.Released;
                        //bool leftTriggerDown = InputSystem.usingGamepad && InputSystem.newGamepadState.Triggers.Left > 0.5f && InputSystem.oldGamepadState.Triggers.Left <= 0.5f;
                        //bool rightTriggerDown = InputSystem.usingGamepad && InputSystem.newGamepadState.Triggers.Right > 0.5f && InputSystem.oldGamepadState.Triggers.Right <= 0.5f;
                        AimComponent aimComponent = _entityManager.getComponent(playerId, ComponentType.Aim) as AimComponent;

                        if (selectedItem.hasAiming && aimComponent != null)
                        {
                            WorldPositionComponent worldPositionComponent = _entityManager.getComponent(playerId, ComponentType.WorldPosition) as WorldPositionComponent;

                            if (worldPositionComponent != null)
                            {
                                Vector2 worldPosition = worldPositionComponent.position;
                                if (InputSystem.usingGamepad)
                                {
                                    Vector2 vector = InputSystem.newGamepadState.ThumbSticks.Left * selectedItem.maxRange;
                                    vector.Y *= -1;
                                    aimComponent.angle = (float)Math.Atan2(vector.Y, vector.X);
                                    aimComponent.length = vector.Length();
                                    aimComponent.vector = vector;
                                }
                                else
                                {
                                    Vector2 relative = (InputSystem.worldMouse - worldPosition);
                                    aimComponent.angle = (float)Math.Atan2(relative.Y, relative.X);
                                    aimComponent.length = Math.Min(relative.Length(), selectedItem.maxRange);
                                    aimComponent.vector = relative;
                                }
                            }
                        }
                    }
                }

                // All toolbars
                for (int i = 0; i < toolbarEntities.Count; i++)
                {
                    ToolbarComponent toolbarComponent = _entityManager.getComponent(toolbarEntities[i], ComponentType.Toolbar) as ToolbarComponent;
                    ItemComponent selectedItem = toolbarComponent.selectedItem;

                    if (selectedItem != null)
                    {
                        if (selectedItem.secondarySingleAction)
                            Console.WriteLine("secondary action");

                        switch (selectedItem.itemType)
                        {
                            // RopeGun
                            case ItemType.RopeGun:
                                if (selectedItem.primarySingleAction)
                                {
                                    AimComponent aimComponent = _entityManager.getComponent(toolbarEntities[i], ComponentType.Aim) as AimComponent;
                                    Vector2 initialPointA = (_entityManager.getComponent(toolbarEntities[i], ComponentType.WorldPosition) as WorldPositionComponent).position;
                                    Vector2 initialPointB = initialPointA + new Vector2((float)Math.Cos(aimComponent.angle), (float)Math.Sin(aimComponent.angle)) * aimComponent.length;
                                    int ropeEntityId = _entityManager.factory.createSingleAnchorRope(initialPointA, initialPointB, _defaultRopeMaterial, true);

                                    if (ropeEntityId != -1)
                                    {
                                        RopeGrabComponent ropeGrabComponent = _entityManager.getComponent(toolbarComponent.entityId, ComponentType.RopeGrab) as RopeGrabComponent;
                                        RopeComponent ropeComponent = _entityManager.getComponent(ropeEntityId, ComponentType.Rope) as RopeComponent;
                                        PhysicsComponent physicsComponent = _entityManager.getComponent(toolbarEntities[i], ComponentType.Physics) as PhysicsComponent;
                                        RopeGrabComponent newRopeGrabComponent = null;
                                        Vector2 initialVelocity = physicsComponent.body.LinearVelocity;
                                        RopeNode currentNode = null;
                                        int ropeSegmentCount;

                                        if (physicsComponent == null)
                                            break;

                                        // Handle initial velocity
                                        currentNode = ropeComponent.ropeNodeHead;
                                        ropeSegmentCount = currentNode.count;
                                        System.Diagnostics.Debug.Assert(ropeSegmentCount != 0);
                                        int count = ropeSegmentCount;
                                        while (currentNode != null)
                                        {
                                            float weight = (float)count / (float)ropeSegmentCount;

                                            currentNode.body.LinearVelocity = currentNode.body.LinearVelocity + initialVelocity * weight;

                                            count--;
                                            currentNode = currentNode.next;
                                        }

                                        // Handle previous grabs
                                        if (ropeGrabComponent != null)
                                        {
                                            RopeComponent previouslyGrabbedRope = _entityManager.getComponent(ropeGrabComponent.ropeEntityId, ComponentType.Rope) as RopeComponent;
                                            ropeSystem.releaseRope(ropeGrabComponent, physicsComponent.body);

                                            if (previouslyGrabbedRope.destroyAfterRelease)
                                                previouslyGrabbedRope.timeToLive = 100;
                                            _entityManager.removeComponent(toolbarComponent.entityId, ropeGrabComponent);
                                            ropeGrabComponent = null;
                                        }

                                        newRopeGrabComponent = new RopeGrabComponent(ropeEntityId, ropeComponent.ropeNodeHead, 0f, ropeComponent.reverseClimbDirection);
                                        ropeSystem.grabRope(newRopeGrabComponent, physicsComponent.body);
                                        _entityManager.addComponent(toolbarComponent.entityId, newRopeGrabComponent);
                                    }
                                }
                                break;

                            // Blueprint
                            case ItemType.Blueprint:
                                Console.WriteLine("Blueprint");
                                break;

                            // BlueprintScrap
                            case ItemType.BlueprintScrap:
                                Console.WriteLine("Blueprint scrap");
                                break;

                            // Dynamite
                            case ItemType.Dynamite:
                                if (selectedItem.primarySingleAction)
                                {
                                    AimComponent aimComponent = _entityManager.getComponent(toolbarEntities[i], ComponentType.Aim) as AimComponent;

                                    _entityManager.factory.createDynamite(playerPhysicsComponent.body.Position, aimComponent.vector * 80f);
                                }
                                break;

                            // Water gun
                            case ItemType.WaterGun:
                                if (selectedItem.primaryContinuousAction)
                                {
                                    FluidSystem fluidSystem = _systemManager.getSystem(SystemType.Fluid) as FluidSystem;
                                    AimComponent aimComponent = _entityManager.getComponent(toolbarEntities[i], ComponentType.Aim) as AimComponent;
                                    Vector2 aimUnitVector = Vector2.Normalize(aimComponent.vector);
                                    Vector2 particlePosition =
                                        playerPhysicsComponent.body.Position +
                                        aimUnitVector +
                                        new Vector2(StasisMathHelper.floatBetween(-0.1f, 0.1f, _rng), StasisMathHelper.floatBetween(-0.1f, 0.1f, _rng));
                                    Vector2 particleVelocity = aimUnitVector * 0.4f;

                                    fluidSystem.createParticle(particlePosition, particleVelocity);
                                }
                                break;
                        }

                        selectedItem.primarySingleAction = false;
                        selectedItem.secondarySingleAction = false;
                        selectedItem.primaryContinuousAction = false;
                        selectedItem.secondaryContinuousAction = false;
                    }
                }
            }
            _singleStep = false;
        }
    }
}
