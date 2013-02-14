using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame.Systems
{
    public class PlayerSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        public const float WALK_SPEED = 7;
        public const float WALK_FORCE = 12;
        public const float JUMP_FORCE = 12;
        private int _playerId;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Player; } }
        public int playerId { get { return _playerId; } set { _playerId = value; } }

        public PlayerSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update()
        {
            InputComponent input = (InputComponent)_entityManager.getComponent(_playerId, ComponentType.Input);
            PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(_playerId, ComponentType.Physics);
            bool walkLeft;
            bool walkRight;

            if (input.newKeyState.IsKeyDown(Keys.A) || input.newKeyState.IsKeyDown(Keys.Left))
            {
                walkLeft = true;
            }

            if (input.newKeyState.IsKeyDown(Keys.D) || input.newKeyState.IsKeyDown(Keys.Right))
            {
                walkRight = true;
            }

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

            //if (applyForce && dir == Direction.LEFT && allowLeftMovement)
            //    body.ApplyForce(new Vector2(getGroundAngleX() * -WALK_FORCE * modifier, getGroundAngleY() * -WALK_FORCE * modifier), body.GetPosition());
            //else if (applyForce && dir == Direction.RIGHT && allowRightMovement)
            //    body.ApplyForce(new Vector2(getGroundAngleX() * WALK_FORCE * modifier, getGroundAngleY() * WALK_FORCE * modifier), body.GetPosition());
        }
    }
}
