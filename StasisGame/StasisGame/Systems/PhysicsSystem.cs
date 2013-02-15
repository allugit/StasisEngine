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

        public World world { get { return _world; } }
        public int defaultPriority { get { return 20; } }
        public SystemType systemType { get { return SystemType.Physics; } }

        public PhysicsSystem(SystemManager systemManager, EntityManager entityManager, XElement data)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;

            // Create world
            _world = new World(Loader.loadVector2(data.Attribute("gravity"), new Vector2(0, 32)), true);
            _world.ContactListener = this;
        }

        public void update()
        {
            List<CharacterMovementComponent> movementComponents = _entityManager.getComponents<CharacterMovementComponent>(ComponentType.CharacterMovement);

            for (int i = 0; i < movementComponents.Count; i++)
            {
                CharacterMovementComponent movementComponent = movementComponents[i];
                if (!movementComponent.onSurface)
                {
                    movementComponent.allowJumpResetOnCollision = true;
                }
            }

            _world.Step(_dt, 12, 8);
        }

        public void PreSolve(Contact contact, ref Manifold manifold)
        {
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
