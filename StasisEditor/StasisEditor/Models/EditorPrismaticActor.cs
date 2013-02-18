using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorPrismaticActor : EditorActor, IActorComponent
    {
        private EditorActor _actorA;
        private EditorActor _actorB;
        private PointListNode _connectionA;
        private PointListNode _connectionB;
        private bool _selectedConnectionA;
        private bool _selectedConnectionB;
        private bool _moveActor;
        private float _angle;

        private Vector2 _position;
        private float _lowerLimit;
        private float _upperLimit;

        [Browsable(false)]
        public override Vector2 circuitConnectionPosition { get { return _position; } }
        [Browsable(false)]
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("actor_a", _actorA == null ? -1 : _actorA.id);
                d.SetAttributeValue("actor_b", _actorB == null ? -1 : _actorB.id);
                d.SetAttributeValue("axis", new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle)));
                d.SetAttributeValue("lower_limit", _lowerLimit);
                d.SetAttributeValue("upper_limit", _upperLimit);
                return d;
            }
        }

        public EditorPrismaticActor(EditorLevel level)
            : base(level, ActorType.Prismatic, level.controller.getUnusedActorID())
        {
            _position = level.controller.worldMouse;
            _angle = 0f;
            _lowerLimit = 1f;
            _upperLimit = 1f;
            initializeControls();
            _selectedConnectionA = true;
            _selectedConnectionB = true;
            _moveActor = true;
        }

        public EditorPrismaticActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            int actorIdA = Loader.loadInt(data.Attribute("actor_a"), -1);
            int actorIdB = Loader.loadInt(data.Attribute("actor_b"), -1);
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _actorA = actorIdA == -1 ? null : level.getActor(actorIdA);
            _actorB = actorIdB == -1 ? null : level.getActor(actorIdB);
            Vector2 axis = Loader.loadVector2(data.Attribute("axis"), new Vector2(1, 0));
            _angle = (float)Math.Atan2(axis.Y, axis.X);
            _lowerLimit = Loader.loadFloat(data.Attribute("lower_limit"), 1f);
            _upperLimit = Loader.loadFloat(data.Attribute("upper_limit"), 1f);
            initializeControls();
        }

        private void initializeControls()
        {
            _connectionA = new PointListNode(_position + new Vector2(-24f, 0f) / _level.controller.scale);
            _connectionB = new PointListNode(_position + new Vector2(24f, 0f) / _level.controller.scale);
        }

        protected override void deselect()
        {
            _selectedConnectionA = false;
            _selectedConnectionB = false;
            _moveActor = true;
            base.deselect();
        }

        public override void handleSelectedClick(System.Windows.Forms.MouseButtons button)
        {
            if ((_selectedConnectionA || _selectedConnectionB) && !_moveActor)
            {
                foreach (EditorActor actor in _level.actors)
                {
                    if (actor.type == ActorType.Box || actor.type == ActorType.Circle || actor.type == ActorType.Terrain)
                    {
                        PointListNode connectionControl = _selectedConnectionA ? _connectionA : _connectionB;
                        if (actor.hitTest(connectionControl.position, (results) =>
                            {
                                if (results.Count > 0)
                                {
                                    if (_selectedConnectionA)
                                        _actorA = actor;
                                    else
                                        _actorB = actor;
                                    return true;
                                }
                                return false;
                            }))
                        {
                            break;
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
                        if (results.Count > 0 && results[0] == this)
                        {
                            _moveActor = true;
                            _selectedConnectionA = false;
                            _selectedConnectionB = false;
                            select();
                            return true;
                        }
                        else if (results.Count > 0 && (results[0] == _connectionA || results[0] == _connectionB))
                        {
                            _moveActor = false;
                            if (results[0] == _connectionA)
                                _selectedConnectionA = true;
                            else if (results[0] == _connectionB)
                                _selectedConnectionB = true;
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
                    if (results.Count == 1 && results[0] == this)
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

            // Hit test icon
            if (_level.controller.hitTestPoint(testPoint, _position))
            {
                results.Add(this);
                return callback(results);
            }

            // Hit test connection control
            if (_level.controller.hitTestPoint(testPoint, _connectionA.position))
            {
                results.Add(_connectionA);
                return callback(results);
            }
            else if (_level.controller.hitTestPoint(testPoint, _connectionB.position))
            {
                results.Add(_connectionB);
                return callback(results);
            }

            return false;
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldDeltaMouse;
            float angleIncrement = _level.controller.shift ? 0.0001f : 0.0005f;
            float limitIncrement = _level.controller.shift ? 0.0001f : 0.0005f;

            // Update connections
            if (_actorA != null && !_level.actors.Contains(_actorA))
            {
                _connectionA.position = _actorA.prismaticConnectionPosition;
                _actorA = null;
            }
            if (_actorB != null && !_level.actors.Contains(_actorB))
            {
                _connectionB.position = _actorB.prismaticConnectionPosition;
                _actorB = null;
            }

            if (selected)
            {
                if (!_level.controller.ctrl)
                {
                    if (_moveActor)
                        _position += worldDelta;
                    if (_selectedConnectionA)
                        _connectionA.position += worldDelta;
                    if (_selectedConnectionB)
                        _connectionB.position += worldDelta;
                }

                if (_level.controller.isKeyHeld(Keys.Q))
                    _angle -= angleIncrement;
                if (_level.controller.isKeyHeld(Keys.E))
                    _angle += angleIncrement;
                if (_level.controller.isKeyHeld(Keys.W))
                    _upperLimit += limitIncrement;
                if (_level.controller.isKeyHeld(Keys.S))
                    _upperLimit = Math.Max(_upperLimit - limitIncrement, 0f);
                if (_level.controller.isKeyHeld(Keys.A))
                    _lowerLimit -= limitIncrement;
                if (_level.controller.isKeyHeld(Keys.D))
                    _lowerLimit = Math.Min(_lowerLimit + limitIncrement, 0f);

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            // Icon
            _level.controller.view.drawIcon(_type, _position, _layerDepth);

            // Limits and axis
            Vector2 axis = new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle));
            _level.controller.view.drawLine(_position, _position + axis, Color.Purple, _layerDepth + 0.0001f);
            _level.controller.view.drawLine(_position, _position - axis, Color.Purple, _layerDepth + 0.0001f);
            _level.controller.view.drawLine(_position, _position - axis * _upperLimit, Color.DarkGray, _layerDepth);
            _level.controller.view.drawLine(_position, _position - axis * _lowerLimit, Color.DarkGray, _layerDepth);

            // Connections and controls
            if (_actorA == null)
            {
                _level.controller.view.drawLine(_position, _connectionA.position, Color.DarkGreen, _layerDepth);
                _level.controller.view.drawPoint(_connectionA.position, Color.Green, _layerDepth);
            }
            else
            {
                _level.controller.view.drawLine(_position, _actorA.prismaticConnectionPosition, Color.DarkGreen * 0.5f, _layerDepth);
            }
            if (_actorB == null)
            {
                _level.controller.view.drawLine(_position, _connectionB.position, Color.DarkRed, _layerDepth);
                _level.controller.view.drawPoint(_connectionB.position, Color.Red, _layerDepth);
            }
            else
            {
                _level.controller.view.drawLine(_position, _actorB.prismaticConnectionPosition, Color.DarkRed * 0.5f, _layerDepth);
            }
        }
    }
}
