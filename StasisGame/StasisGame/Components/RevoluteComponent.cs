using System;
using FarseerPhysics.Dynamics.Joints;
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
                    _revoluteJoint.MotorEnabled = true;
                    break;

                case GameEventType.DisableMotor:
                    _revoluteJoint.MotorEnabled = false;
                    break;

                case GameEventType.ReverseMotor:
                    _revoluteJoint.MotorSpeed = -_revoluteJoint.MotorSpeed;
                    break;
            }
        }
    }
}
