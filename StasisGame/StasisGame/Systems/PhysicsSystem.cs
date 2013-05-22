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

            //BodyDef groundBodyDef = new BodyDef();
            //CircleShape circleShape = new CircleShape();
            //FixtureDef fixtureDef = new FixtureDef();

            // Create world
            _world = new World(Loader.loadVector2(data.Attribute("gravity"), new Vector2(0, 32)));
            //_world.ContactListener = this;

            // Contact callbacks
            _world.ContactManager.BeginContact += new BeginContactDelegate(BeginContact);
            _world.ContactManager.EndContact += new EndContactDelegate(EndContact);
            _world.ContactManager.PreSolve += new PreSolveDelegate(PreSolve);
            _world.ContactManager.PostSolve += new PostSolveDelegate(PostSolve);

            // Create ground body/entity
            _groundBody = _entityManager.factory.createGroundBody(_world);
            /*
            groundBodyDef.type = BodyType.Static;
            groundBodyDef.userData = groundId;
            circleShape._radius = 0.1f;
            fixtureDef.isSensor = true;
            fixtureDef.shape = circleShape;
            _groundBody = world.CreateBody(groundBodyDef);
            _groundBody.CreateFixture(fixtureDef);
            _entityManager.addComponent(groundId, new GroundBodyComponent(_groundBody));
            _entityManager.addComponent(groundId, new SkipFluidResolutionComponent());
            */
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
                    _world.RemoveBody(_bodiesToRemove[i]);
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

                _world.Step(_dt);

                // Update world positions
                physicsEntities = _entityManager.getEntitiesPosessing(ComponentType.Physics);
                for (int i = 0; i < physicsEntities.Count; i++)
                {
                    PhysicsComponent physicsComponent = _entityManager.getComponent(physicsEntities[i], ComponentType.Physics) as PhysicsComponent;
                    WorldPositionComponent worldPositionComponent = _entityManager.getComponent(physicsEntities[i], ComponentType.WorldPosition) as WorldPositionComponent;

                    worldPositionComponent.position = physicsComponent.body.Position;
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
                ItemComponent itemComponent = _entityManager.getComponent(itemEntityId, ComponentType.Item) as ItemComponent;

                if (itemComponent != null)
                {
                    contact.Enabled = false;
                    if (itemComponent.inWorld)
                    {
                        InventoryComponent playerInventory = _entityManager.getComponent(playerId, ComponentType.Inventory) as InventoryComponent;
                        playerInventory.addItem(itemComponent);
                        itemComponent.inWorld = false;
                        _bodiesToRemove.Add(fixture.Body);
                        _entityManager.killEntity(itemEntityId);
                        eventSystem.postEvent(new GameEvent(GameEventType.OnItemPickedUp, itemEntityId));
                    }
                }
            }
        }

        public void PostSolve(Contact contact, ContactConstraint contactConstraint)
        {
            List<int> characterEntities = _entityManager.getEntitiesPosessing(ComponentType.CharacterMovement);
            int entityAId = (int)contact.FixtureA.Body.UserData;
            int entityBId = (int)contact.FixtureB.Body.UserData;
            CharacterMovementComponent characterMovementComponent = null;
            FixedArray2<Vector2> points;
            Vector2 normal;

            characterMovementComponent = (_entityManager.getComponent(entityAId, ComponentType.CharacterMovement) ?? _entityManager.getComponent(entityBId, ComponentType.CharacterMovement)) as CharacterMovementComponent;
            if (characterMovementComponent != null)
            {
                if (contact.FixtureA == characterMovementComponent.feetFixture || contact.FixtureB == characterMovementComponent.feetFixture)
                {
                    contact.GetWorldManifold(out normal, out points);
                    characterMovementComponent.collisionNormals.Add(normal);
                    if (characterMovementComponent.allowJumpResetOnCollision)
                        characterMovementComponent.alreadyJumped = false;
                }
            }
        }

        public bool BeginContact(Contact contact)
        {
            List<int> levelGoalEntities = _entityManager.getEntitiesPosessing(ComponentType.RegionGoal);
            List<int> explosionEntities = _entityManager.getEntitiesPosessing(ComponentType.Explosion);
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
                Vector2 relative;
                Vector2 force;
                float distanceSq;
                FixedArray2<Vector2> points;

                if (_entityManager.tryGetComponent(entityA, ComponentType.Explosion, out component))
                    targetFixture = contact.FixtureB;
                else if (_entityManager.tryGetComponent(entityB, ComponentType.Explosion, out component))
                    targetFixture = contact.FixtureA;

                if (targetFixture != null && component != null)
                {
                    DestructibleGeometryComponent destructibleGeometryComponent = (DestructibleGeometryComponent)_entityManager.getComponent((int)targetFixture.Body.UserData, ComponentType.DestructibleGeometry);
                    Vector2 contactNormal;

                    //contact.GetWorldManifold(out worldManifold);
                    contact.GetWorldManifold(out contactNormal, out points);
                    explosionComponent = (ExplosionComponent)component;
                    relative = targetFixture.Body.Position - explosionComponent.position;
                    distanceSq = relative.LengthSquared();
                    relative.Normalize();
                    force = relative * (explosionComponent.strength / Math.Max(distanceSq, 0.1f));

                    if (destructibleGeometryComponent != null)
                    {
                        // Break fixture off from body
                        explosionSystem.breakFixture(targetFixture, force, 180);
                    }
                    else
                    {
                        // Apply generic explosion force
                        targetFixture.Body.ApplyForce(force, points[0]);
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
