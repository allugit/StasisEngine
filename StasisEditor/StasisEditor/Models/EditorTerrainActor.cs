using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    using Keys = System.Windows.Forms.Keys;

    public class EditorTerrainActor : EditorActor
    {
        private PointListNode _headPoint;
        private List<PointListNode> _selectedPoints;

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                PointListNode current = _headPoint;
                while (current != null)
                {
                    d.Add(new XElement("Point", current.position));
                    current = current.next;
                }
                return d;
            }
        }

        public EditorTerrainActor(EditorLevel level)
            : base(level, ActorType.Terrain, level.controller.getUnusedActorID())
        {
            _selectedPoints = new List<PointListNode>();
            _layerDepth = 0.1f;

            Vector2 worldMouse = level.controller.worldMouse;
            _headPoint = new PointListNode(worldMouse + new Vector2(0f, -0.5f));
            _selectedPoints.Add(_headPoint);
            _selectedPoints.Add(_headPoint.tail.insertAfter(worldMouse + new Vector2(-0.5f, 0.5f)));
            _selectedPoints.Add(_headPoint.tail.insertAfter(worldMouse + new Vector2(0.5f, 0.5f)));

        }

        public EditorTerrainActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            foreach (XElement pointData in data.Elements("Point"))
            {
                if (_headPoint == null)
                    _headPoint = new PointListNode(Loader.loadVector2(pointData, Vector2.Zero));
                else
                    _headPoint.tail.insertAfter(Loader.loadVector2(pointData, Vector2.Zero));
            }
        }

        public override void deselect()
        {
            _selectedPoints.Clear();
            base.deselect();
        }

        public override bool hitTest()
        {
            Vector2 worldMouse = _level.controller.worldMouse;

            // Test points
            PointListNode current = _headPoint;
            while (current != null)
            {
                if (_level.controller.hitTestPoint(worldMouse, current.position))
                {
                    _selectedPoints.Add(current);
                    return true;
                }
                current = current.next;
            }

            // Test line segments
            current = _headPoint;
            while (current != null)
            {
                PointListNode next = current.next ?? _headPoint;
                if (_level.controller.hitTestLine(worldMouse, current.position, next.position))
                {
                    _selectedPoints.Add(current);
                    _selectedPoints.Add(next);
                    return true;
                }

                current = current.next;
            }

            return false;
        }

        public override void update()
        {
            Vector2 worldMouse = _level.controller.worldMouse;
            Vector2 worldDelta = worldMouse - _level.controller.oldWorldMouse;

            if (selected)
            {
                foreach (PointListNode node in _selectedPoints)
                    node.position += worldDelta;

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
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
