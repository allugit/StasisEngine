using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
        public RopeNode head
        {
            get
            {
                RopeNode current = this;
                while (current.previous != null)
                    current = current.previous;
                return current;
            }
        }
        public RopeNode tail
        {
            get
            {
                RopeNode current = this;
                while (current.next != null)
                    current = current.next;
                return current;
            }
        }
        public int count
        {
            get
            {
                RopeNode current = head;
                int i = 1;
                while (current.next != null)
                {
                    i++;
                    current = current.next;
                }
                return i;
            }
        }

        public RopeNode(Body body, RevoluteJoint joint)
        {
            _body = body;
        }

        public RopeNode getByIndex(int index)
        {
            int i = 0;
            RopeNode current = head;

            while (current != null)
            {
                if (i == index)
                    return current;

                i++;
                current = current.next;
            }

            return null;
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
