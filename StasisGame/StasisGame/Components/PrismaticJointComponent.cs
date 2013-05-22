using System;
using FarseerPhysics.Dynamics.Joints;
using StasisCore;

namespace StasisGame.Components
{
    public class PrismaticJointComponent : IComponent, IEventHandler
    {
        private PrismaticJoint _prismaticJoint;
        private LimitState _previousLimitState = LimitState.Inactive;

        public ComponentType componentType { get { return ComponentType.Prismatic; } }
        public PrismaticJoint prismaticJoint { get { return _prismaticJoint; } }
        public LimitState previousLimitState { get { return _previousLimitState; } set { _previousLimitState = value; } }

        public PrismaticJointComponent(PrismaticJoint prismaticJoint)
        {
            _prismaticJoint = prismaticJoint;
        }

        public void trigger(GameEvent e)
        {
            switch (e.type)
            {
                case GameEventType.EnableMotor:
                    prismaticJoint.MotorEnabled = true;
                    break;

                case GameEventType.DisableMotor:
                    prismaticJoint.MotorEnabled = false;
                    break;

                case GameEventType.ReverseMotor:
                    prismaticJoint.MotorSpeed = -prismaticJoint.MotorSpeed;
                    break;
            }
        }
    }
}
