using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    public class EditorTerrainActor : EditorPolygonActor
    {
        public EditorTerrainActor(EditorLevel level)
            : base(level, ActorType.Terrain)
        {
        }

        public EditorTerrainActor(EditorLevel level, XElement data)
            : base(level, data)
        {
        }

        public override void draw()
        {
            // Draw primitive
            if (_primitiveCount > 0)
                _level.controller.view.drawPolygon(_vertices, _primitiveCount);
            
            // Draw lines and points
            int count = _headPoint.listCount;
            Color lineColor = count > 2 ? Color.Orange : Color.Red;
            PointListNode current = _headPoint;
            while (current != null)
            {
                if (current.next != null)
                    _level.controller.view.drawLine(current.position, current.next.position, lineColor, _layerDepth);
                _level.controller.view.drawPoint(current.position, Color.Yellow, _layerDepth);
                current = current.next;
            }
            if (count > 2)
            {
                _level.controller.view.drawLine(_headPoint.position, _headPoint.tail.position, Color.Purple, _layerDepth);
            }
        }
    }
}
