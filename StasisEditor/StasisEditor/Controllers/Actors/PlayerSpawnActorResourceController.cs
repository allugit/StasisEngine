using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore.Resources;

namespace StasisEditor.Controllers.Actors
{
    public class PlayerSpawnActorResourceController : ActorResourceController, IPointSubControllable
    {
        private PointSubController _positionSubController;

        public PlayerSpawnActorResourceController(LevelController levelController, ActorResource actorResource = null)
            : base(levelController)
        {
            // Default player spawn actor resource
            if (actorResource == null)
                actorResource = new PlayerSpawnActorResource(Vector2.Zero);

            _actor = actorResource;

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

        // globalCheckKeys
        public override void globalCheckKey()
        {
            // Delete test
            if (_positionSubController.selected && Input.newKey.IsKeyDown(Keys.Delete) && Input.oldKey.IsKeyUp(Keys.Delete))
                delete();
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
        public override ActorResourceController clone()
        {
            return new PlayerSpawnActorResourceController(_levelController, _actor.clone());
        }

        #endregion
    }
}
