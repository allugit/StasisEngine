using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame
{
    public class RenderablePrimitiveNode
    {
        private RenderablePrimitiveNode _next;
        private RenderablePrimitiveNode _previous;
        private IRenderablePrimitive _renderablePrimitive;

        public RenderablePrimitiveNode next { get { return _next; } set { _next = value; } }
        public RenderablePrimitiveNode previous { get { return _previous; } set { _previous = value; } }
        public IRenderablePrimitive renderablePrimitive { get { return _renderablePrimitive; } set { _renderablePrimitive = value; } }

        public RenderablePrimitiveNode(IRenderablePrimitive renderablePrimitive)
        {
            _renderablePrimitive = renderablePrimitive;
        }

        // Inserts a node after this node
        public void insertAfter(RenderablePrimitiveNode node)
        {
            if (_next != null)
            {
                _next.previous = node;
            }
            node.previous = this;
            node.next = _next;
            _next = node;
        }

        // Inserts a node before this node
        public void insertBefore(RenderablePrimitiveNode node)
        {
            if (_previous != null)
            {
                _previous.next = node;
            }
            node.next = this;
            node.previous = _previous;
            _previous = node;
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
            _renderablePrimitive = null;
        }
    }
}
