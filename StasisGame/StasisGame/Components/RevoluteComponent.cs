using System;
using Box2D.XNA;

namespace StasisGame.Components
{
    public class RevoluteComponent : IComponent
    {
        private RevoluteJoint _revoluteJoint;

        public ComponentType componentType { get { return ComponentType.Revolute; } }
        public RevoluteJoint revoluteJoint { get { return _revoluteJoint; } }

        public RevoluteComponent(RevoluteJoint revoluteJoint)
        {
            _revoluteJoint = revoluteJoint;
        }
    }
}
