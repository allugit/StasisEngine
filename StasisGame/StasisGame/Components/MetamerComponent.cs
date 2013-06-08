using System;

namespace StasisGame.Components
{
    public class MetamerComponent : IComponent
    {
        private Metamer _metamer;

        public ComponentType componentType { get { return ComponentType.Metamer; } }
        public Metamer metamer { get { return _metamer; } set { _metamer = value; } }

        public MetamerComponent(Metamer metamer)
        {
            _metamer = metamer;
        }
    }
}
