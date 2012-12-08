using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public class BoxSubController : ActorSubController
    {
        private IBoxSubControllable _actorResourceController;

        public BoxSubController(IBoxSubControllable actorResourceController)
            : base(actorResourceController)
        {
            _actorResourceController = actorResourceController;
        }

        #region Input

        // handleMouseMove
        public override void handleMouseMove()
        {
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
