using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class CharacterRenderComponent : IComponent
    {
        private string _character;
        private string _animation;

        public ComponentType componentType { get { return ComponentType.CharacterRender; } }
        public string character { get { return _character; } }
        public string animation { get { return _animation; } }

        public CharacterRenderComponent(string character)
        {
            _character = character;
            _animation = "idle";
        }
    }
}
