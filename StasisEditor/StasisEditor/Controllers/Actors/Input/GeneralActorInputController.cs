using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
