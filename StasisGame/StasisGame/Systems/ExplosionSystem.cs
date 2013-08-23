using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
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
        public void explodeDynamite(string levelUid, int entityId, DynamiteComponent dynamiteComponent)
        {
            //WorldPositionComponent worldPositionComponent = (WorldPositionComponent)_entityManager.getComponent(entityId, ComponentType.WorldPosition);
            PhysicsComponent physicsComponent = _entityManager.getComponent(levelUid, entityId, ComponentType.Physics) as PhysicsComponent;
            Vector2 position = physicsComponent.body.Position;

            physicsComponent.body.World.RemoveBody(physicsComponent.body);
            _entityManager.killEntity(levelUid, entityId);
            _entityManager.factory.createExplosion(levelUid, position, dynamiteComponent.strength, dynamiteComponent.radius);
        }

        // breakFixture -- Breaks off a fixture from a body and create debris
        public void breakFixture(Fixture fixture, Vector2 force, int timeToLive)
        {
            _debrisToCreate.Add(new DebrisProperties(fixture, force, timeToLive));
        }

        // killDebris -- Handles removal of debris entities
        public void killDebris(string levelUid, int entityId)
        {
            PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(levelUid, entityId, ComponentType.Physics);

            physicsComponent.body.World.RemoveBody(physicsComponent.body);
            _entityManager.killEntity(levelUid, entityId);
        }

        public void update(GameTime gameTime)
        {
            if (!_paused || _singleStep)
            {
                LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;

                if (levelSystem.finalized)
                {
                    string levelUid = LevelSystem.currentLevelUid;
                    List<int> dynamiteEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Dynamite);
                    List<int> explosionEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Explosion);
                    List<int> debrisEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Debris);

                    // Dynamite entities
                    for (int i = 0; i < dynamiteEntities.Count; i++)
                    {
                        DynamiteComponent dynamiteComponent = (DynamiteComponent)_entityManager.getComponent(levelUid, dynamiteEntities[i], ComponentType.Dynamite);

                        if (dynamiteComponent.timeToLive > 0)
                            dynamiteComponent.timeToLive--;
                        else
                            explodeDynamite(levelUid, dynamiteEntities[i], dynamiteComponent);
                    }

                    // Explosion entities -- Explosion contact logic is handled by the PhysicsSystem's contact listeners. This just removes them, since they only should exist for 1 frame
                    for (int i = 0; i < explosionEntities.Count; i++)
                    {
                        PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(levelUid, explosionEntities[i], ComponentType.Physics);

                        physicsComponent.body.World.RemoveBody(physicsComponent.body);
                        _entityManager.killEntity(levelUid, explosionEntities[i]);
                    }

                    // Break fixtures and create debris
                    for (int i = 0; i < _debrisToCreate.Count; i++)
                    {
                        Fixture fixture = _debrisToCreate[i].fixture;

                        if (fixture.Shape != null)
                        {
                            int entityId = (int)fixture.Body.UserData;
                            PrimitivesRenderComponent primitiveRenderComponent = (PrimitivesRenderComponent)_entityManager.getComponent(levelUid, entityId, ComponentType.PrimitivesRender);
                            PrimitiveRenderObject primitiveRenderObject = primitiveRenderComponent.primitiveRenderObjects[0];
                            RenderableTriangle triangleToRemove = null;

                            for (int j = 0; j < primitiveRenderObject.renderableTriangles.Count; j++)
                            {
                                if (primitiveRenderObject.renderableTriangles[j].fixture == fixture)
                                {
                                    triangleToRemove = primitiveRenderObject.renderableTriangles[j];
                                    break;
                                }
                            }
                            if (triangleToRemove != null)
                                primitiveRenderObject.renderableTriangles.Remove(triangleToRemove);

                            _entityManager.factory.createDebris(levelUid, fixture, _debrisToCreate[i].force, _debrisToCreate[i].timeToLive, triangleToRemove, primitiveRenderObject.texture, primitiveRenderObject.layerDepth);
                            fixture.Body.DestroyFixture(fixture);
                        }
                    }
                    _debrisToCreate.Clear();

                    // Debris
                    for (int i = 0; i < debrisEntities.Count; i++)
                    {
                        DebrisComponent debrisComponent = (DebrisComponent)_entityManager.getComponent(levelUid, debrisEntities[i], ComponentType.Debris);
                        debrisComponent.timeToLive--;

                        if (debrisComponent.restitutionCount < DebrisComponent.RESTITUTION_RESTORE_COUNT)
                            debrisComponent.fixture.Restitution = debrisComponent.fixture.Restitution + debrisComponent.restitutionIncrement;

                        if (debrisComponent.timeToLive < 0)
                            killDebris(levelUid, debrisEntities[i]);
                    }
                }
            }
            _singleStep = false;
        }
    }
}
