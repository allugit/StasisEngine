using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisEditor.Controllers.Actors
{
    public class BoxActorInputController : ActorInputController
    {
        private IBoxActorController _boxActorController;

        public BoxActorInputController(IBoxActorController boxActorController) 
            : base(boxActorController)
        {
            _boxActorController = boxActorController;
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
