using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class PlayerSpawnActorController : ActorController, IPointSubControllable
    {
        private PointSubController _positionSubController;

        public PlayerSpawnActorController(LevelController levelController)
            : base(levelController)
        {
            // Create sub controllers
            _positionSubController = new PointSubController(_levelController.getWorldMouse(), this, 12f);
        }

        #region Input

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

        // globalKeyDown
        public override void globalKeyDown(Keys key)
        {
            // Delete test
            if (_positionSubController.selected && key == Keys.Delete)
                delete();

            base.globalKeyDown(key);
        }

        #endregion

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

        // draw
        public override void draw()
        {
            _levelController.view.drawIcon(StasisCore.ActorType.PlayerSpawn, _positionSubController.position);
        }

        // clone
        public override ActorController clone()
        {
            return new PlayerSpawnActorController(_levelController);
        }

        #endregion
    }
}
