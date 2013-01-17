using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Models;
using StasisCore.Models;
using StasisCore;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class CircuitActorController : ActorController, IPointSubControllable
    {
        private EditorCircuit _circuit;
        private PointSubController _positionSubController;
        private List<CircuitConnectionSubController> _inputControllers;
        private List<CircuitConnectionSubController> _outputControllers;

        public override Vector2 connectionPosition { get { return _positionSubController.position; } }
        public override List<ActorProperties> properties
        {
            get { return new List<ActorProperties>(); }
        }

        // Create new
        public CircuitActorController(LevelController levelController, EditorCircuit circuit)
            : base(levelController, levelController.getUnusedActorID())
        {
            _circuit = circuit;
            _type = ActorType.Circuit;
            initializeSubControllers(levelController.getWorldMouse());
        }

        // Load from xml
        public CircuitActorController(LevelController levelController, EditorCircuit circuit, XElement data)
            : base(levelController, int.Parse(data.Attribute("id").Value))
        {
            throw new NotImplementedException();
        }

        // Initialize sub controllers
        private void initializeSubControllers(Vector2 position)
        {
            _positionSubController = new PointSubController(position, this);
            _inputControllers = new List<CircuitConnectionSubController>();
            _outputControllers = new List<CircuitConnectionSubController>();

            float offset = 24f / _levelController.getScale();
            for (int i = 0; i < _circuit.inputGates.Count; i++)
            {
                Gate gate = _circuit.inputGates[i];
                _inputControllers.Add(new CircuitConnectionSubController(position + new Vector2(-offset, i * offset), this, gate));
            }
            for (int i = 0; i < _circuit.outputGates.Count; i++)
            {
                Gate gate = _circuit.outputGates[i];
                _outputControllers.Add(new CircuitConnectionSubController(position + new Vector2(offset, i * offset), this, gate));
            }
        }

        // Update connections
        public void updateConnections()
        {
            // Preserve connections where the gate still exists in the circuit
            List<CircuitConnectionSubController> connectionsToRemove = new List<CircuitConnectionSubController>();
            foreach (CircuitConnectionSubController connection in _inputControllers)
            {
                if (!_circuit.gates.Contains(connection.gate))
                    connectionsToRemove.Add(connection);
            }
            foreach (CircuitConnectionSubController connection in connectionsToRemove)
            {
                connection.disconnect();
                _inputControllers.Remove(connection);
            }
            connectionsToRemove.Clear();
            foreach (CircuitConnectionSubController connection in _outputControllers)
            {
                if (!_circuit.gates.Contains(connection.gate))
                    connectionsToRemove.Add(connection);
            }
            foreach (CircuitConnectionSubController connection in connectionsToRemove)
            {
                connection.disconnect();
                _outputControllers.Remove(connection);
            }

            // Initialize new connections for new gates
            float offset = 24f / _levelController.getScale();
            List<Gate> existingGates = new List<Gate>(from CircuitConnectionSubController connection in _inputControllers select connection.gate);
            for (int i = 0; i < _circuit.inputGates.Count; i++)
            {
                Gate gate = _circuit.inputGates[i];
                if (!existingGates.Contains(gate))
                    _inputControllers.Add(new CircuitConnectionSubController(_positionSubController.position + new Vector2(-offset, i * offset), this, gate));
            }
            existingGates = new List<Gate>(from CircuitConnectionSubController connection in _outputControllers select connection.gate);
            for (int i = 0; i < _circuit.outputGates.Count; i++)
            {
                Gate gate = _circuit.outputGates[i];
                if (!existingGates.Contains(gate))
                    _outputControllers.Add(new CircuitConnectionSubController(_positionSubController.position + new Vector2(offset, i * offset), this, gate));
            }
        }

        #region Input

        // Handle left mouse down
        public override bool handleLeftMouseDown(List<ActorSubController> results)
        {
            // Disable selection of connected CircuitConnectionControllers
            if (results.Count > 0)
            {
                if (results[0] is CircuitConnectionSubController && (results[0] as CircuitConnectionSubController).connected)
                    return false;
                else
                    return base.handleLeftMouseDown(results);
            }
            else
                return false;
        }

        // Handle right mouse down
        public override bool handleRightMouseDown(List<ActorSubController> results)
        {
            if (results.Count > 0)
            {
                _levelController.closeActorProperties();
                if (results[0] is CircuitConnectionSubController)
                {
                    _levelController.openActorProperties((results[0] as CircuitConnectionSubController).properties);
                }
                else
                {
                    _levelController.openActorProperties(properties);
                }
                return true;
            }

            return false;
        }

        // Hit test
        public override List<ActorSubController> hitTest(Vector2 worldMouse)
        {
            List<ActorSubController> results = new List<ActorSubController>();

            // Test point
            if (_positionSubController.hitTest(worldMouse))
            {
                results.Add(_positionSubController);
                return results;
            }

            // Test connections
            foreach (CircuitConnectionSubController subController in _inputControllers)
            {
                if (subController.hitTest(worldMouse) || subController.lineHitTest(worldMouse, _positionSubController.position))
                {
                    results.Add(subController);
                    return results;
                }
            }
            foreach (CircuitConnectionSubController subController in _outputControllers)
            {
                if (subController.hitTest(worldMouse) || subController.lineHitTest(worldMouse, _positionSubController.position))
                {
                    results.Add(subController);
                    return results;
                }
            }

            return results;
        }

        // globalCheckKeys
        public override void globalKeyDown(Keys key)
        {
            // Delete test
            if (_positionSubController.selected && key == Keys.Delete)
                delete();

            base.globalKeyDown(key);
        }

        #endregion

        #region Point controller

        // Select all sub controllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_positionSubController);
            foreach (PointSubController subController in _inputControllers)
                _levelController.selectSubController(subController);
            foreach (PointSubController subController in _outputControllers)
                _levelController.selectSubController(subController);
        }

        // Deselect all sub controllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_positionSubController);
            foreach (PointSubController subController in _inputControllers)
                _levelController.deselectSubController(subController);
            foreach (PointSubController subController in _outputControllers)
                _levelController.deselectSubController(subController);
        }

        #endregion

        // Draw
        public override void draw()
        {
            foreach (PointSubController subController in _inputControllers)
            {
                _levelController.view.drawLine(_positionSubController.position, subController.position, Color.DarkGreen);
                _levelController.view.drawPoint(subController.position, Color.Green);
            }
            foreach (PointSubController subController in _outputControllers)
            {
                _levelController.view.drawLine(_positionSubController.position, subController.position, Color.DarkRed);
                _levelController.view.drawPoint(subController.position, Color.Red);
            }
            _levelController.view.drawIcon(StasisCore.ActorType.Circuit, _positionSubController.position);
        }

        // Clone
        public override ActorController clone()
        {
            return new CircuitActorController(_levelController, _circuit);
        }
    }
}
