using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.Components
{
    public class LevelTransitionComponent : IComponent
    {
        private string _levelUid;
        private Vector2 _position;
        private float _halfWidth;
        private float _halfHeight;
        private float _angle;
        private bool _requiresActivation;
        private Vector2 _positionInLevel;

        public ComponentType componentType { get { return ComponentType.LevelTransition; } }
        public string levelUid { get { return _levelUid; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public float halfWidth { get { return _halfWidth; } set { _halfWidth = value; } }
        public float halfHeight { get { return _halfHeight; } set { _halfHeight = value; } }
        public float angle { get { return _angle; } set { _angle = value; } }
        public bool requiresActivation { get { return _requiresActivation; } set { _requiresActivation = value; } }
        public Vector2 positionInLevel { get { return _positionInLevel; } set { _positionInLevel = value; } }

        public LevelTransitionComponent(string levelUid, Vector2 position, float halfWidth, float halfHeight, float angle, Vector2 positionInLevel, bool requiresActivation)
        {
            _levelUid = levelUid;
            _position = position;
            _halfWidth = halfWidth;
            _halfHeight = halfHeight;
            _positionInLevel = positionInLevel;
            _requiresActivation = requiresActivation;
        }
    }
}
