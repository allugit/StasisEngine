using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Poly2Tri;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    abstract public class EditorPolygonActor : EditorActor
    {
        protected PointListNode _headPoint;
        protected List<PointListNode> _selectedPoints;
        protected CustomVertexFormat[] _vertices = new CustomVertexFormat[5000];
        protected int _primitiveCount;
        protected Texture2D _polygonTexture;
        protected Vector2 _polygonPosition;
        protected Color _polygonFill = Color.White;

        virtual protected Color polygonFill { get { return _polygonFill; } }
        public override Vector2 circuitWorldAnchor { get { return _headPoint.position; } }
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

        public EditorPolygonActor(EditorLevel level, ActorType type)
            : base(level, type, level.controller.getUnusedActorID())
        {
            _selectedPoints = new List<PointListNode>();
            _layerDepth = 0.1f;

            Vector2 worldMouse = level.controller.worldMouse;
            _headPoint = new PointListNode(worldMouse + new Vector2(0f, -0.5f));
            _selectedPoints.Add(_headPoint);
            _selectedPoints.Add(_headPoint.tail.insertAfter(worldMouse + new Vector2(-0.5f, 0.5f)));
            _selectedPoints.Add(_headPoint.tail.insertAfter(worldMouse + new Vector2(0.5f, 0.5f)));
            triangulate();
        }

        public EditorPolygonActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            foreach (XElement pointData in data.Elements("Point"))
            {
                if (_headPoint == null)
                    _headPoint = new PointListNode(Loader.loadVector2(pointData, Vector2.Zero));
                else
                    _headPoint.tail.insertAfter(Loader.loadVector2(pointData, Vector2.Zero));
            }
            triangulate();
        }

        public override void deselect()
        {
            _selectedPoints.Clear();
            base.deselect();
        }

        private void triangulate()
        {
            List<PolygonPoint> points = new List<PolygonPoint>();
            PointListNode current = _headPoint;
            _polygonPosition = current.position;
            while (current != null)
            {
                _polygonPosition = Vector2.Min(current.position, _polygonPosition);
                points.Add(new PolygonPoint(current.position.X, current.position.Y));
                current = current.next;
            }
            Polygon polygon = new Polygon(points);
            P2T.Triangulate(polygon);
            _primitiveCount = polygon.Triangles.Count;

            int index = 0;
            foreach (DelaunayTriangle triangle in polygon.Triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    _vertices[index++] = new CustomVertexFormat(
                        new Vector3(triangle.Points[i].Xf, triangle.Points[i].Yf, 0),
                        Vector2.Zero,
                        new Vector3(1f, 1f, 1f));
                }
            }

            _polygonTexture = _level.controller.view.renderPolygon(_vertices, _primitiveCount);
        }

        public override bool hitTest(Vector2 testPoint)
        {
            // Test points
            PointListNode current = _headPoint;
            while (current != null)
            {
                if (_level.controller.hitTestPoint(testPoint, current.position))
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
                if (_level.controller.hitTestLine(testPoint, current.position, next.position))
                {
                    _selectedPoints.Add(current);
                    _selectedPoints.Add(next);
                    return true;
                }

                current = current.next;
            }

            // Test polygon
            if (_level.controller.hitTestPolygon(testPoint, _headPoint.points))
            {
                _selectedPoints.Clear();
                _selectedPoints = _headPoint.allNodes;
                return true;
            }

            return false;
        }

        public override void handleMouseDown()
        {
            if (selected)
            {
                // Continue drawing polygon if shift is held
                if (_level.controller.shift && _selectedPoints.Count == 1)
                {
                    if (_selectedPoints[0] == _headPoint)
                    {
                        _headPoint = _headPoint.insertBefore(_level.controller.worldMouse);
                        _selectedPoints.Clear();
                        _selectedPoints.Add(_headPoint);
                    }
                    else if (_selectedPoints[0] == _headPoint.tail)
                    {
                        _headPoint.tail.insertAfter(_level.controller.worldMouse);
                        _selectedPoints.Clear();
                        _selectedPoints.Add(_headPoint.tail);
                    }
                }
                else
                {
                    triangulate();
                    deselect();
                }
            }
            else
            {
                select();
            }
        }

        public override void update()
        {
            Vector2 worldMouse = _level.controller.worldMouse;
            Vector2 worldDelta = worldMouse - _level.controller.oldWorldMouse;

            if (selected)
            {
                if (!_level.controller.ctrl)
                {
                    foreach (PointListNode node in _selectedPoints)
                        node.position += worldDelta;
                }

                // Insert a point
                if (_level.controller.isKeyPressed(Keys.OemPlus))
                {
                    if (_selectedPoints.Count == 1)
                    {
                        PointListNode newNode = null;
                        if (_selectedPoints[0] == _headPoint)
                        {
                            newNode = _headPoint.insertBefore(worldMouse);
                            _headPoint = newNode;
                        }
                        else if (_selectedPoints[0] == _headPoint.tail)
                        {
                            newNode = _headPoint.tail.insertAfter(worldMouse);
                        }
                        else
                        {
                            newNode = _selectedPoints[0].insertAfter(worldMouse);
                        }

                        _selectedPoints.Clear();
                        _selectedPoints.Add(newNode);
                    }
                    else if (_selectedPoints.Count == 2)
                    {
                        PointListNode firstNode = _selectedPoints[0].index < _selectedPoints[1].index ? _selectedPoints[0] : _selectedPoints[1];
                        PointListNode newNode = firstNode.insertAfter(worldMouse);
                        _selectedPoints.Clear();
                        _selectedPoints.Add(newNode);
                    }
                }

                // Remove a point
                if (_level.controller.isKeyPressed(Keys.OemMinus))
                {
                    if (_headPoint.listCount > 3)
                    {
                        if (_selectedPoints.Count == 1)
                        {
                            PointListNode selectionReplacement = null;
                            if (_selectedPoints[0] == _headPoint)
                            {
                                selectionReplacement = _headPoint.next;
                                _headPoint = selectionReplacement;
                            }
                            else if (_selectedPoints[0] == _headPoint.tail)
                            {
                                selectionReplacement = _headPoint.tail.previous;
                            }
                            else
                            {
                                selectionReplacement = _selectedPoints[0].next ?? _selectedPoints[0].previous;
                            }
                            _selectedPoints[0].remove();
                            _selectedPoints.Clear();
                            selectionReplacement.position = worldMouse;
                            _selectedPoints.Add(selectionReplacement);
                        }
                        else if (_selectedPoints.Count == 2)
                        {
                            if (_headPoint.listCount > 4)
                            {
                                if (_selectedPoints[0] == _headPoint)
                                    _headPoint = _selectedPoints[0].next;
                                else if (_selectedPoints[1] == _headPoint)
                                    _headPoint = _selectedPoints[1].next;

                                _selectedPoints[0].remove();
                                _selectedPoints[1].remove();
                                deselect();
                            }
                        }
                    }
                }

                // Deselect/delete
                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
            else if (_level.controller.selectedActor == null)
            {
                // Insert a point
                if (_level.controller.isKeyPressed(Keys.OemPlus))
                {
                    if (hitTest(worldMouse))
                    {
                        if (_selectedPoints.Count == 1)
                        {
                            // Hit test succeeded on a single point, so add a new node after it
                            _selectedPoints[0].insertAfter(worldMouse);
                        }
                        else if ((_selectedPoints[0] == _headPoint && _selectedPoints[1] == _headPoint.tail) ||
                            (_selectedPoints[0] == _headPoint.tail && _selectedPoints[1] == _headPoint))
                        {
                            // Hit test succeeded on the line between head and tail, so add a point after the tail
                            _headPoint.tail.insertAfter(worldMouse);
                        }
                        else
                        {
                            // Hit test succeeded on a normal line segment, so add a point after the node closest to the head
                            PointListNode firstNode = _selectedPoints[0].index < _selectedPoints[1].index ? _selectedPoints[0] : _selectedPoints[1];
                            firstNode.insertAfter(worldMouse);
                        }
                        _selectedPoints.Clear();
                    }
                }

                // Remove a point
                if (_level.controller.isKeyPressed(Keys.OemMinus))
                {
                    if (_headPoint.listCount > 3 && hitTest(worldMouse))
                    {
                        if (_selectedPoints.Count == 1)
                        {
                            if (_selectedPoints[0] == _headPoint)
                                _headPoint = _headPoint.next;
                            _selectedPoints[0].remove();
                        }
                        _selectedPoints.Clear();
                    }
                }
            }
        }
    }
}
