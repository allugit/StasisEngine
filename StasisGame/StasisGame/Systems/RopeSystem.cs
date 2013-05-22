using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class RopeSystem : ISystem
    {
        public const int ROPE_TIME_TO_LIVE = 100;
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

        public void grabRope(RopeGrabComponent ropeGrabComponent, Body bodyToAttach)
        {
            int index = (int)Math.Floor(ropeGrabComponent.distance);
            float fraction = ropeGrabComponent.distance - (float)index;
            RopeNode node = ropeGrabComponent.ropeNode.getByIndex(index);
            float lengthPosition = -(node.halfLength * 2 * fraction - node.halfLength);
            RevoluteJoint joint;

            ropeGrabComponent.ropeNode = node;
            bodyToAttach.Position = node.body.GetWorldPoint(new Vector2(lengthPosition, 0));
            //jointDef.bodyA = bodyToAttach;
            //jointDef.bodyB = node.body;
            //jointDef.localAnchorA = Vector2.Zero;
            //jointDef.localAnchorB = node.body.GetLocalPoint(bodyToAttach.GetPosition());
            //joint = bodyToAttach.GetWorld().CreateJoint(jointDef) as RevoluteJoint;
            joint = JointFactory.CreateRevoluteJoint(bodyToAttach.World, bodyToAttach, node.body, node.body.GetLocalPoint(bodyToAttach.Position));
            ropeGrabComponent.joints.Add(bodyToAttach, joint);
        }

        public void releaseRope(RopeGrabComponent ropeGrabComponent, Body bodyToDetach)
        {
            RevoluteJoint joint = ropeGrabComponent.joints[bodyToDetach];
            bodyToDetach.World.RemoveJoint(joint);
            ropeGrabComponent.joints.Remove(bodyToDetach);
        }

        public void moveAttachedBody(RopeGrabComponent ropeGrabComponent, Body bodyToMove, float climbSpeed)
        {
            float newDistance = ropeGrabComponent.distance + (ropeGrabComponent.reverseClimbDirection ? -climbSpeed : climbSpeed);
            RopeNode newNode = ropeGrabComponent.ropeNode.getByIndex((int)Math.Floor(newDistance));

            if (newNode != null)
            {
                if (ropeGrabComponent.reverseClimbDirection)
                {
                    if (climbSpeed > 0 && newNode != ropeGrabComponent.ropeNode && ropeGrabComponent.ropeNode.joint == null)
                        return;
                    else if (climbSpeed < 0 && newNode != ropeGrabComponent.ropeNode && newNode.joint == null)
                        return;
                }
                else
                {
                    if (climbSpeed > 0 && newNode != ropeGrabComponent.ropeNode && newNode.joint == null)
                        return;
                    else if (climbSpeed < 0 && newNode != ropeGrabComponent.ropeNode && ropeGrabComponent.ropeNode.joint == null)
                        return;
                }

                ropeGrabComponent.distance = newDistance;
                releaseRope(ropeGrabComponent, bodyToMove);
                grabRope(ropeGrabComponent, bodyToMove);
            }
        }

        public void detachAll(RopeGrabComponent ropeGrabComponent)
        {
            foreach (RevoluteJoint joint in ropeGrabComponent.joints.Values)
            {
                joint.BodyA.World.RemoveJoint(joint);
            }
            ropeGrabComponent.joints.Clear();
        }

        public void killRope(int entityId)
        {
            RopePhysicsComponent ropePhysicsComponent = (RopePhysicsComponent)_entityManager.getComponent(entityId, ComponentType.RopePhysics);
            List<int> ropeGrabEntities = _entityManager.getEntitiesPosessing(ComponentType.RopeGrab);

            // Detach any rope grab components from this rope
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
                    if (current.anchorJoint != null)
                    {
                        int entityIdA = (int)current.anchorJoint.BodyA.UserData;
                        int entityIdB = (int)current.anchorJoint.BodyB.UserData;
                        TreeComponent treeComponentA = _entityManager.getComponent(entityIdA, ComponentType.Tree) as TreeComponent;
                        TreeComponent treeComponentB = _entityManager.getComponent(entityIdB, ComponentType.Tree) as TreeComponent;

                        if (treeComponentA != null)
                        {
                            current.anchorJoint.BodyA.World.RemoveBody(current.anchorJoint.BodyA);
                        }
                        else if (treeComponentB != null)
                        {
                            current.anchorJoint.BodyB.World.RemoveBody(current.anchorJoint.BodyB);
                        }
                    }

                    // Destroy body
                    current.body.World.RemoveBody(current.body);
                    current = current.next;
                }
                _entityManager.killEntity(entityId);
            }
        }

        public void update()
        {
            if (!_paused || _singleStep)
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
                                    distance = current.anchorJoint.BodyA.GetWorldPoint(current.anchorJoint.LocalAnchorA) -
                                        current.anchorJoint.BodyB.GetWorldPoint(current.anchorJoint.LocalAnchorB);
                                    if (distance.Length() > 0.8f || current.anchorJoint.GetReactionForce(60f).Length() > 400f)
                                    {
                                        int ttl = ropePhysicsComponent.timeToLive;
                                        ropePhysicsComponent.timeToLive = (ttl > -1 && ttl < ROPE_TIME_TO_LIVE) ? ttl : ROPE_TIME_TO_LIVE;
                                        current.body.World.RemoveJoint(current.anchorJoint);
                                        current.anchorJoint = null;
                                    }
                                }
                            }
                            else if (current == tail)
                            {
                                // Check anchor joint
                                if (current.anchorJoint != null)
                                {
                                    distance = current.anchorJoint.BodyA.GetWorldPoint(current.anchorJoint.LocalAnchorA) -
                                        current.anchorJoint.BodyB.GetWorldPoint(current.anchorJoint.LocalAnchorB);
                                    if (distance.Length() > 0.8f || current.anchorJoint.GetReactionForce(60f).Length() > 400f)
                                    {
                                        int ttl = ropePhysicsComponent.timeToLive;
                                        ropePhysicsComponent.timeToLive = (ttl > -1 && ttl < ROPE_TIME_TO_LIVE) ? ttl : ROPE_TIME_TO_LIVE;
                                        current.body.World.RemoveJoint(current.anchorJoint);
                                        current.anchorJoint = null;
                                    }
                                }
                            }

                            // Check other joints
                            distance = current.joint.BodyA.GetWorldPoint(current.joint.LocalAnchorA) -
                                        current.joint.BodyB.GetWorldPoint(current.joint.LocalAnchorB);
                            if (distance.Length() > 1.2f || current.joint.GetReactionForce(60f).Length() > 300f)
                            {
                                int ttl = ropePhysicsComponent.timeToLive;
                                ropePhysicsComponent.timeToLive = (ttl > -1 && ttl < ROPE_TIME_TO_LIVE) ? ttl : ROPE_TIME_TO_LIVE;
                                current.body.World.RemoveJoint(current.joint);
                                current.joint = null;
                            }
                        }
                        current = current.next;
                    }
                }
            }
            _singleStep = false;
        }
    }
}
