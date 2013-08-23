using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using StasisCore;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class PhysicsSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private float _dt = 1f / 60f;
        private List<Body> _bodiesToRemove;
        private bool _paused;
        private bool _singleStep;
        private PlayerSystem _playerSystem;
        private Dictionary<string, World> _worlds;
        private Dictionary<string, Body> _groundBodies;

        public int defaultPriority { get { return 20; } }
        public SystemType systemType { get { return SystemType.Physics; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public PhysicsSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _bodiesToRemove = new List<Body>();
            _playerSystem = (PlayerSystem)systemManager.getSystem(SystemType.Player);
            _worlds = new Dictionary<string, World>();
            _groundBodies = new Dictionary<string, Body>();
            /*
            // Create world
            _world = new World(gravity);

            // Contact callbacks
            _world.ContactManager.BeginContact += new BeginContactDelegate(BeginContact);
            _world.ContactManager.EndContact += new EndContactDelegate(EndContact);
            _world.ContactManager.PreSolve += new PreSolveDelegate(PreSolve);
            _world.ContactManager.PostSolve += new PostSolveDelegate(PostSolve);

            // Create ground body/entity
            _groundBody = _entityManager.factory.createGroundBody(_world);*/
        }

        public void addWorld(string levelUid, Vector2 gravity)
        {
            World world = new World(gravity);

            world.ContactManager.BeginContact += new BeginContactDelegate(BeginContact);
            world.ContactManager.EndContact += new EndContactDelegate(EndContact);
            world.ContactManager.PreSolve += new PreSolveDelegate(PreSolve);
            world.ContactManager.PostSolve += new PostSolveDelegate(PostSolve);
            _worlds.Add(levelUid, world);
            _groundBodies.Add(levelUid, _entityManager.factory.createGroundBody(levelUid, _worlds[levelUid]));
        }

        public World getWorld(string levelUid)
        {
            return _worlds[levelUid];
        }

        public void removeBody(Body body)
        {
            _bodiesToRemove.Add(body);
        }

        public void update(GameTime gameTime)
        {
            if (_singleStep || !_paused)
            {
                LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;

                if (levelSystem.finalized)
                {
                    string levelUid = LevelSystem.currentLevelUid;
                    EventSystem eventSystem = _systemManager.getSystem(SystemType.Event) as EventSystem;
                    List<CharacterMovementComponent> movementComponents = _entityManager.getComponents<CharacterMovementComponent>(levelUid, ComponentType.CharacterMovement);
                    List<int> ropeGrabEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.RopeGrab);
                    List<int> prismaticEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Prismatic);
                    List<int> physicsEntities;

                    for (int i = 0; i < _bodiesToRemove.Count; i++)
                    {
                        getWorld(levelUid).RemoveBody(_bodiesToRemove[i]);
                    }
                    _bodiesToRemove.Clear();

                    for (int i = 0; i < movementComponents.Count; i++)
                    {
                        CharacterMovementComponent movementComponent = movementComponents[i];

                        movementComponent.collisionNormals.Clear();
                    }

                    for (int i = 0; i < prismaticEntities.Count; i++)
                    {
                        PrismaticJointComponent prismaticJointComponent = _entityManager.getComponent(levelUid, prismaticEntities[i], ComponentType.Prismatic) as PrismaticJointComponent;
                        LimitState limitState = prismaticJointComponent.prismaticJoint.LimitState;

                        if (prismaticJointComponent.previousLimitState != limitState)
                        {
                            if (limitState == LimitState.AtLower)
                            {
                                eventSystem.postEvent(new GameEvent(GameEventType.OnLowerLimitReached, prismaticEntities[i]));
                            }
                            else if (limitState == LimitState.AtUpper)
                            {
                                //eventSystem.postEvent(new GameEvent(GameEventType.OnUpperLimitReached, prismaticEntities[i]));
                            }
                        }

                        prismaticJointComponent.previousLimitState = limitState;
                    }

                    getWorld(levelUid).Step(_dt);

                    // Handle physic entities
                    physicsEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Physics);
                    for (int i = 0; i < physicsEntities.Count; i++)
                    {
                        PhysicsComponent physicsComponent = _entityManager.getComponent(levelUid, physicsEntities[i], ComponentType.Physics) as PhysicsComponent;
                        WorldPositionComponent worldPositionComponent = _entityManager.getComponent(levelUid, physicsEntities[i], ComponentType.WorldPosition) as WorldPositionComponent;
                        FollowMetamerComponent followMetamerComponent = _entityManager.getComponent(levelUid, physicsEntities[i], ComponentType.FollowMetamer) as FollowMetamerComponent;

                        // Set body position to the metamer being followed
                        if (followMetamerComponent != null)
                        {
                            physicsComponent.body.Position = followMetamerComponent.metamer.position;
                            physicsComponent.body.Rotation = followMetamerComponent.metamer.currentAngle + StasisMathHelper.halfPi;
                        }

                        // Update world position component
                        worldPositionComponent.position = physicsComponent.body.Position;
                    }
                }
            }
            _singleStep = false;
        }

        public void PreSolve(Contact contact, ref Manifold manifold)
        {
            EventSystem eventSystem = (EventSystem)_systemManager.getSystem(SystemType.Event);
            Fixture fixtureA = contact.FixtureA;
            Fixture fixtureB = contact.FixtureB;
            int entityA = (int)fixtureA.Body.UserData;
            int entityB = (int)fixtureB.Body.UserData;
            int playerId = _playerSystem.playerId;
            string levelUid = LevelSystem.currentLevelUid;

            // Check for custom collision filters
            bool fixtureAIgnoresEntityB = fixtureA.IsIgnoredEntity(entityB);
            bool fixtureBIgnoresEntityA = fixtureB.IsIgnoredEntity(entityA);
            if (fixtureAIgnoresEntityB)
                contact.Enabled = false;
            else if (fixtureBIgnoresEntityA)
                contact.Enabled = false;

            // Check for item pickup
            if (contact.IsTouching() && (entityA == playerId || entityB == playerId))
            {
                int itemEntityId = entityA == playerId ? entityB : entityA;
                Fixture fixture = entityA == playerId ? fixtureB : fixtureA;
                ItemComponent itemComponent = _entityManager.getComponent(levelUid, itemEntityId, ComponentType.Item) as ItemComponent;

                if (itemComponent != null)
                {
                    contact.Enabled = false;
                    if (itemComponent.state.inWorld)
                    {
                        InventoryComponent playerInventory = _entityManager.getComponent(levelUid, playerId, ComponentType.Inventory) as InventoryComponent;
                        EquipmentSystem equipmentSystem = _systemManager.getSystem(SystemType.Equipment) as EquipmentSystem;

                        equipmentSystem.addInventoryItem(playerInventory, itemComponent);
                        itemComponent.state.inWorld = false;
                        _bodiesToRemove.Add(fixture.Body);
                        _entityManager.killEntity(levelUid, itemEntityId);
                        eventSystem.postEvent(new GameEvent(GameEventType.OnItemPickedUp, itemEntityId));
                    }
                }
            }
        }

        public void PostSolve(Contact contact, ContactConstraint contactConstraint)
        {
            string levelUid = LevelSystem.currentLevelUid;
            List<int> characterEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.CharacterMovement);
            int entityAId = (int)contact.FixtureA.Body.UserData;
            int entityBId = (int)contact.FixtureB.Body.UserData;
            CharacterMovementComponent characterMovementComponent = null;
            FixedArray2<Vector2> points;
            Vector2 normal;

            characterMovementComponent = (_entityManager.getComponent(levelUid, entityAId, ComponentType.CharacterMovement) ?? _entityManager.getComponent(levelUid, entityBId, ComponentType.CharacterMovement)) as CharacterMovementComponent;
            if (characterMovementComponent != null)
            {
                if (contact.FixtureA == characterMovementComponent.feetFixture || contact.FixtureB == characterMovementComponent.feetFixture)
                {
                    contact.GetWorldManifold(out normal, out points);
                    characterMovementComponent.collisionNormals.Add(normal);
                    //if (characterMovementComponent.allowJumpResetOnCollision)
                    //    characterMovementComponent.alreadyJumped = false;
                }
            }
        }

        public bool BeginContact(Contact contact)
        {
            string levelUid = LevelSystem.currentLevelUid;
            List<int> levelGoalEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.RegionGoal);
            List<int> explosionEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Explosion);
            LevelSystem levelSystem = (LevelSystem)_systemManager.getSystem(SystemType.Level);
            ExplosionSystem explosionSystem = (ExplosionSystem)_systemManager.getSystem(SystemType.Explosion);

            // See if player is touching a level goal
            if (levelGoalEntities.Count > 0)
            {
                int entityA = (int)contact.FixtureA.Body.UserData;
                int entityB = (int)contact.FixtureB.Body.UserData;

                if (entityA == _playerSystem.playerId)
                {
                    if (levelGoalEntities.Contains(entityB))
                        levelSystem.completeRegionGoal(entityB);
                }
                else if (entityB == _playerSystem.playerId)
                {
                    if (levelGoalEntities.Contains(entityA))
                        levelSystem.completeRegionGoal(entityA);
                }
            }

            // Explosions
            if (explosionEntities.Count > 0)
            {
                int entityA = (int)contact.FixtureA.Body.UserData;
                int entityB = (int)contact.FixtureB.Body.UserData;
                IComponent component = null;
                ExplosionComponent explosionComponent = null;
                Fixture targetFixture = null;
                FixedArray2<Vector2> points;

                if (_entityManager.tryGetComponent(levelUid, entityA, ComponentType.Explosion, out component))
                    targetFixture = contact.FixtureB;
                else if (_entityManager.tryGetComponent(levelUid, entityB, ComponentType.Explosion, out component))
                    targetFixture = contact.FixtureA;

                if (targetFixture != null && component != null)
                {
                    DestructibleGeometryComponent destructibleGeometryComponent = (DestructibleGeometryComponent)_entityManager.getComponent(levelUid, (int)targetFixture.Body.UserData, ComponentType.DestructibleGeometry);
                    Vector2 contactNormal;
                    Vector2 relative;
                    Vector2 force;
                    float distance;

                    //contact.GetWorldManifold(out worldManifold);
                    contact.GetWorldManifold(out contactNormal, out points);
                    explosionComponent = (ExplosionComponent)component;
                    relative = (targetFixture.Shape.Center + targetFixture.Body.Position) - explosionComponent.position;
                    distance = Math.Max(relative.Length(), 0.1f);
                    force = relative * (1 / distance) * explosionComponent.strength;

                    if (destructibleGeometryComponent != null)
                    {
                        // Break fixture off from body
                        explosionSystem.breakFixture(targetFixture, force, 180);
                        return false;
                    }
                    else
                    {
                        // Apply generic explosion force
                        targetFixture.Body.ApplyForce(force, points[0]);
                        return false;
                    }
                }
            }

            return true;
        }

        public void EndContact(Contact contact)
        {
        }
    }
}
