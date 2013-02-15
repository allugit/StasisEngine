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
        public const float MAX_WALK_SPEED = 7;
        public const float WALK_FORCE = 14;
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
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(characterEntities[i], ComponentType.Physics);
                CharacterMovementComponent characterMovementComponent = (CharacterMovementComponent)_entityManager.getComponent(characterEntities[i], ComponentType.CharacterMovement);
                Vector2 averageNormal = Vector2.Zero;
                float walkForce = 0;
                float modifier = 1;
                bool movingOppositeFromWalkDirection = false;
                Vector2 characterVelocity = physicsComponent.body.GetLinearVelocity();
                float characterSpeed = characterVelocity.Length();
                float characterHorizontalSpeed = Math.Abs(characterVelocity.X);

                if (characterHorizontalSpeed > 0.1f)
                {
                    movingOppositeFromWalkDirection =
                        (characterMovementComponent.walkLeft && characterVelocity.X > 0) ||
                        (characterMovementComponent.walkRight && characterVelocity.X < 0);
                }

                characterMovementComponent.calculateMovementAngle();

                if (characterMovementComponent.walkLeft)
                    walkForce = -WALK_FORCE;
                else if (characterMovementComponent.walkRight)
                    walkForce = WALK_FORCE;

                if ((characterMovementComponent.walkLeft || characterMovementComponent.walkRight) && characterHorizontalSpeed < MAX_WALK_SPEED)
                {
                    Vector2 force = new Vector2((float)Math.Cos(characterMovementComponent.movementAngle), (float)Math.Sin(characterMovementComponent.movementAngle)) * walkForce * modifier;
                    physicsComponent.body.ApplyForce(force, physicsComponent.body.GetPosition());
                }
                else if (!(characterMovementComponent.walkLeft || characterMovementComponent.walkRight) || movingOppositeFromWalkDirection)
                {
                    if (characterMovementComponent.onSurface)
                    {
                        //Vector2 antiForce = new Vector2(-physicsComponent.body.GetLinearVelocity().X * 10, 0);
                        Vector2 antiForce = -characterVelocity * 10;
                        physicsComponent.body.ApplyForce(antiForce, physicsComponent.body.GetPosition());
                    }
                }

                characterMovementComponent.collisionNormals.Clear();
            }
        }
    }
}
