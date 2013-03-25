using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class RopeSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private bool _paused;
        private bool _singleStep;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Rope; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public RopeSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void grabRope(RopeGrabComponent ropeGrabComponent, Body bodyToAttach, float distance)
        {
            int index = (int)Math.Floor(distance);
            float fraction = distance - (float)index;
            RopeNode node = ropeGrabComponent.ropeNode.getByIndex(index);
            float lengthPosition = -(node.halfLength * 2 * fraction - node.halfLength);
            RevoluteJoint joint = null;
            RevoluteJointDef jointDef = new RevoluteJointDef();

            bodyToAttach.Position = node.body.GetWorldPoint(new Vector2(lengthPosition, 0));
            jointDef.bodyA = bodyToAttach;
            jointDef.bodyB = node.body;
            jointDef.localAnchorA = Vector2.Zero;
            jointDef.localAnchorB = node.body.GetLocalPoint(bodyToAttach.GetPosition());
            joint = bodyToAttach.GetWorld().CreateJoint(jointDef) as RevoluteJoint;
            ropeGrabComponent.joints.Add(bodyToAttach, joint);
        }

        public void releaseRope(RopeGrabComponent ropeGrabComponent, Body bodyToDetach)
        {
            RevoluteJoint joint = ropeGrabComponent.joints[bodyToDetach];
            bodyToDetach.GetWorld().DestroyJoint(joint);
            ropeGrabComponent.joints.Remove(bodyToDetach);
        }

        public void moveAttachedBody(RopeGrabComponent ropeGrabComponent, Body bodyToMove, float climbSpeed)
        {
            float newDistance = ropeGrabComponent.distance + climbSpeed;
            RopeNode newNode = ropeGrabComponent.ropeNode.getByIndex((int)Math.Floor(newDistance));

            if (newNode != null)
            {
                releaseRope(ropeGrabComponent, bodyToMove);
                grabRope(ropeGrabComponent, bodyToMove, newDistance);
                ropeGrabComponent.distance = newDistance;
            }
        }

        public void detachAll(RopeGrabComponent ropeGrabComponent)
        {
            foreach (RevoluteJoint joint in ropeGrabComponent.joints.Values)
            {
                joint.GetBodyA().GetWorld().DestroyJoint(joint);
            }
            ropeGrabComponent.joints.Clear();
        }

        public void killRope(int entityId)
        {
            RopePhysicsComponent ropePhysicsComponent = (RopePhysicsComponent)_entityManager.getComponent(entityId, ComponentType.RopePhysics);
            List<int> ropeGrabEntities = _entityManager.getEntitiesPosessing(ComponentType.RopeGrab);

            for (int i = 0; i < ropeGrabEntities.Count; i++)
            {
                RopeGrabComponent ropeGrabComponent = (RopeGrabComponent)_entityManager.getComponent(ropeGrabEntities[i], ComponentType.RopeGrab);
                if (ropeGrabComponent.ropeEntityId == entityId)
                {
                    detachAll(ropeGrabComponent);
                    _entityManager.removeComponent(ropeGrabEntities[i], ropeGrabComponent);
                }
            }

            if (ropePhysicsComponent != null)
            {
                RopeNode current = ropePhysicsComponent.ropeNodeHead;

                while (current != null)
                {
                    current.body.GetWorld().DestroyBody(current.body);
                    current = current.next;
                }
                _entityManager.killEntity(entityId);
            }
        }

        public void update()
        {
            List<int> ropePhysicsEntities = _entityManager.getEntitiesPosessing(ComponentType.RopePhysics);

            for (int i = 0; i < ropePhysicsEntities.Count; i++)
            {
                RopePhysicsComponent ropePhysicsComponent = (RopePhysicsComponent)_entityManager.getComponent(ropePhysicsEntities[i], ComponentType.RopePhysics);
                RopeGrabComponent ropeGrabComponent = (RopeGrabComponent)_entityManager.getComponent(ropePhysicsEntities[i], ComponentType.RopeGrab);
                RopeNode head = ropePhysicsComponent.ropeNodeHead;
                RopeNode current = head;
                RopeNode tail = head.tail;

                // Check time to live
                if (ropePhysicsComponent.timeToLive == 0)
                {
                    killRope(ropePhysicsEntities[i]);
                    ropePhysicsComponent.timeToLive--;
                }
                else if (ropePhysicsComponent.timeToLive > -1)
                {
                    ropePhysicsComponent.timeToLive--;
                }

                while (current != null)
                {
                    // Check tensions
                    if (current.joint != null)
                    {
                        Vector2 distance;
                        if (current == head)
                        {
                            // Check anchor joint
                            if (current.anchorJoint != null)
                            {
                                distance = current.anchorJoint.GetBodyA().GetWorldPoint(current.anchorJoint._localAnchor1) -
                                    current.anchorJoint.GetBodyB().GetWorldPoint(current.anchorJoint._localAnchor2);
                                if (distance.Length() > 0.8f || current.anchorJoint.GetReactionForce(60f).Length() > 400f)
                                {
                                    int ttl = ropePhysicsComponent.timeToLive;
                                    ropePhysicsComponent.timeToLive = (ttl > -1 && ttl < 100) ? ttl : 100;
                                    current.body.GetWorld().DestroyJoint(current.anchorJoint);
                                    current.anchorJoint = null;
                                }
                            }
                        }
                        else if (current == tail)
                        {
                            // Check anchor joint
                            if (current.anchorJoint != null)
                            {
                                distance = current.anchorJoint.GetBodyA().GetWorldPoint(current.anchorJoint._localAnchor1) -
                                    current.anchorJoint.GetBodyB().GetWorldPoint(current.anchorJoint._localAnchor2);
                                if (distance.Length() > 0.8f || current.anchorJoint.GetReactionForce(60f).Length() > 400f)
                                {
                                    int ttl = ropePhysicsComponent.timeToLive;
                                    ropePhysicsComponent.timeToLive = (ttl > -1 && ttl < 100) ? ttl : 100;
                                    current.body.GetWorld().DestroyJoint(current.anchorJoint);
                                    current.anchorJoint = null;
                                }
                            }
                        }

                        // Check other joints
                        distance = current.joint.GetBodyA().GetWorldPoint(current.joint._localAnchor1) -
                                    current.joint.GetBodyB().GetWorldPoint(current.joint._localAnchor2);
                        if (distance.Length() > 1.2f || current.joint.GetReactionForce(60f).Length() > 300f)
                        {
                            int ttl = ropePhysicsComponent.timeToLive;
                            ropePhysicsComponent.timeToLive = (ttl > -1 && ttl < 100) ? ttl : 100;
                            current.body.GetWorld().DestroyJoint(current.joint);
                            current.joint = null;
                        }
                    }
                    current = current.next;
                }
            }
        }
    }
}
