using System;

namespace StasisGame.Components
{
    public class IgnoreTreeCollisionComponent : IComponent
    {
        public ComponentType componentType { get { return ComponentType.IgnoreTreeCollision; } }
        public IgnoreTreeCollisionComponent()
        {
        }
    }
}
