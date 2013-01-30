using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorFluidActor : EditorPolygonActor
    {
        public EditorFluidActor(EditorLevel level)
            : base(level, ActorType.Fluid)
        {
        }

        public EditorFluidActor(EditorLevel level, XElement data)
            : base(level, data)
        {
        }

        public override void draw()
        {
            int count = _headPoint.listCount;
            Color lineColor = count > 2 ? Color.Blue : Color.Red;

            PointListNode current = _headPoint;
            while (current != null)
            {
                if (current.next != null)
                    _level.controller.view.drawLine(current.position, current.next.position, lineColor, _layerDepth);
                _level.controller.view.drawPoint(current.position, Color.LightBlue, _layerDepth);
                current = current.next;
            }

            if (count > 2)
            {
                _level.controller.view.drawLine(_headPoint.position, _headPoint.tail.position, Color.Purple, _layerDepth);
            }
        }
    }
}
