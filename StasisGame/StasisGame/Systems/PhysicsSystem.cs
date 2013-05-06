using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using StasisCore;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class PhysicsSystem : ISystem, IContactListener
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private World _world;
        private float _dt = 1f / 60f;
        private Body _groundBody;
        private List<Body> _bodiesToRemove;
        private bool _paused;
        private bool _singleStep;
        private PlayerSystem _playerSystem;

        public int defaultPriority { get { return 20; } }
        public SystemType systemType { get { return SystemType.Physics; } }
        public World world { get { return _world; } }
        public Body groundBody { get { return _groundBody; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public PhysicsSystem(SystemManager systemManager, EntityManager entityManager, XElement data)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _bodiesToRemove = new List<Body>();
            _playerSystem = (PlayerSystem)systemManager.getSystem(SystemType.Player);

            BodyDef groundBodyDef = new BodyDef();
            CircleShape circleShape = new CircleShape();
            FixtureDef fixtureDef = new FixtureDef();
            int groundId = _entityManager.createEntity(10000);

            // Create world
            _world = new World(Loader.loadVector2(data.Attribute("gravity"), new Vector2(0, 32)), true);
            _world.ContactListener = this;

            // Create ground body/entity
            groundBodyDef.type = BodyType.Static;
            groundBodyDef.userData = groundId;
            circleShape._radius = 0.1f;
            fixtureDef.isSensor = true;
            fixtureDef.shape = circleShape;
            _groundBody = world.CreateBody(groundBodyDef);
            _groundBody.CreateFixture(fixtureDef);
            _entityManager.addComponent(groundId, new GroundBodyComponent(_groundBody));
        }

        public void removeBody(Body body)
        {
            _bodiesToRemove.Add(body);
        }

        public void update()
        {
            if (_singleStep || !_paused)
            {
                EventSystem eventSystem = _systemManager.getSystem(SystemType.Event) as EventSystem;
                List<CharacterMovementComponent> movementComponents = _entityManager.getComponents<CharacterMovementComponent>(ComponentType.CharacterMovement);
                List<int> ropeGrabEntities = _entityManager.getEntitiesPosessing(ComponentType.RopeGrab);
                List<int> prismaticEntities = _entityManager.getEntitiesPosessing(ComponentType.Prismatic);
                List<int> physicsEntities;

                for (int i = 0; i < _bodiesToRemove.Count; i++)
                {
                    _world.DestroyBody(_bodiesToRemove[i]);
                }
                _bodiesToRemove.Clear();

                for (int i = 0; i < movementComponents.Count; i++)
                {
                    CharacterMovementComponent movementComponent = movementComponents[i];
                    if (!movementComponent.onSurface)
                    {
                        movementComponent.allowJumpResetOnCollision = true;
                    }
                }

                for (int i = 0; i < prismaticEntities.Count; i++)
                {
                    PrismaticJointComponent prismaticJointComponent = _entityManager.getComponent(prismaticEntities[i], ComponentType.Prismatic) as PrismaticJointComponent;
                    LimitState limitState = prismaticJointComponent.prismaticJoint._limitState;

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

                _world.Step(_dt, 12, 8);

                // Update world positions
                physicsEntities = _entityManager.getEntitiesPosessing(ComponentType.Physics);
                for (int i = 0; i < physicsEntities.Count; i++)
                {
                    PhysicsComponent physicsComponent = _entityManager.getComponent(physicsEntities[i], ComponentType.Physics) as PhysicsComponent;
                    WorldPositionComponent worldPositionComponent = _entityManager.getComponent(physicsEntities[i], ComponentType.WorldPosition) as WorldPositionComponent;

                    worldPositionComponent.position = physicsComponent.body.GetPosition();
                }

                _singleStep = false;
            }
        }

        public void PreSolve(Contact contact, ref Manifold manifold)
        {
            EventSystem eventSystem = (EventSystem)_systemManager.getSystem(SystemType.Event);
            Fixture fixtureA = contact.GetFixtureA();
            Fixture fixtureB = contact.GetFixtureB();
            int entityA = (int)fixtureA.GetBody().GetUserData();
            int entityB = (int)fixtureB.GetBody().GetUserData();
            int playerId = _playerSystem.playerId;

            // Check for custom collision filters
            bool fixtureAIgnoresEntityB = fixtureA.IsIgnoredEntity(entityB);
            bool fixtureBIgnoresEntityA = fixtureB.IsIgnoredEntity(entityA);
            if (fixtureAIgnoresEntityB)
                contact.SetEnabled(false);
            else if (fixtureBIgnoresEntityA)
                contact.SetEnabled(false);

            // Check for item pickup
            if (contact.IsTouching() && (entityA == playerId || entityB == playerId))
            {
                int itemEntityId = entityA == playerId ? entityB : entityA;
                Fixture fixture = entityA == playerId ? fixtureB : fixtureA;
                ItemComponent itemComponent = _entityManager.getComponent(itemEntityId, ComponentType.Item) as ItemComponent;

                if (itemComponent != null)
                {
                    contact.SetEnabled(false);
                    if (itemComponent.inWorld)
                    {
                        InventoryComponent playerInventory = _entityManager.getComponent(playerId, ComponentType.Inventory) as InventoryComponent;
                        playerInventory.addItem(itemComponent);
                        itemComponent.inWorld = false;
                        _bodiesToRemove.Add(fixture.GetBody());
                        _entityManager.killEntity(itemEntityId);
                        eventSystem.postEvent(new GameEvent(GameEventType.OnItemPickedUp, itemEntityId));
                    }
                }
            }
        }

        public void PostSolve(Contact contact, ref ContactImpulse impulse)
        {
            List<int> characterEntities = _entityManager.getEntitiesPosessing(ComponentType.CharacterMovement);
            int entityAId = (int)contact.GetFixtureA().GetBody().GetUserData();
            int entityBId = (int)contact.GetFixtureB().GetBody().GetUserData();
            WorldManifold worldManifold;
            CharacterMovementComponent characterMovementComponent = null;

            characterMovementComponent = (_entityManager.getComponent(entityAId, ComponentType.CharacterMovement) ?? _entityManager.getComponent(entityBId, ComponentType.CharacterMovement)) as CharacterMovementComponent;
            if (characterMovementComponent != null)
            {
                if (contact.GetFixtureA() == characterMovementComponent.feetFixture || contact.GetFixtureB() == characterMovementComponent.feetFixture)
                {
                    contact.GetWorldManifold(out worldManifold);
                    characterMovementComponent.collisionNormals.Add(worldManifold._normal);
                    if (characterMovementComponent.allowJumpResetOnCollision)
                        characterMovementComponent.alreadyJumped = false;
                }
            }
        }

        public void BeginContact(Contact contact)
        {
            List<int> levelGoalEntities = _entityManager.getEntitiesPosessing(ComponentType.RegionGoal);
            LevelSystem levelSystem = (LevelSystem)_systemManager.getSystem(SystemType.Level);

            // See if player is touching a level goal
            if (levelGoalEntities.Count > 0)
            {
                int entityA = (int)contact.GetFixtureA().GetBody().GetUserData();
                int entityB = (int)contact.GetFixtureB().GetBody().GetUserData();

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
        }

        public void EndContact(Contact contact)
        {
        }
    }
}
