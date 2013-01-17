using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    public class RopeActorController : ActorController, IPointSubControllable
    {
        private PointSubController _pointASubController;
        private PointSubController _pointBSubController;

        public override Vector2 connectionPosition { get { return _pointASubController.position; } }
        public override List<ActorProperties> properties
        {
            get { return new List<ActorProperties>(); }
        }

        // Create new
        public RopeActorController(LevelController levelController)
            : base(levelController, levelController.getUnusedActorID())
        {
            _type = StasisCore.ActorType.Rope;

            // Initialize controls
            initializeControls(
                _levelController.getWorldMouse() - new Vector2(1f, 0),
                _levelController.getWorldMouse() + new Vector2(1f, 0));
        }

        // Load from xml
        public RopeActorController(LevelController levelController, XElement data)
            : base(levelController, int.Parse(data.Attribute("id").Value))
        {
            // TODO: Load id

            // TODO: Initialize from xml...
            initializeControls(Vector2.Zero, Vector2.Zero);
        }

        // Initialize controls
        private void initializeControls(Vector2 pointA, Vector2 pointB)
        {
            _pointASubController = new PointSubController(pointA, this);
            _pointBSubController = new PointSubController(pointB, this);
        }

        #region Actor Resource Controller Methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_pointASubController);
            _levelController.selectSubController(_pointBSubController);
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_pointASubController);
            _levelController.deselectSubController(_pointBSubController);
        }

        // hitTest
        public override List<ActorSubController> hitTest(Vector2 worldMouse)
        {
            List<ActorSubController> results = new List<ActorSubController>();

            // Hit test point A
            if (_pointASubController.hitTest(worldMouse))
            {
                results.Add(_pointASubController);
                return results;
            }

            // Hit test point B
            if (_pointBSubController.hitTest(worldMouse))
            {
                results.Add(_pointBSubController);
                return results;
            }

            // Hit test line
            if (_pointASubController.lineHitTest(worldMouse, _pointBSubController.position))
            {
                results.Add(_pointASubController);
                results.Add(_pointBSubController);
                return results;
            }

            return results;
        }

        // draw
        public override void draw()
        {
            // Draw line
            _levelController.view.drawLine(_pointASubController.position, _pointBSubController.position, Color.Tan);

            // Draw points
            _levelController.view.drawPoint(_pointASubController.position, Color.Yellow);
            _levelController.view.drawPoint(_pointBSubController.position, Color.DarkGoldenrod);
        }

        // clone
        public override ActorController clone()
        {
            return new RopeActorController(_levelController);
        }

        #endregion

    }
}
