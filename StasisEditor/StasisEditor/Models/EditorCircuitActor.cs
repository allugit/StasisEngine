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
    public class GateControl
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

    public class EditorCircuitActor : EditorActor
    {
        private EditorCircuit _circuit;
        private List<CircuitConnection> _connections;
        private List<GateControl> _gateControls;
        private List<GateControl> _markedGateControls;
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
                _connections.Add(new CircuitConnection(this, actor, gate));
            }
            initializeGateControls();
        }

        private void initializeGateControls()
        {
            _markedGateControls = new List<GateControl>();
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
                _markedGateControls.Add(gateControl);
        }

        public override void delete()
        {
            _connections.Clear();
            _gateControls.Clear();
            base.delete();
        }

        public override void handleLeftMouseDown()
        {
            if (selected)
            {
                if (_markedGateControls.Count == 1)
                {
                    // Perform an actor hit test and form a connection if successful
                    foreach (EditorActor actor in _level.actors)
                    {
                        if (actor.type != ActorType.Circuit)
                        {
                            if (actor.hitTest(_markedGateControls[0].position))
                            {
                                _connections.Add(new CircuitConnection(this, actor, _markedGateControls[0].gate));
                                _gateControls.Remove(_markedGateControls[0]);
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

        public override void handleRightMouseDown()
        {
            base.handleRightMouseDown();
        }

        public override bool hitTest(Vector2 testPoint)
        {
            _markedGateControls.Clear();

            // Hit test icon
            if (_level.controller.hitTestPoint(testPoint, _position, 12f))
            {
                _moveActor = true;
                selectAllGateControls();
                return true;
            }

            // Hit test gate controls
            foreach (GateControl control in _gateControls)
            {
                if (_level.controller.hitTestPoint(testPoint, control.position))
                {
                    _moveActor = false;
                    _markedGateControls.Add(control);
                    return true;
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
                if (!_level.actors.Contains(connection.actor))
                    connectionsToRemove.Add(connection);
            }
            foreach (CircuitConnection connection in connectionsToRemove)
            {
                _connections.Remove(connection);
                _gateControls.Add(new GateControl(connection.gate, connection.actor.circuitConnectionPosition));
            }

            if (selected)
            {
                if (!_level.controller.ctrl)
                {
                    if (_moveActor)
                        _position += worldDelta;
                    foreach (GateControl gateControl in _markedGateControls)
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
            }

            // Connections
            foreach (CircuitConnection connection in _connections)
            {
                Color lineColor = (connection.gate.type == "input" ? Color.DarkGreen : Color.DarkRed) * 0.5f;
                Color dotColor = (connection.gate.type == "input" ? Color.Green : Color.Red) * 0.5f;
                _level.controller.view.drawLine(_position, connection.actor.circuitConnectionPosition, lineColor, _layerDepth - 0.0001f);
                _level.controller.view.drawPoint(connection.actor.circuitConnectionPosition, dotColor, _layerDepth - 0.0001f);
            }
        }
    }
}
