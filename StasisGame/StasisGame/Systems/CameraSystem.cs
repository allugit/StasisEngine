using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public enum FocusType
    {
        Single,
        Multiple
    };

    public class CameraSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private Vector2 _screenCenter;
        private bool _enableManualMovement = true;
        private KeyboardState _keyState;
        private KeyboardState _oldKeyState;
        private bool _paused;
        private bool _singleStep;

        public int defaultPriority { get { return 80; } }
        public SystemType systemType { get { return SystemType.Camera; } }
        public Vector2 screenCenter { get { return _screenCenter; } }
        public bool enableManualMovement { get { return _enableManualMovement; } set { _enableManualMovement = value; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public CameraSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update()
        {
            if (_enableManualMovement)
            {
                float speed = 0.1f;

                _keyState = Keyboard.GetState();

                if (_keyState.IsKeyDown(Keys.NumPad8))
                    _screenCenter += new Vector2(0, -speed);
                if (_keyState.IsKeyDown(Keys.NumPad2))
                    _screenCenter += new Vector2(0, speed);
                if (_keyState.IsKeyDown(Keys.NumPad4))
                    _screenCenter += new Vector2(-speed, 0);
                if (_keyState.IsKeyDown(Keys.NumPad6))
                    _screenCenter += new Vector2(speed, 0);

                _oldKeyState = _keyState;
            }
            else
            {
                List<BodyFocusPointComponent> bodyFocusPoints = _entityManager.getComponents<BodyFocusPointComponent>(ComponentType.BodyFocusPoint);
                Vector2 singleTarget = _screenCenter;
                Vector2 multipleTarget = Vector2.Zero;
                bool useSingleTarget = false;
                int multipleTargetCount = 0;

                for (int i = 0; i < bodyFocusPoints.Count; i++)
                {
                    if (bodyFocusPoints[i].focusType == FocusType.Multiple)
                    {
                        multipleTarget += bodyFocusPoints[i].focusPoint;
                        multipleTargetCount++;
                    }
                    else if (bodyFocusPoints[i].focusType == FocusType.Single)
                    {
                        singleTarget = bodyFocusPoints[i].focusPoint;
                        useSingleTarget = true;
                        break;
                    }
                }

                if (useSingleTarget)
                    _screenCenter += (singleTarget - _screenCenter) / 2f;
                else
                    _screenCenter += (multipleTarget / multipleTargetCount - _screenCenter) / 2f;
            }
        }
    }
}
