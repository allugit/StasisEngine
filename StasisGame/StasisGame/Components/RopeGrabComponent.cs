﻿using System;
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
            RevoluteJoint joint = null;
            RevoluteJointDef jointDef = new RevoluteJointDef();

            jointDef.bodyA = node.body;
            jointDef.bodyB = bodyToAttach;
            jointDef.localAnchorA = Vector2.Zero;
            jointDef.localAnchorB = Vector2.Zero;

            joint = bodyToAttach.GetWorld().CreateJoint(jointDef) as RevoluteJoint;
            _joints.Add(bodyToAttach, joint);
        }

        public void detachBody(Body bodyToDetach)
        {
            RevoluteJoint joint = _joints[bodyToDetach];
            bodyToDetach.GetWorld().DestroyJoint(joint);
            _joints.Remove(bodyToDetach);
        }
    }
}
