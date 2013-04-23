using System;
using Microsoft.Xna.Framework.Input;
using StasisGame.Managers;

namespace StasisGame.Components
{
    public class InputComponent : IComponent
    {
        public KeyboardState newKeyState;
        public KeyboardState oldKeyState;
        public MouseState newMouseState;
        public MouseState oldMouseState;
        public GamePadState newGamepadState;
        public GamePadState oldGamepadState;
        public bool usingGamepad { get { return DataManager.gameSettings.controllerType == ControllerType.Gamepad && newGamepadState.IsConnected; } }

        public ComponentType componentType { get { return ComponentType.Input; } }

        public InputComponent()
        {
        }
    }
}
