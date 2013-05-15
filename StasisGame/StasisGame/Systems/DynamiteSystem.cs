using System;
using System.Collections.Generic;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class DynamiteSystem : ISystem
    {
        private bool _singleStep;
        private bool _paused;
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public SystemType systemType { get { return SystemType.Dynamite; } }
        public int defaultPriority { get { return 40; } }

        public DynamiteSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        // explodeDynamite -- Creates an explosion entity and kills the dynamite entity
        public void explodeDynamite(int entityId, DynamiteComponent dynamiteComponent)
        {
            WorldPositionComponent worldPositionComponent = (WorldPositionComponent)_entityManager.getComponent(entityId, ComponentType.WorldPosition);

            _entityManager.killEntity(entityId);
            _entityManager.factory.createExplosion(worldPositionComponent.position, dynamiteComponent.strength, dynamiteComponent.radius);
        }

        public void update()
        {
            List<int> dynamiteEntities = _entityManager.getEntitiesPosessing(ComponentType.Dynamite);
            List<int> explosionEntities = _entityManager.getEntitiesPosessing(ComponentType.Explosion);

            // Dynamite entities
            for (int i = 0; i < dynamiteEntities.Count; i++)
            {
                DynamiteComponent dynamiteComponent = (DynamiteComponent)_entityManager.getComponent(dynamiteEntities[i], ComponentType.Dynamite);

                if (dynamiteComponent.timeToLive > 0)
                    dynamiteComponent.timeToLive--;
                else
                    explodeDynamite(dynamiteEntities[i], dynamiteComponent);
            }

            // Explosion entities
            for (int i = 0; i < explosionEntities.Count; i++)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(explosionEntities[i], ComponentType.Physics);

                physicsComponent.body.GetWorld().DestroyBody(physicsComponent.body);
                _entityManager.killEntity(explosionEntities[i]);
            }
        }
    }
}
