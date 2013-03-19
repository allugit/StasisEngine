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

        public int defaultPriority { get { return 20; } }
        public SystemType systemType { get { return SystemType.Physics; } }
        public World world { get { return _world; } }
        public Body groundBody { get { return _groundBody; } }

        public PhysicsSystem(SystemManager systemManager, EntityManager entityManager, XElement data)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _bodiesToRemove = new List<Body>();

            BodyDef groundBodyDef = new BodyDef();
            CircleShape circleShape = new CircleShape();
            FixtureDef fixtureDef = new FixtureDef();
            int groundId = _entityManager.createEntity();

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
            EventSystem eventSystem = _systemManager.getSystem(SystemType.Event) as EventSystem;
            List<CharacterMovementComponent> movementComponents = _entityManager.getComponents<CharacterMovementComponent>(ComponentType.CharacterMovement);
            List<int> prismaticEntities = _entityManager.getEntitiesPosessing(ComponentType.Prismatic);

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
        }

        public void PreSolve(Contact contact, ref Manifold manifold)
        {
            Fixture fixtureA = contact.GetFixtureA();
            Fixture fixtureB = contact.GetFixtureB();
            int entityA = (int)fixtureA.GetBody().GetUserData();
            int entityB = (int)fixtureB.GetBody().GetUserData();
            int playerId = (_systemManager.getSystem(SystemType.Player) as PlayerSystem).playerId;

            // Check for custom collision filters
            if (fixtureA.IsIgnoredEntity(entityB))
                contact.SetEnabled(false);
            else if (fixtureB.IsIgnoredEntity(entityA))
                contact.SetEnabled(false);

            // Check for item pickup
            if (contact.IsTouching() && (entityA == playerId || entityB == playerId))
            {
                ItemComponent itemComponent = _entityManager.getComponent(entityA, ComponentType.Item) as ItemComponent;
                if (itemComponent != null && itemComponent.inWorld)
                {
                    InventoryComponent playerInventory = _entityManager.getComponent(playerId, ComponentType.Inventory) as InventoryComponent;
                    playerInventory.addItem(itemComponent);
                    itemComponent.inWorld = false;
                    _bodiesToRemove.Add(fixtureA.GetBody());
                    _entityManager.killEntity(entityA);
                }
                else
                {
                    itemComponent = _entityManager.getComponent(entityB, ComponentType.Item) as ItemComponent;
                    if (itemComponent != null && itemComponent.inWorld)
                    {
                        InventoryComponent playerInventory = _entityManager.getComponent(playerId, ComponentType.Inventory) as InventoryComponent;
                        playerInventory.addItem(itemComponent);
                        itemComponent.inWorld = false;
                        _bodiesToRemove.Add(fixtureB.GetBody());
                        _entityManager.killEntity(entityB);
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
        }

        public void EndContact(Contact contact)
        {
        }
    }
}
