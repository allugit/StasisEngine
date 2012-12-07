using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public class GeneralActorInputController : ActorInputController
    {
        private IGeneralActorController _generalActorController;

        public GeneralActorInputController(IGeneralActorController generalActorController)
            : base(generalActorController)
        {
            _generalActorController = generalActorController;
        }

        #region Input

        // handleMouseMove
        public override void handleMouseMove()
        {
            Vector2 worldMouse = _levelController.getWorldMouse();
            _generalActorController.setPosition(worldMouse);
        }

        // handleMouseEnterView
        public override void handleMouseEnterView()
        {
        }

        // handleMouseLeaveView
        public override void handleMouseLeaveView()
        {
        }

        // handleKeyDown
        public override void handleKeyDown()
        {
        }

        // handleKeyUp
        public override void handleKeyUp()
        {
        }

        #endregion
    }
}
