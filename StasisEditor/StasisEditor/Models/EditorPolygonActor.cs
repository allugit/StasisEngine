using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    abstract public class EditorPolygonActor : EditorActor, IActorComponent
    {
        protected PointListNode _headPoint;
        protected List<PointListNode> _selectedPoints;
        protected CustomVertexFormat[] _vertices = new CustomVertexFormat[5000];
        protected int _primitiveCount;
        protected Texture2D _polygonTexture;
        protected Vector2 _polygonPosition;
        protected Color _polygonFill = Color.White;

        [Browsable(false)]
        virtual protected Color polygonFill { get { return _polygonFill; } }
        [Browsable(false)]
        public override Vector2 circuitConnectionPosition { get { return _headPoint.position; } }
        [Browsable(false)]
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
            _selectedPoints = new List<PointListNode>();
            foreach (XElement pointData in data.Elements("Point"))
            {
                if (_headPoint == null)
                    _headPoint = new PointListNode(Loader.loadVector2(pointData, Vector2.Zero));
                else
                    _headPoint.tail.insertAfter(Loader.loadVector2(pointData, Vector2.Zero));
            }
            triangulate();
        }

        protected override void deselect()
        {
            _selectedPoints.Clear();
            base.deselect();
        }

        public void triangulate()
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

            if (_polygonTexture != null)
                _polygonTexture.Dispose();
            _polygonTexture = _level.controller.view.renderPolygon(_vertices, _primitiveCount);
        }

        public override void handleSelectedClick(System.Windows.Forms.MouseButtons button)
        {
            // Continue drawing polygon if shift is held
            if (_level.controller.isKeyHeld(Keys.LeftShift) && _selectedPoints.Count == 1)
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

        public override bool handleUnselectedClick(System.Windows.Forms.MouseButtons button)
        {
            Vector2 worldMouse = _level.controller.worldMouse;
            if (button == System.Windows.Forms.MouseButtons.Left)
            {
                return hitTest(worldMouse, (results) =>
                    {
                        // Handle point results
                        if (results.Count == 1 && results[0] is PointListNode)
                        {
                            _selectedPoints.Add(results[0] as PointListNode);
                            select();
                            return true;
                        }

                        // Handle line results
                        if (results.Count == 2 && results[0] is PointListNode &&
                            results[0] is PointListNode)
                        {
                            _selectedPoints.Add(results[0] as PointListNode);
                            _selectedPoints.Add(results[1] as PointListNode);
                            select();
                            return true;
                        }

                        // Handle polygon results
                        if (results.Count == 1 && results[0] == this)
                        {
                            _selectedPoints = _headPoint.allNodes;
                            select();
                            return true;
                        }

                        return false;
                    });
            }
            else if (button == System.Windows.Forms.MouseButtons.Right)
            {
                return hitTest(_level.controller.worldMouse, (results) =>
                {
                    if (results.Count == 1)
                    {
                        _level.controller.openActorProperties(results[0]);
                        return true;
                    }
                    return false;
                });
            }

            return false;
        }

        public override bool hitTest(Vector2 testPoint, HitTestCallback callback)
        {
            List<IActorComponent> results = new List<IActorComponent>();

            // Test points
            PointListNode current = _headPoint;
            while (current != null)
            {
                if (_level.controller.hitTestPoint(testPoint, current.position))
                {
                    results.Add(current);
                    callback(results);
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
                    results.Add(current);
                    results.Add(next);
                    return callback(results);
                }

                current = current.next;
            }

            // Test polygon
            if (_level.controller.hitTestPolygon(testPoint, _headPoint.points))
            {
                results.Add(this);
                return callback(results);
            }

            return false;
        }

        public override void update()
        {
            Vector2 worldMouse = _level.controller.worldMouse;
            Vector2 worldDelta = worldMouse - _level.controller.oldWorldMouse;

            if (selected)
            {
                if (!_level.controller.isKeyHeld(Keys.LeftControl))
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
                    hitTest(worldMouse, (results) =>
                        {
                            // Handle insertion on a single point
                            if (results.Count == 1 && results[0] is PointListNode)
                            {
                                (results[0] as PointListNode).insertAfter(worldMouse);
                                return true;
                            }

                            // Handle insertion on line between head and tail
                            if (results.Count == 2 && 
                                ((results[0] == _headPoint && results[1] == _headPoint.tail) ||
                                (results[0] == _headPoint.tail && results[1] == _headPoint)))
                            {
                                _headPoint.tail.insertAfter(worldMouse);
                                return true;
                            }

                            // Handle insertion on a normal line segment
                            if (results.Count == 2 && results[0] is PointListNode && results[0] is PointListNode)
                            {
                                PointListNode firstNode = ((results[0] as PointListNode).index < (results[1] as PointListNode).index ? results[0] : results[1]) as PointListNode;
                                firstNode.insertAfter(worldMouse);
                            }

                            return false;
                        });
                }

                // Remove a point
                if (_level.controller.isKeyPressed(Keys.OemMinus))
                {
                    if (_headPoint.listCount > 3)
                    {
                        hitTest(worldMouse, (results) =>
                            {
                                if (results.Count == 1 && results[0] is PointListNode)
                                {
                                    if (results[0] == _headPoint)
                                        _headPoint = _headPoint.next;
                                    (results[0] as PointListNode).remove();
                                    return true;
                                }
                                return false;
                            });
                    }
                }
            }
        }
    }
}
