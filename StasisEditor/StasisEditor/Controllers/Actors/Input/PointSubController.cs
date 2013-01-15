using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class PointSubController : ActorSubController
    {
        protected Vector2 _position;
        protected IPointSubControllable _actorController;
        protected float _marginInPixels;

        public Vector2 position { get { return _position; } }

        public PointSubController(Vector2 position, IPointSubControllable actorResourceController, float marginInPixels = 8f)
            : base()
        {
            _position = position;
            _actorController = actorResourceController;
            _marginInPixels = marginInPixels;
        }

        #region Input

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            float margin = _marginInPixels / _actorController.getLevelController().getScale();
            Vector2 relative = _position - worldMouse;
            float length = relative.Length();

            return length <= margin;
        }

        // lineHitTest
        public bool lineHitTest(Vector2 worldMouse, Vector2 lineEndPoint)
        {
            // Treat line as a thin box
            Vector2 boxCenter = (position + lineEndPoint) / 2;
            Vector2 linkRelative = lineEndPoint - position;
            float boxHalfWidth = linkRelative.Length() / 2;
            float boxHalfHeight = 0.15f;
            float angle = (float)Math.Atan2(linkRelative.Y, linkRelative.X);

            // Get the mouse position relative to the box's center
            Vector2 relative = boxCenter - worldMouse;

            // Rotate the relative mouse position by the negative angle of the box
            Vector2 alignedRelative = Vector2.Transform(relative, Matrix.CreateRotationZ(-angle));

            // Get the local, cartiasian-aligned bounding-box
            Vector2 topLeft = -(new Vector2(boxHalfWidth, boxHalfHeight));
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
        public override void handleMouseMove(Vector2 worldDelta)
        {
            _position = _position + worldDelta;
        }

        // handleLeftMouseDown
        public override void handleMouseDown()
        {
            _actorController.deselectSubController(this);
        }

        #endregion
    }
}
