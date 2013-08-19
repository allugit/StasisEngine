using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.Components
{
    public class LevelTransitionComponent : IComponent
    {
        private string _levelUid;
        private Vector2 _position;
        private float _radius;
        private float _radiusSq;
        private bool _requiresActivation;
        private Vector2 _positionInLevel;

        public ComponentType componentType { get { return ComponentType.LevelTransition; } }
        public string levelUid { get { return _levelUid; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public float radius { get { return _radius; } set { _radius = value; _radiusSq = value * value; } }
        public float radiusSq { get { return _radiusSq; } }
        public bool requiresActivation { get { return _requiresActivation; } set { _requiresActivation = value; } }
        public Vector2 positionInLevel { get { return _positionInLevel; } set { _positionInLevel = value; } }

        public LevelTransitionComponent(string levelUid, Vector2 position, float radius, Vector2 positionInLevel, bool requiresActivation)
        {
            _levelUid = levelUid;
            _position = position;
            this.radius = radius;
            _positionInLevel = positionInLevel;
            _requiresActivation = requiresActivation;
        }
    }
}
