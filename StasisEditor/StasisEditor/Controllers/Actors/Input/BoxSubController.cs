using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public class BoxSubController : ActorSubController
    {
        private IBoxSubControllable _actorResourceController;

        public BoxSubController(IBoxSubControllable actorResourceController)
            : base()
        {
            _actorResourceController = actorResourceController;
        }

        #region Input

        // handleMouseMove
        public override void handleMouseMove(Vector2 worldMouse)
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

        // handleMouseDown
        public override void handleMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
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
