using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    abstract public class EditorPolygonActor : EditorActor
    {
        protected PointListNode _headPoint;
        protected List<PointListNode> _selectedPoints;

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
                    //deselect();
                }

                // Deselect/delete
                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
            else
            {
                // Insert a point
                if (_level.controller.isKeyPressed(Keys.OemPlus))
                {
                    if (hitTest())
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
                    if (_headPoint.listCount > 3 && hitTest())
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
