using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Box2D.XNA;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame.Systems
{
    public class EquipmentSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private bool _paused;
        private bool _singleStep;

        public SystemType systemType { get { return SystemType.Equipment; } }
        public int defaultPriority { get { return 10; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public EquipmentSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
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
                RopeSystem ropeSystem = _systemManager.getSystem(SystemType.Rope) as RopeSystem;
                PhysicsComponent playerPhysicsComponent = _entityManager.getComponent(playerSystem.playerId, ComponentType.Physics) as PhysicsComponent;
                List<int> toolbarEntities = _entityManager.getEntitiesPosessing(ComponentType.Toolbar);

                // Player
                if (playerSystem != null)
                {
                    int playerId = playerSystem.playerId;
                    ToolbarComponent playerToolbar = _entityManager.getComponent(playerId, ComponentType.Toolbar) as ToolbarComponent;
                    WorldPositionComponent playerPositionComponent = _entityManager.getComponent(playerId, ComponentType.WorldPosition) as WorldPositionComponent;
                    ItemComponent selectedItem = playerToolbar.selectedItem;

                    if (selectedItem != null)
                    {
                        bool mouseLeftDown = InputSystem.newMouseState.LeftButton == ButtonState.Pressed && InputSystem.oldMouseState.LeftButton == ButtonState.Released;
                        bool mouseRightDown = InputSystem.newMouseState.RightButton == ButtonState.Pressed && InputSystem.oldMouseState.RightButton == ButtonState.Released;
                        bool leftTriggerDown = InputSystem.newGamepadState.IsConnected && InputSystem.newGamepadState.Triggers.Left > 0.5f && InputSystem.oldGamepadState.Triggers.Left <= 0.5f;
                        bool rightTriggerDown = InputSystem.newGamepadState.IsConnected && InputSystem.newGamepadState.Triggers.Right > 0.5f && InputSystem.oldGamepadState.Triggers.Right <= 0.5f;
                        AimComponent aimComponent = _entityManager.getComponent(playerId, ComponentType.Aim) as AimComponent;

                        if (mouseLeftDown || leftTriggerDown)
                        {
                            selectedItem.primaryAction = true;
                        }
                        if (mouseRightDown || rightTriggerDown)
                        {
                            selectedItem.secondaryAction = true;
                        }

                        if (selectedItem.hasAiming && aimComponent != null)
                        {
                            WorldPositionComponent worldPositionComponent = _entityManager.getComponent(playerId, ComponentType.WorldPosition) as WorldPositionComponent;

                            if (worldPositionComponent != null)
                            {
                                Vector2 worldPosition = worldPositionComponent.position;
                                if (InputSystem.newGamepadState.IsConnected)
                                {
                                    Vector2 aim = InputSystem.newGamepadState.ThumbSticks.Left * selectedItem.maxRange;
                                    aim.Y *= -1;
                                    aimComponent.angle = (float)Math.Atan2(aim.Y, aim.X);
                                    aimComponent.length = aim.Length();
                                    aimComponent.vector = aim;
                                }
                                else
                                {
                                    Vector2 relative = (InputSystem.worldMouse - worldPosition);
                                    aimComponent.angle = (float)Math.Atan2(relative.Y, relative.X);
                                    aimComponent.length = Math.Min(relative.Length(), selectedItem.maxRange);
                                    relative.Normalize();
                                    aimComponent.vector = relative * aimComponent.length;
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
                        if (selectedItem.primaryAction || selectedItem.secondaryAction)
                        {
                            if (selectedItem.secondaryAction)
                                Console.WriteLine("secondary action");

                            switch (selectedItem.itemType)
                            {
                                // RopeGun
                                case ItemType.RopeGun:
                                    if (selectedItem.primaryAction)
                                    {
                                        AimComponent aimComponent = _entityManager.getComponent(toolbarEntities[i], ComponentType.Aim) as AimComponent;
                                        Vector2 initialPointA = (_entityManager.getComponent(toolbarEntities[i], ComponentType.WorldPosition) as WorldPositionComponent).position;
                                        Vector2 initialPointB = initialPointA + new Vector2((float)Math.Cos(aimComponent.angle), (float)Math.Sin(aimComponent.angle)) * aimComponent.length;
                                        int ropeEntityId = _entityManager.factory.createRope(false, true, initialPointA, initialPointB, -1);

                                        if (ropeEntityId != -1)
                                        {
                                            RopeGrabComponent ropeGrabComponent = _entityManager.getComponent(toolbarComponent.entityId, ComponentType.RopeGrab) as RopeGrabComponent;
                                            RopePhysicsComponent ropePhysicsComponent = _entityManager.getComponent(ropeEntityId, ComponentType.RopePhysics) as RopePhysicsComponent;
                                            PhysicsComponent physicsComponent = _entityManager.getComponent(toolbarEntities[i], ComponentType.Physics) as PhysicsComponent;
                                            RopeGrabComponent newRopeGrabComponent = null;
                                            Vector2 initialVelocity = physicsComponent.body.GetLinearVelocity();
                                            RopeNode currentNode = null;
                                            int ropeSegmentCount;

                                            if (physicsComponent == null)
                                                break;

                                            // Handle initial velocity
                                            currentNode = ropePhysicsComponent.ropeNodeHead;
                                            ropeSegmentCount = currentNode.count;
                                            System.Diagnostics.Debug.Assert(ropeSegmentCount != 0);
                                            int count = ropeSegmentCount;
                                            while (currentNode != null)
                                            {
                                                float weight = (float)count / (float)ropeSegmentCount;

                                                currentNode.body.SetLinearVelocity(currentNode.body.GetLinearVelocity() + initialVelocity * weight);

                                                count--;
                                                currentNode = currentNode.next;
                                            }

                                            // Handle previous grabs
                                            if (ropeGrabComponent != null)
                                            {
                                                RopePhysicsComponent previouslyGrabbedRope = _entityManager.getComponent(ropeGrabComponent.ropeEntityId, ComponentType.RopePhysics) as RopePhysicsComponent;
                                                ropeSystem.releaseRope(ropeGrabComponent, physicsComponent.body);

                                                if (previouslyGrabbedRope.destroyAfterRelease)
                                                    previouslyGrabbedRope.timeToLive = 100;
                                                _entityManager.removeComponent(toolbarComponent.entityId, ropeGrabComponent);
                                                ropeGrabComponent = null;
                                            }

                                            newRopeGrabComponent = new RopeGrabComponent(ropeEntityId, ropePhysicsComponent.ropeNodeHead, 0f, ropePhysicsComponent.reverseClimbDirection);
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
                                    if (selectedItem.primaryAction)
                                    {
                                        AimComponent aimComponent = _entityManager.getComponent(toolbarEntities[i], ComponentType.Aim) as AimComponent;

                                        _entityManager.factory.createDynamite(playerPhysicsComponent.body.GetPosition(), aimComponent.vector * 80f);
                                    }
                                    break;
                            }

                            selectedItem.primaryAction = false;
                            selectedItem.secondaryAction = false;
                        }
                    }
                }
            }
            _singleStep = false;
        }
    }
}
