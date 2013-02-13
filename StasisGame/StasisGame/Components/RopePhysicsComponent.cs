using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class RopePhysicsComponent : IComponent
    {
        private RopeNode _head;

        public ComponentType componentType { get { return ComponentType.RopePhysics; } }
        public RopeNode head { get { return _head; } }

        public RopePhysicsComponent(RopeNode head)
        {
            _head = head;
        }
    }
}
