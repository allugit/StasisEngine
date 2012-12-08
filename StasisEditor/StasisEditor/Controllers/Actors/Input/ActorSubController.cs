using System;
using System.Collections.Generic;

namespace StasisEditor.Controllers.Actors
{
    abstract public class ActorSubController
    {
        protected ILevelController _levelController;

        public ActorSubController(ISubControllable actorController)
        {
            _levelController = actorController.getLevelController();
        }

        // handleMouseMove
        abstract public void handleMouseMove();

        // handleMouseEnterView
        abstract public void handleMouseEnterView();

        // handleMouseLeaveView
        abstract public void handleMouseLeaveView();

        // handleKeyDown
        abstract public void handleKeyDown();

        // handleKeyUp
        abstract public void handleKeyUp();
    }
}
