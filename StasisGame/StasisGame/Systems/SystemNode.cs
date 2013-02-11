using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.Systems
{
    public class SystemNode
    {
        private SystemNode _next;
        private SystemNode _previous;
        private ISystem _system;
        private int _priority;

        public SystemNode next { get { return _next; } set { _next = value; } }
        public SystemNode previous { get { return _previous; } set { _previous = value; } }
        public ISystem system { get { return _system; } set { _system = value; } }
        public int priority { get { return _priority; } }

        public SystemNode(ISystem system, int priority)
        {
            _system = system;
            _priority = priority;
        }

        public void insert(SystemNode node)
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
            _system = null;
        }
    }
}
