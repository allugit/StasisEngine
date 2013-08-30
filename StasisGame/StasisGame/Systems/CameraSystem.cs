using System;
using System.Collections.Generic;
using FarseerPhysics.Collision;
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
        private bool _paused;
        private bool _singleStep;

        public int defaultPriority { get { return 80; } }
        public SystemType systemType { get { return SystemType.Camera; } }
        public Vector2 screenCenter { get { return _screenCenter; } set { _screenCenter = value; } }
        public bool enableManualMovement { get { return _enableManualMovement; } set { _enableManualMovement = value; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public CameraSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update(GameTime gameTime)
        {
            string levelUid = LevelSystem.currentLevelUid;
            LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;
            RenderSystem renderSystem = _systemManager.getSystem(SystemType.Render) as RenderSystem;

            if (levelSystem.finalized)
            {
                float speed = 0.1f;
                List<BodyFocusPointComponent> bodyFocusPoints = _entityManager.getComponents<BodyFocusPointComponent>(levelUid, ComponentType.BodyFocusPoint);
                Vector2 singleTarget = _screenCenter;
                Vector2 multipleTarget = Vector2.Zero;
                bool useSingleTarget = false;
                int multipleTargetCount = 0;

                // Handle manual camera movement
                if (_enableManualMovement)
                {
                    if (InputSystem.newKeyState.IsKeyDown(Keys.NumPad8))
                        _screenCenter += new Vector2(0, -speed);
                    if (InputSystem.newKeyState.IsKeyDown(Keys.NumPad2))
                        _screenCenter += new Vector2(0, speed);
                    if (InputSystem.newKeyState.IsKeyDown(Keys.NumPad4))
                        _screenCenter += new Vector2(-speed, 0);
                    if (InputSystem.newKeyState.IsKeyDown(Keys.NumPad6))
                        _screenCenter += new Vector2(speed, 0);
                    //if (InputSystem.newKeyState.IsKeyDown(Keys.F4))
                    //    Console.WriteLine("Screen center: {0}", _screenCenter);
                }

                // Handle camera movement
                if (!_paused || _singleStep)
                {
                    AABB boundary = levelSystem.getBoundary(levelUid);
                    Vector2 scaledHalfScreen = renderSystem.halfScreen / renderSystem.scale;

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

                    // Enforce edge boundaries
                    _screenCenter = Vector2.Max(_screenCenter, boundary.LowerBound + scaledHalfScreen);
                    _screenCenter = Vector2.Min(_screenCenter, boundary.UpperBound - scaledHalfScreen);

                    _singleStep = false;
                }
            }
        }
    }
}
