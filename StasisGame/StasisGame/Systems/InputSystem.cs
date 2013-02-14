using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame.Systems
{
    public class InputSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        private KeyboardState _newKeyState;
        private KeyboardState _oldKeyState;
        private MouseState _newMouseState;
        private MouseState _oldMouseState;

        public int defaultPriority { get { return 0; } }
        public SystemType systemType { get { return SystemType.Input; } }

        public InputSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update()
        {
            List<InputComponent> inputComponents = _entityManager.getComponents<InputComponent>(ComponentType.Input);

            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newKeyState = Keyboard.GetState();
            _newMouseState = Mouse.GetState();

            for (int i = 0; i < inputComponents.Count; i++)
            {
                inputComponents[i].newKeyState = _newKeyState;
                inputComponents[i].newMouseState = _newMouseState;
                inputComponents[i].oldKeyState = _oldKeyState;
                inputComponents[i].oldMouseState = _oldMouseState;
            }
        }
    }
}
