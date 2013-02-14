using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class PlayerComponent : IComponent
    {
        public ComponentType componentType { get { return ComponentType.Player; } }

        public PlayerComponent()
        {
        }
    }
}
