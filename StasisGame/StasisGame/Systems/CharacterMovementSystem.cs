using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class CharacterMovementSystem : ISystem
    {
        public const float WALK_SPEED = 7;
        public const float WALK_FORCE = 12;
        public const float JUMP_FORCE = 12;
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        public int defaultPriority { get { return 40; } }
        public SystemType systemType { get { return SystemType.CharacterMovement; } }

        public CharacterMovementSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update()
        {
            List<int> characterEntities = _entityManager.getEntitiesPosessing(ComponentType.CharacterMovement);

            for (int i = 0; i < characterEntities.Count; i++)
            {
                int entityId = characterEntities[i];
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(entityId, ComponentType.Physics);
                CharacterMovementComponent characterMovementComponent = (CharacterMovementComponent)_entityManager.getComponent(entityId, ComponentType.CharacterMovement);
                Vector2 averageNormal = Vector2.Zero;
                float walkForce = 0;

                characterMovementComponent.calculateMovementAngle();

                //if (!allowLeftMovement && !allowRightMovement)
                //    return;

                bool applyForce = true;
                float modifier = 1;

                // Check speed limit
                if (Math.Abs(physicsComponent.body.GetLinearVelocity().Length()) > WALK_SPEED)
                    applyForce = false;

                // Pull harder if grabbing
                //if (grabbing)
                //    modifier = 2;

                // Apply movement force
                if (characterMovementComponent.walkLeft)
                    walkForce = -WALK_FORCE;
                else if (characterMovementComponent.walkRight)
                    walkForce = WALK_FORCE;

                if (characterMovementComponent.walkLeft || characterMovementComponent.walkRight)
                {
                    Vector2 force = new Vector2((float)Math.Cos(characterMovementComponent.movementAngle), (float)Math.Sin(characterMovementComponent.movementAngle)) * walkForce * modifier;
                    physicsComponent.body.ApplyForce(force, physicsComponent.body.GetPosition());
                }
                //if (applyForce && dir == Direction.LEFT && allowLeftMovement)
                //    body.ApplyForce(new Vector2(getGroundAngleX() * -WALK_FORCE * modifier, getGroundAngleY() * -WALK_FORCE * modifier), body.GetPosition());
                //else if (applyForce && dir == Direction.RIGHT && allowRightMovement)
                //    body.ApplyForce(new Vector2(getGroundAngleX() * WALK_FORCE * modifier, getGroundAngleY() * WALK_FORCE * modifier), body.GetPosition());

                characterMovementComponent.movementNormals.Clear();
            }
        }
    }
}
