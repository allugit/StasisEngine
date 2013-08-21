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
        public const float CLIMB_SPEED = 0.1f;
        private float _baseWalkMultiplier = 0.25f;
        private float _baseSwingMultiplier = 0.1f;
        private float _baseAirWalkMultiplier = 0.05f;
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

        public void attemptRopeGrab(string levelUid, int characterId, CharacterMovementComponent characterMovementComponent, PhysicsComponent physicsComponent, RopeGrabComponent existingRopeGrabComponent)
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
                    RopeComponent ropeComponent = (RopeComponent)_entityManager.getComponent(levelUid, ropeEntityId, ComponentType.Rope);
                    RopeGrabComponent ropeGrabComponent = null;

                    if (ropeComponent != null && !ropeComponent.doubleAnchor)
                    {
                        RopeNode current = ropeComponent.ropeNodeHead;

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
                            RopeComponent existingRopeComponent = (RopeComponent)_entityManager.getComponent(levelUid, existingRopeGrabComponent.ropeEntityId, ComponentType.Rope);

                            if (existingRopeComponent.destroyAfterRelease)
                                existingRopeComponent.timeToLive = 100;

                            _ropeSystem.releaseRope(existingRopeGrabComponent, physicsComponent.body);
                            _entityManager.removeComponent(levelUid, characterId, existingRopeGrabComponent);
                        }

                        ropeGrabComponent = new RopeGrabComponent(ropeEntityId, ropeNode, (float)nodeCount, ropeComponent.reverseClimbDirection);
                        _ropeSystem.grabRope(ropeGrabComponent, physicsComponent.body);
                        _entityManager.addComponent(levelUid, characterId, ropeGrabComponent);

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
                LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;

                if (levelSystem.finalized)
                {
                    string levelUid = LevelSystem.currentLevelUid;
                    List<int> characterEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.CharacterMovement);

                    for (int i = 0; i < characterEntities.Count; i++)
                    {
                        PhysicsComponent physicsComponent = _entityManager.getComponent(levelUid, characterEntities[i], ComponentType.Physics) as PhysicsComponent;
                        ParticleInfluenceComponent particleInfluenceComponent = _entityManager.getComponent(levelUid, characterEntities[i], ComponentType.ParticleInfluence) as ParticleInfluenceComponent;
                        CharacterMovementComponent characterMovementComponent = _entityManager.getComponent(levelUid, characterEntities[i], ComponentType.CharacterMovement) as CharacterMovementComponent;
                        RopeGrabComponent ropeGrabComponent = _entityManager.getComponent(levelUid, characterEntities[i], ComponentType.RopeGrab) as RopeGrabComponent;
                        Body body = physicsComponent.body;
                        float currentSpeed = body.LinearVelocity.Length();

                        // Handle fluid properties
                        characterMovementComponent.inFluid = particleInfluenceComponent.particleCount > 2;
                        //characterMovementComponent.alreadyJumped = characterMovementComponent.inFluid ? false : characterMovementComponent.alreadyJumped;

                        // Handle rope grabs
                        if (characterMovementComponent.allowRopeGrab && characterMovementComponent.doRopeGrab)
                        {
                            attemptRopeGrab(levelUid, characterEntities[i], characterMovementComponent, physicsComponent, ropeGrabComponent);
                        }

                        // Calculate movement vector
                        if (characterMovementComponent.collisionNormals.Count > 0)
                        {
                            characterMovementComponent.movementUnitVector = Vector2.Zero;
                            for (int j = 0; j < characterMovementComponent.collisionNormals.Count; j++)
                            {
                                characterMovementComponent.movementUnitVector += characterMovementComponent.collisionNormals[j] / characterMovementComponent.collisionNormals.Count;
                            }
                            characterMovementComponent.movementUnitVector = new Vector2(characterMovementComponent.movementUnitVector.Y, -characterMovementComponent.movementUnitVector.X);
                            characterMovementComponent.movementUnitVector.Normalize();
                        }
                        else
                        {
                            characterMovementComponent.movementUnitVector = new Vector2(-1, 0);
                        }

                        // On surface movement
                        if (characterMovementComponent.onSurface)
                        {
                            if (characterMovementComponent.walkLeft || characterMovementComponent.walkRight)
                            {
                                // Adjust friction
                                if (body.LinearVelocity.X < -0.1f && characterMovementComponent.walkRight)
                                {
                                    body.Friction = 10f;
                                }
                                else if (body.LinearVelocity.X > 0.1f && characterMovementComponent.walkLeft)
                                {
                                    body.Friction = 10f;
                                }
                                else
                                {
                                    body.Friction = 0.1f;
                                }

                                // Walk
                                if (currentSpeed <= characterMovementComponent.speedLimit)
                                {
                                    Vector2 movementUnitVector = characterMovementComponent.movementUnitVector;
                                    Vector2 impulse;

                                    if (characterMovementComponent.walkRight)
                                    {
                                        movementUnitVector *= -1;
                                    }
                                    impulse = movementUnitVector * _baseWalkMultiplier;
                                    body.ApplyLinearImpulse(ref impulse);
                                }
                            }
                            else
                            {
                                body.Friction = 10f;
                            }
                        }
                        else  // In-air movement
                        {
                            if (characterMovementComponent.walkLeft || characterMovementComponent.walkRight)
                            {
                                if (ropeGrabComponent != null)
                                {
                                    // Swing
                                    //float angle = ropeGrabComponent.ropeNode.body.Rotation;
                                    //Vector2 ropeUnitVector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                                    //Vector2 averageUnitVector = Vector2.Normalize((ropeUnitVector + characterMovementComponent.movementUnitVector * 3f) / 4f);
                                    Vector2 impulse = characterMovementComponent.movementUnitVector * _baseSwingMultiplier;

                                    if (characterMovementComponent.walkRight)
                                    {
                                        impulse *= -1;
                                    }

                                    body.ApplyLinearImpulse(ref impulse);
                                }
                                else
                                {
                                    // Air walk
                                    if ((body.LinearVelocity.X < 0 && characterMovementComponent.walkRight) ||
                                        (body.LinearVelocity.X > 0 && characterMovementComponent.walkLeft) ||
                                        (body.LinearVelocity.X > -characterMovementComponent.speedLimit && characterMovementComponent.walkLeft) ||
                                        (body.LinearVelocity.X < characterMovementComponent.speedLimit && characterMovementComponent.walkRight))
                                    {
                                        Vector2 impulse = characterMovementComponent.movementUnitVector * _baseAirWalkMultiplier;

                                        if (characterMovementComponent.walkRight)
                                        {
                                            impulse *= -1;
                                        }

                                        body.ApplyLinearImpulse(ref impulse);
                                    }
                                }
                            }
                        }

                        // Jump
                        if (characterMovementComponent.attemptJump)
                        {
                            // While holding rope
                            if (ropeGrabComponent != null)
                            {
                                RopeComponent ropeComponent = _entityManager.getComponent(levelUid, ropeGrabComponent.ropeEntityId, ComponentType.Rope) as RopeComponent;
                                Vector2 impulse = new Vector2(0, -1.2f);

                                if (ropeComponent != null && ropeComponent.destroyAfterRelease)
                                    ropeComponent.timeToLive = 100;

                                _ropeSystem.releaseRope(ropeGrabComponent, physicsComponent.body);
                                _entityManager.removeComponent(levelUid, characterEntities[i], ropeGrabComponent);
                                ropeGrabComponent = null;

                                body.ApplyLinearImpulse(ref impulse);
                                //body.LinearVelocity = new Vector2(body.LinearVelocity.X, body.LinearVelocity.Y - jumpForce * 0.66f);
                            }

                            if (characterMovementComponent.onSurface)
                            {
                                Vector2 impulse = new Vector2(0, -2f);
                                float adjustment = 0f;

                                // Try to limit the impulse based on the current y velocity.
                                // This is done to prevent jumps from contributing too much to the y velocity when
                                // the player is already moving upwards too fast (which results in a super-jump).
                                adjustment = (body.LinearVelocity.Y / 6f);
                                impulse.Y -= adjustment;

                                body.ApplyLinearImpulse(ref impulse);
                                characterMovementComponent.attemptJump = false;
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

                        //characterMovementComponent.collisionNormals.Clear();
                    }
                }
            }
            _singleStep = false;
        }
    }
}
