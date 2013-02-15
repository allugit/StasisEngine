using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using StasisGame.Systems;

namespace StasisGame.Components
{
    public class BodyFocusPointComponent : IComponent
    {
        private Body _body;
        private Vector2 _localPoint;
        private Transform _transform;
        private bool _enabled = true;
        private FocusType _focusType;

        public ComponentType componentType { get { return ComponentType.BodyFocusPoint; } }
        public bool enabled { get { return _enabled; } set { _enabled = value; } }
        public Vector2 focusPoint
        {
            get
            {
                _body.GetTransform(out _transform);
                return MathUtils.Multiply(ref _transform, _localPoint);
            }
        }
        public FocusType focusType { get { return _focusType; } set { _focusType = value; } }

        public BodyFocusPointComponent(Body body, Vector2 localPoint, FocusType focusType)
        {
            _body = body;
            _localPoint = localPoint;
            _focusType = focusType;
        }
    }
}
