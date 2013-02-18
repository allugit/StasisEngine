using System;
using Box2D.XNA;

namespace StasisGame.Components
{
    public class PrismaticJointComponent : IComponent
    {
        private PrismaticJoint _prismaticJoint;

        public ComponentType componentType { get { return ComponentType.Prismatic; } }
        public PrismaticJoint prismaticJoint { get { return _prismaticJoint; } }

        public PrismaticJointComponent(PrismaticJoint prismaticJoint)
        {
            _prismaticJoint = prismaticJoint;
        }
    }
}
