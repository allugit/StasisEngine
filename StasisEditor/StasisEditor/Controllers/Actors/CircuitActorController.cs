using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    public class CircuitActorController : ActorController, IPointSubControllable
    {
        private EditorCircuit _circuit;
        private PointSubController _positionSubController;

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
            _positionSubController = new PointSubController(levelController.getWorldMouse(), this);
        }

        // Load from xml
        public CircuitActorController(LevelController levelController, EditorCircuit circuit, XElement data)
            : base(levelController, int.Parse(data.Attribute("id").Value))
        {
            _circuit = circuit;

            throw new NotImplementedException();
        }

        #region Input

        // Hit test
        public override List<ActorSubController> hitTest(Vector2 worldMouse)
        {
            List<ActorSubController> results = new List<ActorSubController>();

            // Test point
            if (_positionSubController.hitTest(worldMouse))
                results.Add(_positionSubController);

            return results;
        }

        #endregion

        #region Point controller

        // Select all sub controllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_positionSubController);
        }

        // Deselect all sub controllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_positionSubController);
        }

        #endregion

        // Draw
        public override void draw()
        {
            _levelController.view.drawIcon(StasisCore.ActorType.Circuit, _positionSubController.position);
        }

        // Clone
        public override ActorController clone()
        {
            return new CircuitActorController(_levelController, _circuit);
        }
    }
}
