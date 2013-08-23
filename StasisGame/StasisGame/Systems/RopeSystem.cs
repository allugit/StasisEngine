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

        public void killRope(string levelUid, int entityId)
        {
            RopeComponent ropeComponent = (RopeComponent)_entityManager.getComponent(levelUid, entityId, ComponentType.Rope);
            List<int> ropeGrabEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.RopeGrab);

            // Detach any rope grab components from this rope
            for (int i = 0; i < ropeGrabEntities.Count; i++)
            {
                RopeGrabComponent ropeGrabComponent = (RopeGrabComponent)_entityManager.getComponent(levelUid, ropeGrabEntities[i], ComponentType.RopeGrab);
                if (ropeGrabComponent.ropeEntityId == entityId)
                {
                    detachAll(ropeGrabComponent);
                    _entityManager.removeComponent(levelUid, ropeGrabEntities[i], ropeGrabComponent);
                }
            }

            if (ropeComponent != null)
            {
                RopeNode current = ropeComponent.ropeNodeHead;
                while (current != null)
                {
                    if (current.anchorJoint != null)
                    {
                        int entityIdA = (int)current.anchorJoint.BodyA.UserData;
                        int entityIdB = (int)current.anchorJoint.BodyB.UserData;
                        MetamerComponent metamerComponentA = _entityManager.getComponent(levelUid, entityIdA, ComponentType.Metamer) as MetamerComponent;
                        MetamerComponent metamerComponentB = _entityManager.getComponent(levelUid, entityIdB, ComponentType.Metamer) as MetamerComponent;

                        if (metamerComponentA != null)
                        {
                            metamerComponentA.metamer.anchorCount--;
                            if (metamerComponentA.metamer.anchorCount <= 0)
                            {
                                current.anchorJoint.BodyA.World.RemoveBody(current.anchorJoint.BodyA);
                                metamerComponentA.metamer.body = null;
                            }
                        }
                        if (metamerComponentB != null)
                        {
                            metamerComponentB.metamer.anchorCount--;
                            if (metamerComponentB.metamer.anchorCount <= 0)
                            {
                                current.anchorJoint.BodyB.World.RemoveBody(current.anchorJoint.BodyB);
                                metamerComponentB.metamer.body = null;
                            }
                        }
                    }

                    // Destroy body
                    current.body.World.RemoveBody(current.body);
                    current = current.next;
                }
                _entityManager.killEntity(levelUid, entityId);
            }
        }

        public void breakAnchor(RopeNode ropeNode)
        {
            bool markedForDestruction = false;

            // Destroy joint
            ropeNode.body.World.RemoveJoint(ropeNode.anchorJoint);
            ropeNode.anchorJoint = null;

            // Start rope's time to live timer if markedForDestruction is true
            if (markedForDestruction)
                ropeNode.ropeComponent.startTTLCountdown();
        }

        // breakJoint -- Breaks the link between two nodes and determines whether or not to keep them alive, or kill them
        public void breakJoint(string levelUid, int entityId, RopeNode ropeNode)
        {
            // Disconnect linked rope nodes
            if (ropeNode.previous != null)
                ropeNode.previous.next = null;
            ropeNode.previous = null;

            // Recreate disconnected segment as its own entity
            _entityManager.factory.recreateRope(levelUid, ropeNode, ropeNode.ropeComponent.interpolationCount);

            // Destroy joint
            ropeNode.body.World.RemoveJoint(ropeNode.joint);
            ropeNode.joint = null;
        }

        public void update(GameTime gameTime)
        {
            if (!_paused || _singleStep)
            {
                string levelUid = LevelSystem.currentLevelUid;
                LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;

                if (levelSystem.finalized)
                {
                    List<int> ropeEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Rope);

                    for (int i = 0; i < ropeEntities.Count; i++)
                    {
                        RopeComponent ropeComponent = _entityManager.getComponent(levelUid, ropeEntities[i], ComponentType.Rope) as RopeComponent;
                        RopeGrabComponent ropeGrabComponent = _entityManager.getComponent(levelUid, ropeEntities[i], ComponentType.RopeGrab) as RopeGrabComponent;
                        RopeNode head = ropeComponent.ropeNodeHead;
                        RopeNode current = head;
                        RopeNode tail = head.tail;

                        // Check segment length
                        if (head.count < 3 && ropeGrabComponent == null)
                            ropeComponent.startTTLCountdown();

                        // Check anchors
                        if (head.anchorJoint == null && tail.anchorJoint == null)
                            ropeComponent.startTTLCountdown();

                        // Check time to live
                        if (ropeComponent.timeToLive == 0)
                        {
                            killRope(levelUid, ropeEntities[i]);
                            ropeComponent.timeToLive--;
                        }
                        else if (ropeComponent.timeToLive > -1)
                        {
                            ropeComponent.timeToLive--;
                        }

                        while (current != null)
                        {
                            // Check tensions
                            if (current.joint != null)
                            {
                                Vector2 relative;
                                if (current == head || current == tail)
                                {
                                    // Check anchor joint
                                    if (current.anchorJoint != null)
                                    {
                                        relative = current.anchorJoint.BodyA.GetWorldPoint(current.anchorJoint.LocalAnchorA) -
                                            current.anchorJoint.BodyB.GetWorldPoint(current.anchorJoint.LocalAnchorB);
                                        if (relative.Length() > 0.8f || current.anchorJoint.GetReactionForce(60f).Length() > 400f)
                                        {
                                            breakAnchor(current);
                                        }
                                    }
                                }

                                // Check other joints
                                relative = current.joint.BodyA.GetWorldPoint(current.joint.LocalAnchorA) -
                                            current.joint.BodyB.GetWorldPoint(current.joint.LocalAnchorB);
                                if (relative.Length() > 1.2f || current.joint.GetReactionForce(60f).Length() > 300f)
                                {
                                    breakJoint(levelUid, ropeEntities[i], current);
                                }
                            }
                            current = current.next;
                        }
                    }
                }
            }
            _singleStep = false;
        }
    }
}
