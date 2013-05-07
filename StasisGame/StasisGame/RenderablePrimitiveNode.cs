using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame
{
    public class RenderablePrimitiveNode
    {
        private RenderablePrimitiveNode _next;
        private IRenderablePrimitive _renderablePrimitive;

        public RenderablePrimitiveNode next { get { return _next; } set { _next = value; } }
        public IRenderablePrimitive renderablePrimitive { get { return _renderablePrimitive; } set { _renderablePrimitive = value; } }

        public RenderablePrimitiveNode(IRenderablePrimitive renderablePrimitive)
        {
            _renderablePrimitive = renderablePrimitive;
        }
    }
}
