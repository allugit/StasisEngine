using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.Components
{
    public class AIPaceBehaviorComponent : IComponent
    {
        private List<Vector2> _points;
        private int _currentTargetIndex;

        public ComponentType componentType { get { return ComponentType.AIPaceBehavior; } }
        public List<Vector2> points { get { return _points; } }
        public int currentTargetIndex { get { return _currentTargetIndex; } set { _currentTargetIndex = value; } }

        public AIPaceBehaviorComponent()
        {
        }
    }
}
