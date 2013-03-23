using System;
using System.Collections.Generic;
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
