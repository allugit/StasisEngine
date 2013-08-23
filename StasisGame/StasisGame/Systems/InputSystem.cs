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
        private bool _paused;
        private bool _singleStep;

        public static KeyboardState newKeyState;
        public static KeyboardState oldKeyState;
        public static MouseState newMouseState;
        public static MouseState oldMouseState;
        public static GamePadState newGamepadState;
        public static GamePadState oldGamepadState;
        public static Vector2 worldMouse;

        public int defaultPriority { get { return 1; } }
        public SystemType systemType { get { return SystemType.Input; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public static bool usingGamepad { get { return DataManager.gameSettings.controllerType == ControllerType.Gamepad && newGamepadState.IsConnected; } }

        public InputSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update(GameTime gameTime)
        {
            RenderSystem renderSystem = (RenderSystem)_systemManager.getSystem(SystemType.Render);
            bool togglePause = newKeyState.IsKeyDown(Keys.F3) && oldKeyState.IsKeyUp(Keys.F3);
            bool toggleSingleStep = newKeyState.IsKeyDown(Keys.F2) && oldKeyState.IsKeyUp(Keys.F2);
            bool toggleDebug = newKeyState.IsKeyDown(Keys.F1) && oldKeyState.IsKeyUp(Keys.F1);

            oldKeyState = newKeyState;
            oldMouseState = newMouseState;
            oldGamepadState = newGamepadState;

            newKeyState = Keyboard.GetState();
            newMouseState = Mouse.GetState();
            newGamepadState = GamePad.GetState(PlayerIndex.One);

            if (renderSystem != null)
                worldMouse = (new Vector2(newMouseState.X, newMouseState.Y) - renderSystem.halfScreen) / renderSystem.scale + renderSystem.screenCenter;

            if (togglePause || toggleSingleStep)
            {
                SystemNode node = _systemManager.head;
                while (node != null)
                {
                    if (node.system.systemType != SystemType.Render)
                    {
                        if (togglePause)
                            node.system.paused = !node.system.paused;

                        if (toggleSingleStep)
                            node.system.singleStep = true;
                    }

                    node = node.next;
                }
            }

            if (toggleDebug)
                LoderGame.debug = !LoderGame.debug;
        }
    }
}
