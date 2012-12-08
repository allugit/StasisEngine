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

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            // Get the mouse position relative to the box's center
            Vector2 relative = _actorResourceController.getPosition() - worldMouse;

            // Rotate the relative mouse position by the negative angle of the box
            Vector2 alignedRelative = Vector2.Transform(relative, Matrix.CreateRotationZ(-_actorResourceController.getAngle()));

            // Get the local, cartiasian-aligned bounding-box
            Vector2 topLeft = -(new Vector2(_actorResourceController.getHalfWidth(), _actorResourceController.getHalfHeight()));
            Vector2 bottomRight = -topLeft;

            // Check if the relative mouse point is inside the bounding box
            Vector2 d1, d2;
            d1 = alignedRelative - topLeft;
            d2 = bottomRight - alignedRelative;

            // One of these components will be less than zero if the alignedRelative position is outside of the bounds
            if (d1.X < 0 || d1.Y < 0)
                return false;

            if (d2.X < 0 || d2.Y < 0)
                return false;

            return true;
        }

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
