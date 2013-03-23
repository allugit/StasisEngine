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

        public void attachBody(RopeGrabComponent ropeGrabComponent, Body bodyToAttach, float distance)
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

        public void detachBody(RopeGrabComponent ropeGrabComponent, Body bodyToDetach)
        {
            RevoluteJoint joint = ropeGrabComponent.joints[bodyToDetach];
            bodyToDetach.GetWorld().DestroyJoint(joint);
            ropeGrabComponent.joints.Remove(bodyToDetach);
        }

        public void detachAll(RopeGrabComponent ropeGrabComponent)
        {
            foreach (RevoluteJoint joint in ropeGrabComponent.joints.Values)
            {
                joint.GetBodyA().GetWorld().DestroyJoint(joint);
            }
            ropeGrabComponent.joints.Clear();
        }

        public void moveAttachedBody(RopeGrabComponent ropeGrabComponent, Body bodyToMove, float climbSpeed)
        {
            float newDistance = ropeGrabComponent.distance + climbSpeed;
            RopeNode newNode = ropeGrabComponent.ropeNode.getByIndex((int)Math.Floor(newDistance));

            if (newNode != null)
            {
                detachBody(ropeGrabComponent, bodyToMove);
                attachBody(ropeGrabComponent, bodyToMove, newDistance);
                ropeGrabComponent.distance = newDistance;
            }
        }

        public void update()
        {
            List<RopePhysicsComponent> ropePhysicsComponents = _entityManager.getComponents<RopePhysicsComponent>(ComponentType.RopePhysics);

            for (int i = 0; i < ropePhysicsComponents.Count; i++)
            {
                RopeNode head = ropePhysicsComponents[i].ropeNodeHead;
                RopeNode current = head;
                RopeNode tail = head.tail;

                while (current != null)
                {
                    if (current.joint != null)
                    {
                        if (current == head)
                        {
                            // Check anchor joint
                            if (current.anchorJoint != null && current.anchorJoint.GetReactionForce(60f).Length() > 400f)
                            {
                                current.body.GetWorld().DestroyJoint(current.anchorJoint);
                                current.anchorJoint = null;
                            }
                        }
                        else if (current == tail)
                        {
                            // Check anchor joint
                            if (current.anchorJoint != null && current.anchorJoint.GetReactionForce(60f).Length() > 400f)
                            {
                                current.body.GetWorld().DestroyJoint(current.anchorJoint);
                                current.anchorJoint = null;
                            }
                        }

                        // Check other joints
                        if (current.joint.GetReactionForce(60f).Length() > 400f)
                        {
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
