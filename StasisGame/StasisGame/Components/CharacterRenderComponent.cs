using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class CharacterRenderComponent : IComponent
    {
        public ComponentType componentType { get { return ComponentType.CharacterRender; } }

        public CharacterRenderComponent()
        {

        }
    }
}
