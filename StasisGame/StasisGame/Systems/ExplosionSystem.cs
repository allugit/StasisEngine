using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class ExplosionSystem : ISystem
    {
        private struct DebrisProperties
        {
            public Fixture fixture;
            public Vector2 force;
            public int timeToLive;
            public DebrisProperties(Fixture fixture, Vector2 force, int timeToLive)
            {
                this.fixture = fixture;
                this.force = force;
                this.timeToLive = timeToLive;
            }
        }

        private bool _singleStep;
        private bool _paused;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private List<DebrisProperties> _debrisToCreate;

        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public SystemType systemType { get { return SystemType.Explosion; } }
        public int defaultPriority { get { return 40; } }

        public ExplosionSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _debrisToCreate = new List<DebrisProperties>();
        }

        // explodeDynamite -- Creates an explosion entity and kills the dynamite entity
        public void explodeDynamite(int entityId, DynamiteComponent dynamiteComponent)
        {
            WorldPositionComponent worldPositionComponent = (WorldPositionComponent)_entityManager.getComponent(entityId, ComponentType.WorldPosition);

            _entityManager.killEntity(entityId);
            _entityManager.factory.createExplosion(worldPositionComponent.position, dynamiteComponent.strength, dynamiteComponent.radius);
        }

        // breakFixture -- Breaks off a fixture from a body and create debris
        public void breakFixture(Fixture fixture, Vector2 force, int timeToLive)
        {
            _debrisToCreate.Add(new DebrisProperties(fixture, force, timeToLive));
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

            // Explosion entities -- Explosion contact logic is handled by the PhysicsSystem's contact listeners. This just removes them, since they only should exist for 1 frame
            for (int i = 0; i < explosionEntities.Count; i++)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(explosionEntities[i], ComponentType.Physics);

                physicsComponent.body.GetWorld().DestroyBody(physicsComponent.body);
                _entityManager.killEntity(explosionEntities[i]);
            }

            // Break fixtures and create debris
            for (int i = 0; i < _debrisToCreate.Count; i++)
            {
                Fixture fixture = _debrisToCreate[i].fixture;

                if (fixture.GetShape() != null)
                {
                    int hostEntityId = (int)fixture.GetBody().GetUserData();

                    _entityManager.factory.createDebris(fixture, _debrisToCreate[i].force, _debrisToCreate[i].timeToLive);
                    fixture.GetBody().DestroyFixture(fixture);
                }
            }
            _debrisToCreate.Clear();
        }
    }
}
