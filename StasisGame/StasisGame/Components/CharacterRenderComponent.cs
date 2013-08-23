using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class CharacterRenderComponent : IComponent
    {
        private string _character;
        private string _animation;
        private float _time;
        private int _currentFrame;

        public ComponentType componentType { get { return ComponentType.CharacterRender; } }
        public string character { get { return _character; } }
        public string animation { get { return _animation; } set { _animation = value; } }
        public float time { get { return _time; } set { _time = value; } }
        public int currentFrame { get { return _currentFrame; } set { _currentFrame = value; } }

        public CharacterRenderComponent(string character)
        {
            _character = character;
            _animation = "idle";
        }
    }
}
