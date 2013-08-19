using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class GateControl : IActorComponent
    {
        private Gate _gate;
        private Vector2 _position;

        public Gate gate { get { return _gate; } }
        public Vector2 position { get { return _position; } set { _position = value; } }

        public GateControl(Gate gate, Vector2 position)
        {
            _gate = gate;
            _position = position;
        }
    }

    public class EditorCircuitActor : EditorActor, IActorComponent
    {
        private EditorCircuit _circuit;
        private List<CircuitConnection> _connections;
        private List<GateControl> _gateControls;
        private List<GateControl> _selectedGateControls;
        private bool _moveActor = true;

        private Vector2 _position;

        [Browsable(false)]
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("circuit_uid", _circuit.uid);
                foreach (CircuitConnection connection in _connections)
                    d.Add(connection.data);
                return d;
            }
        }

        public EditorCircuitActor(EditorLevel level, string circuitUID)
            : base(level, ActorType.Circuit, level.controller.getUnusedActorID())
        {
            _circuit = level.controller.editorController.circuitController.getCircuit(circuitUID);
            _position = _level.controller.worldMouse;
            _connections = new List<CircuitConnection>();
            _layerDepth = 0.1f;
            initializeGateControls();
            selectAllGateControls();
        }

        public EditorCircuitActor(EditorLevel level, XElement data) 
            : base(level, data)
        {
            _circuit = level.controller.editorController.circuitController.getCircuit(data.Attribute("circuit_uid").Value);
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _connections = new List<CircuitConnection>();
            foreach (XElement connectionData in data.Elements("CircuitConnection"))
            {
                EditorActor actor = level.getActor(int.Parse(connectionData.Attribute("actor_id").Value));
                Gate gate = _circuit.getGate(int.Parse(connectionData.Attribute("gate_id").Value));
                string connectionType = connectionData.Attribute("type").Value;

                if (connectionType == "input")
                {
                    GameEventType listenToEvent = (GameEventType)Loader.loadEnum(typeof(GameEventType), connectionData.Attribute("listen_to_event"), 0);
                    _connections.Add(new CircuitInputConnection(this, actor, gate, listenToEvent));
                }
                else if (connectionType == "output")
                {
                    GameEventType onEnabledEvent = (GameEventType)Loader.loadEnum(typeof(GameEventType), connectionData.Attribute("on_enabled_event"), 0);
                    GameEventType onDisabledEvent = (GameEventType)Loader.loadEnum(typeof(GameEventType), connectionData.Attribute("on_disabled_event"), 0);
                    _connections.Add(new CircuitOutputConnection(this, actor, gate, onEnabledEvent, onDisabledEvent));
                }
            }
            initializeGateControls();
        }

        private void initializeGateControls()
        {
            _selectedGateControls = new List<GateControl>();
            _gateControls = new List<GateControl>();

            foreach (Gate gate in _circuit.gates)
            {
                bool createControl = true;
                foreach (CircuitConnection connection in _connections)
                {
                    if (connection.gate.id == gate.id)
                    {
                        createControl = false;
                        break;
                    }
                }
                if (createControl)
                {
                    if (gate.type == "input")
                    {
                        _gateControls.Add(new GateControl(gate, _position + new Vector2(-24f, 24f * _circuit.inputGates.IndexOf(gate)) / _level.controller.scale));
                    }
                    else if (gate.type == "output")
                    {
                        _gateControls.Add(new GateControl(gate, _position + new Vector2(24f, 24f * _circuit.outputGates.IndexOf(gate)) / _level.controller.scale));
                    }
                }
            }
        }

        private void selectAllGateControls()
        {
            foreach (GateControl gateControl in _gateControls)
                _selectedGateControls.Add(gateControl);
        }

        protected override void deselect()
        {
            _selectedGateControls.Clear();
            base.deselect();
        }

        protected override void delete()
        {
            _connections.Clear();
            _gateControls.Clear();
            base.delete();
        }

        public override void handleSelectedClick(System.Windows.Forms.MouseButtons button)
        {
            if (_selectedGateControls.Count == 1)
            {
                // Perform an actor hit test and form a connection if successful
                foreach (List<EditorActor> actors in _level.sortedActors.Values)
                {
                    foreach (EditorActor actor in actors)
                    {
                        if (actor.type != ActorType.Circuit)
                        {
                            bool connectionFormed = actor.hitTest(_selectedGateControls[0].position, (results) =>
                                {
                                    if (results.Count > 0)
                                    {
                                        if (_selectedGateControls[0].gate.type == "input")
                                            _connections.Add(new CircuitInputConnection(this, actor, _selectedGateControls[0].gate, GameEventType.None));
                                        else if (_selectedGateControls[0].gate.type == "output")
                                            _connections.Add(new CircuitOutputConnection(this, actor, _selectedGateControls[0].gate, GameEventType.None, GameEventType.None));

                                        _gateControls.Remove(_selectedGateControls[0]);
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
                        if (results.Count > 0)
                        {
                            if (results[0] is GateControl)
                            {
                                _selectedGateControls.Add(results[0] as GateControl);
                                _moveActor = false;
                                select();
                                return true;
                            }
                            else if (results[0] == this)
                            {
                                selectAllGateControls();
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
                    if (results.Count > 0)
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

            // Hit test gate controls
            foreach (GateControl control in _gateControls)
            {
                if (_level.controller.hitTestPoint(testPoint, control.position))
                {
                    results.Add(control);
                    return callback(results);
                }
            }

            // Hit test icon
            if (_level.controller.hitTestPoint(testPoint, _position, 12f))
            {
                results.Add(this);
                return callback(results);
            }

            // Hit test connection
            foreach (CircuitConnection connection in _connections)
            {
                if (_level.controller.hitTestLine(testPoint, _position, connection.actor.circuitConnectionPosition))
                {
                    results.Add(connection);
                    return callback(results);
                }
            }

            return false;
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldDeltaMouse;

            // Update connections -- Handle deleted actors, etc...
            List<CircuitConnection> connectionsToRemove = new List<CircuitConnection>();
            foreach (CircuitConnection connection in _connections)
            {
                if (!_level.containsActor(connection.actor))
                    connectionsToRemove.Add(connection);
            }
            foreach (CircuitConnection connection in connectionsToRemove)
            {
                _connections.Remove(connection);
                _gateControls.Add(new GateControl(connection.gate, connection.actor.circuitConnectionPosition));
            }

            if (selected)
            {
                if (!_level.controller.isKeyHeld(Keys.LeftControl))
                {
                    if (_moveActor)
                        _position += worldDelta;
                    foreach (GateControl gateControl in _selectedGateControls)
                        gateControl.position += worldDelta;
                }

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            _level.controller.view.drawIcon(ActorType.Circuit, _position, _layerDepth);

            // Gate controls
            foreach (GateControl gateControl in _gateControls)
            {
                Color lineColor = gateControl.gate.type == "input" ? Color.DarkGreen : Color.DarkRed;
                Color dotColor = gateControl.gate.type == "input" ? Color.Green : Color.Red;
                _level.controller.view.drawLine(_position, gateControl.position, lineColor, _layerDepth - 0.0001f);
                _level.controller.view.drawPoint(gateControl.position, dotColor, _layerDepth - 0.0001f);
                _level.controller.view.drawString(gateControl.gate.id.ToString(), gateControl.position, Color.White, 0f);
            }

            // Connections
            foreach (CircuitConnection connection in _connections)
            {
                Color lineColor = (connection.gate.type == "input" ? Color.DarkGreen : Color.DarkRed) * 0.5f;
                Color dotColor = (connection.gate.type == "input" ? Color.Green : Color.Red) * 0.5f;
                _level.controller.view.drawLine(_position, connection.actor.circuitConnectionPosition, lineColor, _layerDepth - 0.0001f);
                _level.controller.view.drawPoint(connection.actor.circuitConnectionPosition, dotColor, _layerDepth - 0.0001f);
                _level.controller.view.drawString(connection.gate.id.ToString(), (_position + connection.actor.circuitConnectionPosition) / 2, Color.White, 0f);
            }
        }
    }
}
