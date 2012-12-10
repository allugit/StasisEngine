using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Controllers.Actors
{
    public class RopeActorResourceController : ActorResourceController, IPointSubControllable
    {
        private RopeActorResource _ropeActor;
        private PointSubController _pointASubController;
        private PointSubController _pointBSubController;

        public RopeActorResourceController(LevelController levelController, ActorResource actorResource = null)
            : base(levelController)
        {
            // Default actor resource
            if (actorResource == null)
                actorResource = new RopeActorResource(Vector2.Zero, Vector2.Zero);

            _actor = actorResource;
            _ropeActor = actorResource as RopeActorResource;

            // Create sub controllers
            _pointASubController = new PointSubController(_levelController.getWorldMouse() - new Vector2(1f, 0), this);
            _pointBSubController = new PointSubController(_levelController.getWorldMouse() + new Vector2(1f, 0), this);
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
        public override bool hitTest(Vector2 worldMouse)
        {
            // Hit test point A
            if (_pointASubController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_pointASubController);
                return true;
            }

            // Hit test point B
            if (_pointBSubController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_pointBSubController);
                return true;
            }

            // Hit test line
            if (_pointASubController.lineHitTest(worldMouse, _pointBSubController.position))
            {
                _levelController.selectSubController(_pointASubController);
                _levelController.selectSubController(_pointBSubController);
                return true;
            }

            return false;
        }

        // draw
        public override void draw()
        {
            // Draw line
            _renderer.drawLine(_pointASubController.position, _pointBSubController.position, Color.Tan);

            // Draw points
            _renderer.drawPoint(_pointASubController.position, Color.Yellow);
            _renderer.drawPoint(_pointBSubController.position, Color.DarkGoldenrod);
        }

        // clone
        public override ActorResourceController clone()
        {
            return new RopeActorResourceController(_levelController, _ropeActor.clone());
        }

        #endregion

    }
}
