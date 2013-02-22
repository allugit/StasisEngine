using System;
using Box2D.XNA;
using StasisCore;

namespace StasisGame.Components
{
    public class RevoluteComponent : IComponent, IEventHandler
    {
        private RevoluteJoint _revoluteJoint;

        public ComponentType componentType { get { return ComponentType.Revolute; } }
        public RevoluteJoint revoluteJoint { get { return _revoluteJoint; } }

        public RevoluteComponent(RevoluteJoint revoluteJoint)
        {
            _revoluteJoint = revoluteJoint;
        }

        public void trigger(GameEvent e)
        {
            switch (e.type)
            {
                case GameEventType.EnableMotor:
                    _revoluteJoint.EnableMotor(true);
                    break;

                case GameEventType.DisableMotor:
                    _revoluteJoint.EnableMotor(false);
                    break;

                case GameEventType.ReverseMotor:
                    _revoluteJoint.SetMotorSpeed(-_revoluteJoint.GetMotorSpeed());
                    break;
            }
        }
    }
}
