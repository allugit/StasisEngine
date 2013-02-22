using System;
using Box2D.XNA;
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
            Console.WriteLine("Prismatic joint component event. origin: {0}, type: {1}", e.originEntityId, e.type);
        }
    }
}
