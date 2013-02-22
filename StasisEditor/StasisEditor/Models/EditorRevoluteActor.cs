using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorRevoluteActor : EditorActor, IActorComponent
    {
        private EditorActor _actorA;
        private EditorActor _actorB;
        private PointListNode _controlA;
        private PointListNode _controlB;
        private bool _selectedA;
        private bool _selectedB;
        private bool _moveActor;
        private Vector2 _position;
        private float _lowerAngle;
        private float _upperAngle;
        private bool _enableLimit;
        private float _maxMotorTorque;
        private float _motorSpeed;
        private bool _enableMotor;

        [Browsable(false)]
        public override Vector2 circuitConnectionPosition { get { return _position; } }
        public float lowerAngle { get { return _lowerAngle; } set { _lowerAngle = value; } }
        public float upperAngle { get { return _upperAngle; } set { _upperAngle = value; } }
        public bool enableLimit { get { return _enableLimit; } set { _enableLimit = value; } }
        public float maxMotorTorque { get { return _maxMotorTorque; } set { _maxMotorTorque = value; } }
        public float motorSpeed { get { return _motorSpeed; } set { _motorSpeed = value; } }
        public bool enableMotor { get { return _enableMotor; } set { _enableMotor = value; } }
        [Browsable(false)]
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("actor_a", _actorA == null ? -1 : _actorA.id);  // -1 signifies ground body should be used in game
                d.SetAttributeValue("actor_b", _actorB == null ? -1 : _actorB.id);
                d.SetAttributeValue("lower_angle", _lowerAngle);
                d.SetAttributeValue("upper_angle", _upperAngle);
                d.SetAttributeValue("enable_limit", _enableLimit);
                d.SetAttributeValue("max_motor_torque", _maxMotorTorque);
                d.SetAttributeValue("motor_speed", _motorSpeed);
                d.SetAttributeValue("enable_motor", _enableMotor);
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
            _lowerAngle = Loader.loadFloat(data.Attribute("lower_angle"), 0f);
            _upperAngle = Loader.loadFloat(data.Attribute("upper_angle"), 0f);
            _enableLimit = Loader.loadBool(data.Attribute("enable_limit"), false);
            _maxMotorTorque = Loader.loadFloat(data.Attribute("max_motor_torque"), 0f);
            _motorSpeed = Loader.loadFloat(data.Attribute("motor_speed"), 0f);
            _enableMotor = Loader.loadBool(data.Attribute("enable_motor"), false);
            initializeControls();
        }

        private void initializeControls()
        {
            _controlA = new PointListNode(_position + new Vector2(-24f, 0f) / _level.controller.scale);
            _controlB = new PointListNode(_position + new Vector2(24f, 0f) / _level.controller.scale);
        }

        protected override void deselect()
        {
            _selectedA = false;
            _selectedB = false;
            _moveActor = false;
            base.deselect();
        }

        public override void handleSelectedClick(System.Windows.Forms.MouseButtons button)
        {
            if (_selectedA || _selectedB)
            {
                // Attempt to form a connection with another actor
                foreach (List<EditorActor> actors in _level.sortedActors.Values)
                {
                    foreach (EditorActor actor in actors)
                    {
                        // Only connect with certain actor types
                        if (actor.type == ActorType.Box || actor.type == ActorType.Circle || actor.type == ActorType.Terrain)
                        {
                            if (_selectedA && actor != _actorB)
                            {
                                // Form a connection (actorA)
                                if (actor.hitTest(_controlA.position, (results) =>
                                    {
                                        if (results.Count > 0 && results[0] is EditorActor)
                                        {
                                            _actorA = actor;
                                            return true;
                                        }
                                        return false;
                                    }))
                                {
                                    break;
                                }
                            }
                            else if (_selectedB && actor != _actorA)
                            {
                                // Form a connection (actorB)
                                if (actor.hitTest(_controlB.position, (results) =>
                                    {
                                        if (results.Count > 0 && results[0] is EditorActor)
                                        {
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
                        if (results.Count == 1)
                        {
                            if (results[0] == _controlA)
                            {
                                _moveActor = false;
                                _selectedA = true;
                                select();
                                return true;
                            }
                            else if (results[0] == _controlB)
                            {
                                _moveActor = false;
                                _selectedB = true;
                                select();
                                return true;
                            }
                            else if (results[0] == this)
                            {
                                _moveActor = true;
                                select();
                                return true;
                            }
                        }
                        return false;
                    });
            }
            else if (button == System.Windows.Forms.MouseButtons.Right)
            {
                return hitTest(_level.controller.worldMouse, (results) =>
                {
                    if (results.Count == 1 && !(results[0] is PointListNode))
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

            // Hit test controls
            if (_level.controller.hitTestPoint(testPoint, _controlA.position))
            {
                results.Add(_controlA);
                return callback(results);
            }
            if (_level.controller.hitTestPoint(testPoint, _controlB.position))
            {
                results.Add(_controlB);
                return callback(results);
            }

            // Hit test icon
            if (_level.controller.hitTestPoint(testPoint, _position, 12f))
            {
                results.Add(this);
                return callback(results);
            }

            return false;
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldDeltaMouse;

            if (_actorA != null && !_level.containsActor(_actorA))
            {
                _controlA.position = _actorA.revoluteConnectionPosition;
                _actorA = null;
            }
            if (_actorB != null && !_level.containsActor(_actorB))
            {
                _controlB.position = _actorB.revoluteConnectionPosition;
                _actorB = null;
            }

            if (selected)
            {
                if (!_level.controller.ctrl)
                {
                    if (_moveActor)
                        _position += worldDelta;
                    if (_selectedA)
                        _controlA.position += worldDelta;
                    if (_selectedB)
                        _controlB.position += worldDelta;
                }

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
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
                _level.controller.view.drawLine(_position, _controlA.position, Color.Gray, _layerDepth);
                _level.controller.view.drawPoint(_controlA.position, Color.White, _layerDepth);
            }
            else
            {
                _level.controller.view.drawLine(_position, _actorA.revoluteConnectionPosition, Color.Gray * 0.5f, _layerDepth);
            }
            if (_actorB == null)
            {
                _level.controller.view.drawLine(_position, _controlB.position, Color.DarkGray, _layerDepth);
                _level.controller.view.drawPoint(_controlB.position, Color.Gray, _layerDepth);
            }
            else
            {
                _level.controller.view.drawLine(_position, _actorB.revoluteConnectionPosition, Color.DarkGray * 0.5f, _layerDepth);
            }
        }
    }
}
