using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorCollisionFilterActor : EditorActor
    {
        private PointListNode _nodeA;
        private PointListNode _nodeB;
        private List<PointListNode> _selectedPoints;
        private EditorActor _actorA;
        private EditorActor _actorB;

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("actor_a", _actorA == null ? -1 : _actorA.id);
                d.SetAttributeValue("actor_b", _actorB == null ? -1 : _actorB.id);
                return d;
            }
        }

        public EditorCollisionFilterActor(EditorLevel level)
            : base(level, ActorType.CollisionFilter, level.getUnusedActorId())
        {
            Vector2 worldMouse = _level.controller.worldMouse;
            _selectedPoints = new List<PointListNode>();
            initializeControls(worldMouse + new Vector2(0f, -0.5f), worldMouse + new Vector2(0f, 0.5f));
            _selectedPoints.Add(_nodeA);
            _selectedPoints.Add(_nodeB);
            _layerDepth = 0.1f;
        }

        public EditorCollisionFilterActor(EditorLevel level, XElement data) 
            : base(level, data)
        {
            _selectedPoints = new List<PointListNode>();
            if (data.Attribute("actor_a").Value != "-1")
                _actorA = level.getActor(int.Parse(data.Attribute("actor_a").Value));
            if (data.Attribute("actor_b").Value != "-1")
                _actorB = level.getActor(int.Parse(data.Attribute("actor_b").Value));
            initializeControls(Vector2.Zero, Vector2.Zero);
        }

        private void initializeControls(Vector2 pointA, Vector2 pointB)
        {
            _nodeA = new PointListNode(pointA);
            _nodeB = new PointListNode(pointB);
        }

        protected override void deselect()
        {
            _selectedPoints.Clear();
            base.deselect();
        }

        public override void handleSelectedClick(System.Windows.Forms.MouseButtons button)
        {
            if (_selectedPoints.Count == 1)
            {
                // Perform an actor hit test and form a connection if successful
                foreach (List<EditorActor> actors in _level.sortedActors.Values)
                {
                    foreach (EditorActor actor in actors)
                    {
                        if (actor.type == ActorType.Box || actor.type == ActorType.Circle || actor.type == ActorType.Terrain)
                        {
                            bool connectionFormed = actor.hitTest(_selectedPoints[0].position, (results) =>
                            {
                                if (results.Count > 0)
                                {
                                    if (_selectedPoints[0] == _nodeA)
                                        _actorA = actor;
                                    else if (_selectedPoints[0] == _nodeB)
                                        _actorB = actor;

                                    return true;
                                }
                                return false;
                            });

                            if (connectionFormed)
                            {
                                deselect();
                                return;
                            }
                        }
                    }
                }
            }
            deselect();
        }

        public override bool handleUnselectedClick(System.Windows.Forms.MouseButtons button)
        {
            if (button == System.Windows.Forms.MouseButtons.Left)
            {
                return hitTest(_level.controller.worldMouse, (results) =>
                {
                    if (results.Count == 1 && results[0] is PointListNode)
                    {
                        _selectedPoints.Add(results[0] as PointListNode);
                        select();
                        return true;
                    }
                    else if (results.Count == 2 && _actorA == null && _actorB == null)
                    {
                        foreach (IActorComponent component in results)
                        {
                            if (component is PointListNode)
                                _selectedPoints.Add(component as PointListNode);
                        }
                        select();
                        return true;
                    }
                    else if (results.Count == 2 && _actorA != null && _actorB != null)
                    {
                        _nodeA.position = _actorA.collisionFilterConnectionPosition;
                        _nodeB.position = _actorB.collisionFilterConnectionPosition;
                        _actorA = null;
                        _actorB = null;
                        _selectedPoints.Add(_nodeA);
                        _selectedPoints.Add(_nodeB);
                        select();
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
            Vector2 pointA = _actorA == null ? _nodeA.position : _actorA.collisionFilterConnectionPosition;
            Vector2 pointB = _actorB == null ? _nodeB.position : _actorB.collisionFilterConnectionPosition;

            // Hit test points
            if (_level.controller.hitTestPoint(testPoint, pointA))
            {
                if (_actorA == null)
                {
                    results.Add(_nodeA);
                    return callback(results);
                }
            }
            else if (_level.controller.hitTestPoint(testPoint, pointB))
            {
                if (_actorB == null)
                {
                    results.Add(_nodeB);
                    return callback(results);
                }
            }
            
            if (_level.controller.hitTestLine(testPoint, pointA, pointB))
            {
                results.Add(_nodeA);
                results.Add(_nodeB);
                return callback(results);
            }

            return false;
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldMouse - _level.controller.oldWorldMouse;

            if (selected)
            {
                if (!_level.controller.isKeyHeld(Keys.LeftControl))
                {
                    foreach (PointListNode node in _selectedPoints)
                        node.position += worldDelta;
                }

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            Vector2 pointA = _actorA == null ? _nodeA.position : _actorA.collisionFilterConnectionPosition;
            Vector2 pointB = _actorB == null ? _nodeB.position : _actorB.collisionFilterConnectionPosition;
            _level.controller.view.drawLine(pointA, pointB, Color.Red, _layerDepth);
            _level.controller.view.drawPoint(pointA, Color.Pink, _layerDepth);
            _level.controller.view.drawPoint(pointB, Color.Pink, _layerDepth);
        }
    }
}
