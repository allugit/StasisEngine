using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public class GeneralSubController : ActorSubController
    {
        private IGeneralSubControllable _actorResourceController;

        public GeneralSubController(IGeneralSubControllable actorResourceController)
            : base()
        {
            _actorResourceController = actorResourceController;
        }

        #region Input

        // handleMouseMove
        public override void handleMouseMove(Vector2 worldMouse)
        {
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

        // handleMouseDown
        public override void handleMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            Console.WriteLine("shit yall");
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                _actorResourceController.deselectAllSubControllers();
        }

        // handleMouseUp
        public override void handleMouseUp(System.Windows.Forms.MouseEventArgs e)
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
