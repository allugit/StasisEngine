using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame.Systems
{
    public class AIBehaviorSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private bool _paused;
        private bool _singleStep;
        private LevelSystem _levelSystem;
        private Random _random;

        public SystemType systemType { get { return SystemType.AIBehavior; } }
        public int defaultPriority { get { return 30; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public AIBehaviorSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;
            _random = new Random();
        }

        public WaypointsComponent getWaypointsComponent(string waypointsUid, List<WaypointsComponent> waypointsComponents)
        {
            for (int i = 0; i < waypointsComponents.Count; i++)
            {
                if (waypointsComponents[i].uid == waypointsUid)
                {
                    return waypointsComponents[i];
                }
            }
            return null;
        }

        public void update()
        {
            if (!_paused || _singleStep)
            {
                if (_levelSystem.finalized)
                {
                    string levelUid = LevelSystem.currentLevelUid;
                    List<int> wanderBehaviorEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.AIWanderBehavior);
                    List<WaypointsComponent> waypointsComponents = _entityManager.getComponents<WaypointsComponent>(levelUid, ComponentType.Waypoints);

                    for (int i = 0; i < wanderBehaviorEntities.Count; i++)
                    {
                        int entityId = wanderBehaviorEntities[i];
                        AIWanderBehaviorComponent behaviorComponent = _entityManager.getComponent(levelUid, entityId, ComponentType.AIWanderBehavior) as AIWanderBehaviorComponent;
                        WaypointsComponent waypointsComponent = getWaypointsComponent(behaviorComponent.waypointsUid, waypointsComponents);
                        WorldPositionComponent worldPositionComponent = _entityManager.getComponent(levelUid, entityId, ComponentType.WorldPosition) as WorldPositionComponent;
                        CharacterMovementComponent movementComponent = _entityManager.getComponent(levelUid, entityId, ComponentType.CharacterMovement) as CharacterMovementComponent;
                        Vector2 currentWaypoint = waypointsComponent.waypoints[behaviorComponent.currentWaypointIndex];
                        Vector2 relative = currentWaypoint - worldPositionComponent.position;

                        // Adjust walk speed based on distance from waypoint
                        movementComponent.walkSpeedModifier = Math.Max(0.1f, Math.Min(1f, Math.Abs(relative.X)));
                        Console.WriteLine("relative: {0} | walkSpeed: {1}", relative, movementComponent.walkSpeedModifier);

                        if (relative.X > 0.25f)
                        {
                            movementComponent.walkRight = true;
                            movementComponent.walkLeft = false;
                        }
                        else if (relative.X < -0.25f)
                        {
                            movementComponent.walkLeft = true;
                            movementComponent.walkRight = false;
                        }
                        else
                        {
                            behaviorComponent.currentWaypointIndex = _random.Next(waypointsComponent.waypoints.Count);
                        }
                    }
                }
            }
            _singleStep = false;
        }
    }
}
