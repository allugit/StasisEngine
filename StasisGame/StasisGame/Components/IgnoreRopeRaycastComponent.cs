using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class IgnoreRopeRaycastComponent : IComponent
    {
        public ComponentType componentType { get { return ComponentType.IgnoreRopeRaycast; } }

        public IgnoreRopeRaycastComponent()
        {
        }
    }
}
