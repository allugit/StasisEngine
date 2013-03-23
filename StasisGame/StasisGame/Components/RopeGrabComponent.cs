using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Box2D.XNA;

namespace StasisGame.Components
{
    public class RopeGrabComponent : IComponent
    {
        private RopeNode _ropeNode;
        private float _distance;
        private Dictionary<Body, RevoluteJoint> _joints;

        public ComponentType componentType { get { return ComponentType.RopeGrab; } }
        //public RopeNode ropeNode { get { return _ropeNode; } set { _ropeNode = value; } }
        public float distance { get { return _distance; } set { _distance = value; } }
        public Dictionary<Body, RevoluteJoint> joints { get { return _joints; } }

        public RopeGrabComponent(RopeNode ropeNode)
        {
            _ropeNode = ropeNode;
            _joints = new Dictionary<Body, RevoluteJoint>();
        }

        public void attachBody(Body bodyToAttach, float distance)
        {
            int index = (int)Math.Floor(distance);
            float fraction = distance - (float)index;
            RopeNode node = _ropeNode.getByIndex(index);
            float lengthPosition = -(node.halfLength * 2 * fraction - node.halfLength);
            RevoluteJoint joint = null;
            RevoluteJointDef jointDef = new RevoluteJointDef();

            bodyToAttach.Position = node.body.GetWorldPoint(new Vector2(lengthPosition, 0));
            jointDef.bodyA = bodyToAttach;
            jointDef.bodyB = node.body;
            jointDef.localAnchorA = Vector2.Zero;
            jointDef.localAnchorB = node.body.GetLocalPoint(bodyToAttach.GetPosition());
            joint = bodyToAttach.GetWorld().CreateJoint(jointDef) as RevoluteJoint;
            _joints.Add(bodyToAttach, joint);
        }

        public void detachBody(Body bodyToDetach)
        {
            RevoluteJoint joint = _joints[bodyToDetach];
            bodyToDetach.GetWorld().DestroyJoint(joint);
            _joints.Remove(bodyToDetach);
        }

        public void detachAll()
        {
            foreach (RevoluteJoint joint in _joints.Values)
            {
                joint.GetBodyA().GetWorld().DestroyJoint(joint);
            }
            _joints.Clear();
        }

        public void moveAttachedBody(Body bodyToMove, float climbSpeed)
        {
            float newDistance = _distance + climbSpeed;
            RopeNode newNode = _ropeNode.getByIndex((int)Math.Floor(newDistance));

            if (newNode != null)
            {
                detachBody(bodyToMove);
                attachBody(bodyToMove, newDistance);
                _distance = newDistance;
            }
        }
    }
}
