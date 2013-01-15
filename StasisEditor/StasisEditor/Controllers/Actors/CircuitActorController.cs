using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Models;
using StasisCore.Models;

namespace StasisEditor.Controllers.Actors
{
    public class CircuitActorController : ActorController, IPointSubControllable
    {
        private EditorCircuit _circuit;
        private PointSubController _positionSubController;
        private Dictionary<int, CircuitConnectionSubController> _inputControllers;

        // Properties
        public override List<ActorProperties> properties
        {
            get { return new List<ActorProperties>(); }
        }

        // Data
        public override XElement data
        {
            get { throw new NotImplementedException(); }
        }

        // Create new
        public CircuitActorController(LevelController levelController, EditorCircuit circuit)
            : base(levelController, levelController.getUnusedActorID())
        {
            _circuit = circuit;
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
            _inputControllers = new Dictionary<int, CircuitConnectionSubController>();
            float offset = 24f / _levelController.getScale();
            for (int i = 0; i < _circuit.gates.Count; i++)
            {
                Gate gate = _circuit.gates[i];
                if (gate.type == "input")
                {
                    _inputControllers[gate.id] = new CircuitConnectionSubController(position + new Vector2(-offset, i * offset), this, gate);
                }
            }
        }

        #region Input

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
            foreach (CircuitConnectionSubController subController in _inputControllers.Values)
            {
                if (subController.hitTest(worldMouse))
                {
                    results.Add(subController);
                    return results;
                }
            }

            return results;
        }

        #endregion

        #region Point controller

        // Select all sub controllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_positionSubController);
            foreach (PointSubController subController in _inputControllers.Values)
                _levelController.selectSubController(subController);
        }

        // Deselect all sub controllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_positionSubController);
            foreach (PointSubController subController in _inputControllers.Values)
                _levelController.deselectSubController(subController);
        }

        #endregion

        // Draw
        public override void draw()
        {
            foreach (PointSubController subController in _inputControllers.Values)
            {
                _levelController.view.drawLine(_positionSubController.position, subController.position, Color.DarkGray);
                _levelController.view.drawPoint(subController.position, Color.Gray);
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
