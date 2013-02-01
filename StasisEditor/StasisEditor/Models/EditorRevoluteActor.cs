using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorRevoluteActor : EditorActor
    {
        private EditorActor _actorA;
        private EditorActor _actorB;
        private Vector2 _controlA;
        private Vector2 _controlB;
        private bool _selectedA;
        private bool _selectedB;
        private bool _moveActor;

        private Vector2 _position;

        public override Vector2 circuitConnectionPosition { get { return _position; } }
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("actor_a", _actorA == null ? -1 : _actorA.id);  // -1 signifies ground body should be used in game
                d.SetAttributeValue("actor_b", _actorB == null ? -1 : _actorB.id);
                return d;
            }
        }

        public EditorRevoluteActor(EditorLevel level)
            : base(level, ActorType.Revolute, level.controller.getUnusedActorID())
        {
            _position = level.controller.worldMouse;
            initializeControls();
            _selectedA = true;
            _selectedB = true;
            _moveActor = true;
        }

        public EditorRevoluteActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _actorA = data.Attribute("actor_a").Value == "-1" ? null : level.getActor(int.Parse(data.Attribute("actor_a").Value));
            _actorB = data.Attribute("actor_b").Value == "-1" ? null : level.getActor(int.Parse(data.Attribute("actor_b").Value));
            initializeControls();
        }

        private void initializeControls()
        {
            if (_actorA == null)
                _controlA = _position + new Vector2(-24f, 0f) / _level.controller.scale;
            if (_actorB == null)
                _controlB = _position + new Vector2(24f, 0f) / _level.controller.scale;
        }

        public override void deselect()
        {
            _selectedA = false;
            _selectedB = false;
            _moveActor = true;
            base.deselect();
        }

        public override void handleMouseDown()
        {
            if (selected)
            {
                if (_selectedA || _selectedB)
                {
                    foreach (EditorActor actor in _level.actors)
                    {
                        if (actor.type == ActorType.Box || actor.type == ActorType.Circle || actor.type == ActorType.Terrain)
                        {
                            if (_selectedA && actor.hitTest(_controlA))
                            {
                                _actorA = actor;
                                break;
                            }
                            else if (_selectedB && actor.hitTest(_controlB))
                            {
                                _actorB = actor;
                                break;
                            }
                        }
                    }
                }
                deselect();
            }
            else
            {
                select();
            }
        }

        public override bool hitTest(Vector2 testPoint)
        {
            Vector2 worldMouse = _level.controller.worldMouse;

            // Hit test controls
            if (_actorA == null)
            {
                if (_level.controller.hitTestPoint(worldMouse, _controlA))
                {
                    _moveActor = false;
                    _selectedA = true;
                    return true;
                }
            }
            if (_actorB == null)
            {
                if (_level.controller.hitTestPoint(worldMouse, _controlB))
                {
                    _moveActor = false;
                    _selectedB = true;
                    return true;
                }
            }

            // Hit test icon
            if (_level.controller.hitTestPoint(worldMouse, _position, 12f))
            {
                _moveActor = true;
                _selectedA = true;
                _selectedB = true;
                return true;
            }

            return false;
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldDeltaMouse;
            if (selected)
            {
                if (!_level.controller.ctrl)
                {
                    if (_moveActor)
                        _position += worldDelta;
                    if (_selectedA)
                        _controlA += worldDelta;
                    if (_selectedB)
                        _controlB += worldDelta;
                }
            }
        }

        public override void draw()
        {
            // Icon
            _level.controller.view.drawPoint(_position, Color.DarkSlateBlue, _layerDepth + 0.0001f);
            _level.controller.view.drawIcon(ActorType.Revolute, _position, _layerDepth);

            // Connections and connections
            if (_actorA == null)
            {
                _level.controller.view.drawLine(_position, _controlA, Color.Gray, _layerDepth);
                _level.controller.view.drawPoint(_controlA, Color.White, _layerDepth);
            }
            else
            {
                _level.controller.view.drawLine(_position, _actorA.revoluteConnectionPosition, Color.Gray * 0.5f, _layerDepth);
            }
            if (_actorB == null)
            {
                _level.controller.view.drawLine(_position, _controlB, Color.DarkGray, _layerDepth);
                _level.controller.view.drawPoint(_controlB, Color.Gray, _layerDepth);
            }
            else
            {
                _level.controller.view.drawLine(_position, _actorB.revoluteConnectionPosition, Color.DarkGray * 0.5f, _layerDepth);
            }
        }
    }
}
