using System;
using System.Collections.Generic;
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
        public int index
        {
            get
            {
                PointListNode current = head;
                int i = 0;
                while (current != null)
                {
                    i++;
                    if (current == this)
                        return i;
                    current = current.next;
                }
                return i;
            }
        }
        public List<Vector2> points
        {
            get
            {
                List<Vector2> results = new List<Vector2>();
                PointListNode current = head;
                while (current != null)
                {
                    results.Add(current.position);
                    current = current.next;
                }
                return results;
            }
        }
        public List<PointListNode> allNodes
        {
            get
            {
                List<PointListNode> all = new List<PointListNode>();
                PointListNode current = head;
                while (current != null)
                {
                    all.Add(current);
                    current = current.next;
                }
                return all;
            }
        }

        public PointListNode(Vector2 position)
        {
            _position = position;
        }

        // Insert new node (n) before this node (c)
        //  a--b   c           c--b--a
        //      \ /           /
        //       n           n
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

        // Remove node from list
        public void remove()
        {
            if (_previous != null)
                _previous.next = _next;
            if (_next != null)
                _next.previous = _previous;

            _previous = null;
            _next = null;
        }
    }
}
