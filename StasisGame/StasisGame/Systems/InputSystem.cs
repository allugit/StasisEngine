using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame.Systems
{
    public class InputSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        public static KeyboardState newKeyState;
        public static KeyboardState oldKeyState;
        public static MouseState newMouseState;
        public static MouseState oldMouseState;
        public static GamePadState newGamepadState;
        public static GamePadState oldGamepadState;
        public static Vector2 worldMouse;

        public int defaultPriority { get { return 1; } }
        public SystemType systemType { get { return SystemType.Input; } }

        public InputSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update()
        {
            List<InputComponent> inputComponents = _entityManager.getComponents<InputComponent>(ComponentType.Input);
            RenderSystem renderSystem = (RenderSystem)_systemManager.getSystem(SystemType.Render);

            oldKeyState = newKeyState;
            oldMouseState = newMouseState;
            oldGamepadState = newGamepadState;

            newKeyState = Keyboard.GetState();
            newMouseState = Mouse.GetState();
            newGamepadState = GamePad.GetState(PlayerIndex.One);

            worldMouse = (new Vector2(newMouseState.X, newMouseState.Y) - renderSystem.halfScreen) / renderSystem.scale + renderSystem.screenCenter;

            for (int i = 0; i < inputComponents.Count; i++)
            {
                inputComponents[i].newKeyState = newKeyState;
                inputComponents[i].newMouseState = newMouseState;
                inputComponents[i].newGamepadState = newGamepadState;
                inputComponents[i].oldKeyState = oldKeyState;
                inputComponents[i].oldMouseState = oldMouseState;
                inputComponents[i].oldGamepadState = oldGamepadState;
            }
        }
    }
}
