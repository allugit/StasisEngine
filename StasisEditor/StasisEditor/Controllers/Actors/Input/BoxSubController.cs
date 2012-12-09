using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StasisEditor.Controllers.Actors
{
    public class BoxSubController : ActorSubController
    {
        private IBoxSubControllable _actorResourceController;
        private float _sizeChangeAmount = 0.1f;
        private float _rotationChangeAmount = 0.05f;

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
        public override void handleMouseMove(Vector2 worldDelta)
        {
            _actorResourceController.setPosition(_actorResourceController.getPosition() + worldDelta);
        }

        // handleMouseDown
        public override void handleMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                _actorResourceController.deselectSubController(this);
        }

        // checkXNAKeys
        public override void checkXNAKeys()
        {
            // Update width
            if (Input.newKey.IsKeyDown(Keys.A))
                _actorResourceController.setHalfWidth(_actorResourceController.getHalfWidth() + _sizeChangeAmount);
            if (Input.newKey.IsKeyDown(Keys.D))
                _actorResourceController.setHalfWidth(_actorResourceController.getHalfWidth() - _sizeChangeAmount);

            // Update height
            if (Input.newKey.IsKeyDown(Keys.W))
                _actorResourceController.setHalfHeight(_actorResourceController.getHalfHeight() + _sizeChangeAmount);
            if (Input.newKey.IsKeyDown(Keys.S))
                _actorResourceController.setHalfHeight(_actorResourceController.getHalfHeight() - _sizeChangeAmount);

            // Update angle
            if (Input.newKey.IsKeyDown(Keys.E))
                _actorResourceController.setAngle(_actorResourceController.getAngle() + _rotationChangeAmount);
            if (Input.newKey.IsKeyDown(Keys.Q))
                _actorResourceController.setAngle(_actorResourceController.getAngle() - _rotationChangeAmount);
        }

        #endregion

    }
}
