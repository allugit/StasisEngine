using System;

namespace StasisGame.Components
{
    public class FollowMetamerComponent : IComponent
    {
        private Metamer _metamer;

        public ComponentType componentType { get { return ComponentType.FollowMetamer; } }
        public Metamer metamer { get { return _metamer; } }

        public FollowMetamerComponent(Metamer metamer)
        {
            _metamer = metamer;
        }
    }
}
