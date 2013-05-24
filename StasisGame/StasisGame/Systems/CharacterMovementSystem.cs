using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class CharacterMovementSystem : ISystem
    {
        public const float MAX_WALK_SPEED = 7;
        public const float WALK_FORCE = 12;
        public const float JUMP_FORCE = 10.5f;
        public const float CLIMB_SPEED = 0.1f;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private RopeSystem _ropeSystem;
        private bool _paused;
        private bool _singleStep;

        public int defaultPriority { get { return 40; } }
        public SystemType systemType { get { return SystemType.CharacterMovement; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public CharacterMovementSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _ropeSystem = _systemManager.getSystem(SystemType.Rope) as RopeSystem;
        }

        public void attemptRopeGrab(int characterId, CharacterMovementComponent characterMovementComponent, PhysicsComponent physicsComponent, RopeGrabComponent existingRopeGrabComponent)
        {
            float margin = 0.5f;
            AABB region = new AABB();
            RopeNode ropeNode = null;
            int nodeCount = 0;

            region.LowerBound = physicsComponent.body.Position - new Vector2(margin, margin);
            region.UpperBound = physicsComponent.body.Position + new Vector2(margin, margin);

            if (physicsComponent == null)
                return;

            // Query the world for a body, and check to see if it's a rope
            physicsComponent.body.World.QueryAABB((fixture) =>
                {
                    int ropeEntityId = (int)fixture.Body.UserData;
                    RopePhysicsComponent ropePhysicsComponent = (RopePhysicsComponent)_entityManager.getComponent(ropeEntityId, ComponentType.RopePhysics);
                    RopeGrabComponent ropeGrabComponent = null;

                    if (ropePhysicsComponent != null && !ropePhysicsComponent.doubleAnchor)
                    {
                        RopeNode current = ropePhysicsComponent.ropeNodeHead;

                        characterMovementComponent.allowRopeGrab = false;

                        while (current != null)
                        {
                            if (current.body == fixture.Body)
                            {
                                ropeNode = current;
                                break;
                            }
                            nodeCount++;
                            current = current.next;
                        }

                        if (existingRopeGrabComponent != null)
                        {
                            RopePhysicsComponent existingRopePhysicsComponent = (RopePhysicsComponent)_entityManager.getComponent(existingRopeGrabComponent.ropeEntityId, ComponentType.RopePhysics);

                            if (existingRopePhysicsComponent.destroyAfterRelease)
                                existingRopePhysicsComponent.timeToLive = 100;

                            _ropeSystem.releaseRope(existingRopeGrabComponent, physicsComponent.body);
                            _entityManager.removeComponent(characterId, existingRopeGrabComponent);
                        }

                        ropeGrabComponent = new RopeGrabComponent(ropeEntityId, ropeNode, (float)nodeCount, ropePhysicsComponent.reverseClimbDirection);
                        _ropeSystem.grabRope(ropeGrabComponent, physicsComponent.body);
                        _entityManager.addComponent(characterId, ropeGrabComponent);

                        return false;
                    }
                    return true;
                },
                ref region);
        }

        public void update()
        {
            if (!_paused || _singleStep)
            {
                List<int> characterEntities = _entityManager.getEntitiesPosessing(ComponentType.CharacterMovement);

                for (int i = 0; i < characterEntities.Count; i++)
                {
                    PhysicsComponent physicsComponent = _entityManager.getComponent(characterEntities[i], ComponentType.Physics) as PhysicsComponent;
                    ParticleInfluenceComponent particleInfluenceComponent = _entityManager.getComponent(characterEntities[i], ComponentType.ParticleInfluence) as ParticleInfluenceComponent;
                    CharacterMovementComponent characterMovementComponent = _entityManager.getComponent(characterEntities[i], ComponentType.CharacterMovement) as CharacterMovementComponent;
                    RopeGrabComponent ropeGrabComponent = _entityManager.getComponent(characterEntities[i], ComponentType.RopeGrab) as RopeGrabComponent;
                    Body body = physicsComponent.body;
                    Vector2 averageNormal = Vector2.Zero;
                    float modifier = characterMovementComponent.walkSpeedModifier;
                    bool applyForce =
                        (characterMovementComponent.walkLeft && characterMovementComponent.allowLeftMovement) ||
                        (characterMovementComponent.walkRight && characterMovementComponent.allowRightMovement);
                    Vector2 characterVelocity = physicsComponent.body.LinearVelocity;
                    float characterSpeed = characterVelocity.Length();
                    float characterHorizontalSpeed = Math.Abs(characterVelocity.X);

                    // Handle fluid properties
                    characterMovementComponent.inFluid = particleInfluenceComponent.particleCount > 2;
                    characterMovementComponent.alreadyJumped = characterMovementComponent.inFluid ? false : characterMovementComponent.alreadyJumped;

                    characterMovementComponent.calculateMovementAngle();

                    if (characterMovementComponent.allowRopeGrab && characterMovementComponent.doRopeGrab)
                    {
                        attemptRopeGrab(characterEntities[i], characterMovementComponent, physicsComponent, ropeGrabComponent);
                    }

                    if (characterMovementComponent.onSurface)
                    {
                        // Check speed limit
                        if (Math.Abs(body.LinearVelocity.Length()) > MAX_WALK_SPEED)
                            applyForce = false;

                        // Pull harder if grabbing
                        //if (grabbing)
                        //    modifier = 2;

                        // Apply movement force
                        if (applyForce)
                        {
                            Vector2 movement = new Vector2((float)Math.Cos(characterMovementComponent.movementAngle), (float)Math.Sin(characterMovementComponent.movementAngle));

                            if (characterMovementComponent.inFluid)
                                modifier *= 0.66f;

                            movement *= characterMovementComponent.walkLeft ? -1 : 1;
                            movement *= WALK_FORCE * modifier;
                            body.ApplyForce(movement, body.Position);
                        }

                        // Fake friction when not moving
                        if ((body.LinearVelocity.X > 0 && characterMovementComponent.walkLeft) ||
                            (body.LinearVelocity.X < 0 && characterMovementComponent.walkRight) ||
                            (!characterMovementComponent.walkLeft && !characterMovementComponent.walkRight) &&
                            !characterMovementComponent.jump)
                        {
                            // All conditions necessary for damping have been met
                            if (Math.Abs(body.LinearVelocity.Y) < 1 || Math.Abs(body.LinearVelocity.Length()) < 10)
                                body.ApplyForce(new Vector2(-body.LinearVelocity.X * 10, -body.LinearVelocity.Y * 5), body.Position);
                        }
                    }
                    else
                    {
                        if (characterMovementComponent.walkLeft || characterMovementComponent.walkRight)
                        {
                            if (ropeGrabComponent != null)
                            {
                                // Swing
                                float swingForce = (characterMovementComponent.walkLeft ? -WALK_FORCE : WALK_FORCE) / 2f;
                                Vector2 movement = new Vector2((float)Math.Cos(characterMovementComponent.movementAngle), (float)Math.Sin(characterMovementComponent.movementAngle));
                                physicsComponent.body.ApplyForce(movement * swingForce, body.Position);
                            }
                            else
                            {
                                // Air walk
                                float airWalkForce = (characterMovementComponent.walkLeft ? -WALK_FORCE : WALK_FORCE) / 4;

                                // Check speed limit
                                if (Math.Abs(body.LinearVelocity.X) > MAX_WALK_SPEED / 2)
                                {
                                    if (body.LinearVelocity.X < -MAX_WALK_SPEED / 2 && airWalkForce < 0)
                                        applyForce = false;
                                    else if (body.LinearVelocity.X > MAX_WALK_SPEED / 2 && airWalkForce > 0)
                                        applyForce = false;
                                }

                                // Apply movement force
                                if (applyForce)
                                {
                                    Vector2 movement = new Vector2((float)Math.Cos(characterMovementComponent.movementAngle), (float)Math.Sin(characterMovementComponent.movementAngle));
                                    movement *= airWalkForce;
                                    body.ApplyForce(movement, body.Position);
                                }
                            }
                        }
                    }

                    // Jump
                    if (characterMovementComponent.jump)
                    {
                        float jumpForce = characterMovementComponent.inFluid ? JUMP_FORCE * 0.66f : JUMP_FORCE;

                        // While holding rope
                        if (ropeGrabComponent != null)
                        {
                            RopePhysicsComponent ropePhysicsComponent = _entityManager.getComponent(ropeGrabComponent.ropeEntityId, ComponentType.RopePhysics) as RopePhysicsComponent;

                            if (ropePhysicsComponent != null && ropePhysicsComponent.destroyAfterRelease)
                                ropePhysicsComponent.timeToLive = 100;

                            _ropeSystem.releaseRope(ropeGrabComponent, physicsComponent.body);
                            _entityManager.removeComponent(characterEntities[i], ropeGrabComponent);
                            ropeGrabComponent = null;

                            body.LinearVelocity = new Vector2(body.LinearVelocity.X, body.LinearVelocity.Y - jumpForce * 0.66f);
                        }
                        else if (!characterMovementComponent.alreadyJumped && (characterMovementComponent.allowLeftMovement || characterMovementComponent.allowRightMovement))
                        {
                            characterMovementComponent.alreadyJumped = true;
                            body.LinearVelocity = new Vector2(body.LinearVelocity.X, -jumpForce);
                        }
                    }

                    // Climbing
                    if (ropeGrabComponent != null)
                    {
                        float climbSpeed = characterMovementComponent.climbAmount * CLIMB_SPEED;
                        if (characterMovementComponent.climbUp)
                        {
                            _ropeSystem.moveAttachedBody(ropeGrabComponent, physicsComponent.body, climbSpeed);
                        }
                        else if (characterMovementComponent.climbDown)
                        {
                            _ropeSystem.moveAttachedBody(ropeGrabComponent, physicsComponent.body, -climbSpeed);
                        }
                    }

                    characterMovementComponent.collisionNormals.Clear();
                }
            }
            _singleStep = false;
        }
    }
}
