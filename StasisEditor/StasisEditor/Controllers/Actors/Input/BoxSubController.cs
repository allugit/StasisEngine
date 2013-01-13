using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public enum BoxSubControllerAlignment
    {
        Center = 0,
        Edge
    };

    public class BoxSubController : ActorSubController
    {
        private IBoxSubControllable _actorResourceController;
        private float _sizeChangeAmount = 0.1f;
        private float _rotationChangeAmount = 0.05f;
        private BoxSubControllerAlignment _alignment;
        private Vector2 _alignmentOffset = Vector2.Zero;

        public Vector2 alignmentOffset { get { return _alignmentOffset; } }

        public BoxSubController(IBoxSubControllable actorResourceController, BoxSubControllerAlignment alignment = BoxSubControllerAlignment.Center)
            : base()
        {
            _actorResourceController = actorResourceController;
            _alignment = alignment;
            calculateAlignmentOffset();
        }

        // Calculate alignment offset
        private void calculateAlignmentOffset()
        {
            _alignmentOffset = Vector2.Zero;
            if (_alignment == BoxSubControllerAlignment.Edge)
            {
                float halfHeight = _actorResourceController.getHalfHeight();
                float angle = _actorResourceController.getAngle();
                float halfPi = (float)(Math.PI / 2);
                _alignmentOffset = new Vector2((float)Math.Cos(angle - halfPi), (float)Math.Sin(angle - halfPi)) * halfHeight;
            }
        }

        #region Input

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            // Get the mouse position relative to the box's center
            Vector2 relative = (_actorResourceController.getPosition() + _alignmentOffset) - worldMouse;

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
        public override void handleMouseMove(Vector2 worldDelta)
        {
            //bool ctrl = Input.newKey.IsKeyDown(Keys.LeftControl) || Input.newKey.IsKeyDown(Keys.RightControl);

            //if (!ctrl)
                _actorResourceController.setPosition(_actorResourceController.getPosition() + worldDelta);
        }

        // handleMouseDown
        public override void handleMouseDown()
        {
            _actorResourceController.deselectSubController(this);
        }

        // keyDown
        public override void keyDown(Keys key)
        {
            // Update width
            if (key == Keys.A)
                _actorResourceController.setHalfWidth(_actorResourceController.getHalfWidth() + _sizeChangeAmount);
            if (key == Keys.D)
                _actorResourceController.setHalfWidth(_actorResourceController.getHalfWidth() - _sizeChangeAmount);

            // Update height
            if (key == Keys.W)
                _actorResourceController.setHalfHeight(_actorResourceController.getHalfHeight() + _sizeChangeAmount);
            if (key == Keys.S)
                _actorResourceController.setHalfHeight(_actorResourceController.getHalfHeight() - _sizeChangeAmount);

            // Update angle
            if (key == Keys.E)
                _actorResourceController.setAngle(_actorResourceController.getAngle() + _rotationChangeAmount);
            if (key == Keys.Q)
                _actorResourceController.setAngle(_actorResourceController.getAngle() - _rotationChangeAmount);

            // Calculate alignment offset
            calculateAlignmentOffset();
        }

        #endregion

    }
}
