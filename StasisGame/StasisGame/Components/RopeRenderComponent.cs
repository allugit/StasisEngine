using System;

namespace StasisGame.Components
{
    public class RopeRenderComponent : IComponent
    {
        public ComponentType componentType { get { return ComponentType.RopeRender; } }

        public RopeRenderComponent()
        {
        }
    }
}
