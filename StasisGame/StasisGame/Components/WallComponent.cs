using System;

namespace StasisGame.Components
{
    public class WallComponent : IComponent
    {
        public ComponentType componentType { get { return ComponentType.Wall; } }
    }
}
