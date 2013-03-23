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
        private RopeSystem _ropeSystem;
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
            _ropeSystem = _systemManager.getSystem(SystemType.Rope) as RopeSystem;
        }

        public void assignItemToToolbar(ItemComponent itemComponent, ToolbarComponent toolbarComponent, int toolbarSlot)
        {
            toolbarComponent.inventory[toolbarSlot] = itemComponent;
            selectToolbarSlot(toolbarComponent, toolbarComponent.selectedIndex);
        }

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
                _entityManager.addComponent(toolbarComponent.entityId, new AimComponent(0f, 10f));
            }
        }

        public void update()
        {
            if (_singleStep || !_paused)
            {
                PlayerSystem playerSystem = _systemManager.getSystem(SystemType.Player) as PlayerSystem;
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
                            Vector2 worldPosition = (_entityManager.getComponent(playerId, ComponentType.WorldPosition) as WorldPositionComponent).position;

                            if (InputSystem.newGamepadState.IsConnected)
                            {
                                Vector2 aim = InputSystem.newGamepadState.ThumbSticks.Left * selectedItem.maxRange;
                                aim.Y *= -1;
                                aimComponent.angle = (float)Math.Atan2(aim.Y, aim.X);
                                aimComponent.length = aim.Length();
                            }
                            else
                            {
                                Vector2 relative = (InputSystem.worldMouse - worldPosition);
                                aimComponent.angle = (float)Math.Atan2(relative.Y, relative.X);
                                aimComponent.length = Math.Min(relative.Length(), selectedItem.maxRange);
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
                                case ItemType.RopeGun:
                                    if (selectedItem.primaryAction)
                                    {
                                        AimComponent aimComponent = _entityManager.getComponent(toolbarEntities[i], ComponentType.Aim) as AimComponent;
                                        Vector2 initialPointA = (_entityManager.getComponent(toolbarEntities[i], ComponentType.WorldPosition) as WorldPositionComponent).position;
                                        Vector2 initialPointB = initialPointA + new Vector2((float)Math.Cos(aimComponent.angle), (float)Math.Sin(aimComponent.angle)) * aimComponent.length;
                                        int ropeEntityId = _entityManager.factory.createRope(false, initialPointA, initialPointB, playerPhysicsComponent.body.GetLinearVelocity(), -1);

                                        if (ropeEntityId != -1)
                                        {
                                            RopeGrabComponent ropeGrabComponent = _entityManager.getComponent(toolbarComponent.entityId, ComponentType.RopeGrab) as RopeGrabComponent;
                                            RopePhysicsComponent ropePhysicsComponent = _entityManager.getComponent(ropeEntityId, ComponentType.RopePhysics) as RopePhysicsComponent;
                                            PhysicsComponent physicsComponent = _entityManager.getComponent(toolbarEntities[i], ComponentType.Physics) as PhysicsComponent;
                                            RopeGrabComponent newRopeGrabComponent = null;

                                            if (physicsComponent == null)
                                                break;

                                            if (ropeGrabComponent != null)
                                            {
                                                _ropeSystem.releaseRope(ropeGrabComponent, physicsComponent.body);
                                                (_entityManager.getComponent(ropeGrabComponent.ropeEntityId, ComponentType.RopePhysics) as RopePhysicsComponent).timeToLive = 100;
                                                _entityManager.removeComponent(toolbarComponent.entityId, ropeGrabComponent);
                                                ropeGrabComponent = null;
                                            }

                                            newRopeGrabComponent = new RopeGrabComponent(ropeEntityId, ropePhysicsComponent.ropeNodeHead);
                                            _ropeSystem.grabRope(newRopeGrabComponent, physicsComponent.body, newRopeGrabComponent.distance);

                                            _entityManager.addComponent(toolbarComponent.entityId, newRopeGrabComponent);

                                            /*
                                            // TEMPORARY -- Pause after rope creation
                                            SystemNode node = _systemManager.head;
                                            while (node != null)
                                            {
                                                if (node.system.systemType != SystemType.Render)
                                                {
                                                    node.system.paused = !node.system.paused;
                                                }

                                                node = node.next;
                                            }
                                            */
                                        }
                                    }
                                    break;

                                case ItemType.Blueprint:
                                    Console.WriteLine("Blueprint");
                                    break;

                                case ItemType.BlueprintScrap:
                                    Console.WriteLine("Blueprint scrap");
                                    break;
                            }

                            selectedItem.primaryAction = false;
                            selectedItem.secondaryAction = false;
                        }
                    }
                }

                _singleStep = false;
            }
        }
    }
}
