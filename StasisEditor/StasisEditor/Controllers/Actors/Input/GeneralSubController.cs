using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public class GeneralSubController : ActorSubController
    {
        private IGeneralSubControllable _actorResourceController;

        public GeneralSubController(IGeneralSubControllable actorResourceController)
            : base(actorResourceController)
        {
            _actorResourceController = actorResourceController;
        }

        #region Input

        // handleMouseMove
        public override void handleMouseMove()
        {
            Vector2 worldMouse = _levelController.getWorldMouse();
            _actorResourceController.setPosition(worldMouse);
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
