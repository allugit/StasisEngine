using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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

        public int defaultPriority { get { return 80; } }
        public SystemType systemType { get { return SystemType.Camera; } }
        public Vector2 screenCenter { get { return _screenCenter; } }

        public CameraSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update()
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
