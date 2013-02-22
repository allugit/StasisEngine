using System;
using Box2D.XNA;
using StasisCore;

namespace StasisGame.Components
{
    public class PrismaticJointComponent : IComponent, IEventHandler
    {
        private PrismaticJoint _prismaticJoint;

        public ComponentType componentType { get { return ComponentType.Prismatic; } }
        public PrismaticJoint prismaticJoint { get { return _prismaticJoint; } }

        public PrismaticJointComponent(PrismaticJoint prismaticJoint)
        {
            _prismaticJoint = prismaticJoint;
        }

        public void trigger(GameEvent e)
        {
            Console.WriteLine("Prismatic joint component event: {0}", e);
        }
    }
}
