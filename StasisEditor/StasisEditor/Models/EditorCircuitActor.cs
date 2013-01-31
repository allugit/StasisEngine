using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
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
        private List<GateControl> _selectedGateControls;
        private bool _moveActor = true;

        private Vector2 _position;

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
        }

        public EditorCircuitActor(EditorLevel level, XElement data) 
            : base(level, data)
        {
            _circuit = level.controller.editorController.circuitController.getCircuit(data.Attribute("circuit_uid").Value);
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _connections = new List<CircuitConnection>();
            foreach (XElement connectionData in data.Elements("CircuitConnection"))
            {
                EditorActor actor = level.controller.getActor(int.Parse(connectionData.Attribute("actor_id").Value));
                Gate gate = _circuit.getGate(int.Parse(connectionData.Attribute("gate_id").Value));
                _connections.Add(new CircuitConnection(this, actor, gate));
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

            selectAllGateControls();
        }

        private void selectAllGateControls()
        {
            foreach (GateControl gateControl in _gateControls)
                _selectedGateControls.Add(gateControl);
        }

        public override void deselect()
        {
            _moveActor = true;
            _selectedGateControls.Clear();
            base.deselect();
        }

        public override bool hitTest()
        {
            Vector2 worldMouse = _level.controller.worldMouse;

            // Hit test icon
            if (_level.controller.hitTestPoint(worldMouse, _position, 12f))
            {
                _moveActor = true;
                selectAllGateControls();
                return true;
            }

            // Hit test gate controls
            foreach (GateControl control in _gateControls)
            {
                if (_level.controller.hitTestPoint(worldMouse, control.position))
                {
                    _moveActor = false;
                    _selectedGateControls.Add(control);
                    return true;
                }
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
                    foreach (GateControl gateControl in _selectedGateControls)
                        gateControl.position += worldDelta;
                }
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
        }
    }
}
