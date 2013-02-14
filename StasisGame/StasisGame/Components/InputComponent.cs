using System;
using Microsoft.Xna.Framework.Input;

namespace StasisGame.Components
{
    public class InputComponent : IComponent
    {
        private KeyboardState _newKeyState;
        private KeyboardState _oldKeyState;
        private MouseState _newMouseState;
        private MouseState _oldMouseState;

        public ComponentType componentType { get { return ComponentType.Input; } }
        public KeyboardState newKeyState { get { return _newKeyState; } set { _newKeyState = value; } }
        public KeyboardState oldKeyState { get { return _oldKeyState; } set { _oldKeyState = value; } }
        public MouseState newMouseState { get { return _newMouseState; } set { _newMouseState = value; } }
        public MouseState oldMouseState { get { return _oldMouseState; } set { _oldMouseState = value; } }

        public InputComponent()
        {
        }
    }
}
