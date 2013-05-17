using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class SkipFluidResolutionComponent : IComponent
    {
        public ComponentType componentType { get { return ComponentType.SkipFluidResolution; } }

        public SkipFluidResolutionComponent()
        {
        }
    }
}
