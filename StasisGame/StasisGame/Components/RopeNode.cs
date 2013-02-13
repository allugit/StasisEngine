using System;
using Box2D.XNA;

namespace StasisGame.Components
{
    public class RopeNode
    {
        private RopeNode _next;
        private RopeNode _previous;
        private Body _body;

        public RopeNode next { get { return _next; } set { _next = value; } }
        public RopeNode previous { get { return _previous; } set { _previous = value; } }
        public Body body { get { return _body; } }

        public RopeNode(Body body)
        {
            _body = body;
        }

        public void insert(RopeNode node)
        {
            if (_next != null)
            {
                _next.previous = node;
            }
            node.previous = this;
            node.next = _next;
            _next = node;
        }

        public void remove()
        {
            if (_previous != null)
            {
                _previous.next = _next;
            }
            if (_next != null)
            {
                _next.previous = _previous;
            }
            _previous = null;
            _next = null;
        }
    }
}
