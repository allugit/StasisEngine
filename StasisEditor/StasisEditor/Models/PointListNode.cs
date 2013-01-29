using System;
using Microsoft.Xna.Framework;

namespace StasisEditor.Models
{
    public class PointListNode
    {
        private PointListNode _next;
        private PointListNode _previous;
        private Vector2 _position;

        public PointListNode next { get { return _next; } set { _next = value; } }
        public PointListNode previous { get { return _previous; } set { _previous = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public PointListNode head
        {
            get 
            {
                PointListNode current = this;
                while (current.previous != null)
                    current = current.previous;
                return current;
            }
        }
        public PointListNode tail
        {
            get
            {
                PointListNode current = this;
                while (current.next != null)
                    current = current.next;
                return current;
            }
        }
        public int listCount
        {
            get
            {
                PointListNode current = head;
                int i = 0;
                while (current != null)
                {
                    i++;
                    current = current.next;
                }
                return i;
            }
        }

        public PointListNode(Vector2 position)
        {
            _position = position;
        }

        // Insert new node (n) before this node (c)
        //  a--b   c
        //      \ /
        //       n
        public PointListNode insertBefore(Vector2 position)
        {
            PointListNode n = new PointListNode(position);

            // Wire up new node
            n.previous = _previous;
            n.next = this;

            // Rewire existing links
            if (_previous != null)
                _previous.next = n;
            _previous = n;

            return n;
        }

        // Insert new node (n) after this node (b)
        //  a--b   c
        //      \ /
        //       n
        public PointListNode insertAfter(Vector2 position)
        {
            PointListNode n = new PointListNode(position);

            // Wire up new node
            n.previous = this;
            n.next = _next;

            // Rewire existing links
            if (_next != null)
                _next.previous = n;
            _next = n;

            return n;
        }
    }
}
