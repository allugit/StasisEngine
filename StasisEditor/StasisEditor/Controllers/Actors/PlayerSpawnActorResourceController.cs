using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Controllers.Actors
{
    public class PlayerSpawnActorResourceController : ActorResourceController, IPointSubControllable
    {
        private PointSubController _positionSubController;

        public PlayerSpawnActorResourceController(ILevelController levelController, ActorResource actorResource = null)
            : base(levelController)
        {
            // Default player spawn actor resource
            if (actorResource == null)
                actorResource = new PlayerSpawnActorResource(Vector2.Zero);

            _actor = actorResource;

            // Create sub controllers
            _positionSubController = new PointSubController(_levelController.getWorldMouse(), this, 12f);
        }

        #region Actor Resource Controller Methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_positionSubController);
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_positionSubController);
        }

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            // Hit test
            if (_positionSubController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_positionSubController);
                return true;
            }

            return false;
        }

        // draw
        public override void draw()
        {
            // Draw icon
            _renderer.drawIcon(_actor.type, _positionSubController.position);
        }

        // clone
        public override ActorResourceController clone()
        {
            return new PlayerSpawnActorResourceController(_levelController, _actor.clone());
        }

        #endregion
    }
}
