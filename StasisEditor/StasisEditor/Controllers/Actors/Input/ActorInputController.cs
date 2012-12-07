using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisEditor.Controllers.Actors
{
    abstract public class ActorInputController
    {
        protected ILevelController _levelController;

        public ActorInputController(IActorController actorController)
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
