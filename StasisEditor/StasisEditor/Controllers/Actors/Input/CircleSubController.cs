using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class CircleSubController : ActorSubController
    {
        private ICircleSubControllable _actorResourceController;
        private float _radiusChangeAmount = 0.1f;

        public CircleSubController(ICircleSubControllable actorResourceController)
            : base()
        {
            _actorResourceController = actorResourceController;
        }

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            Vector2 relative = _actorResourceController.getPosition() - worldMouse;
            float length = relative.Length();

            return length <= _actorResourceController.getRadius();
        }

        // handleMouseMove
        public override void handleMouseMove(Vector2 worldDelta)
        {
            //bool ctrl = Input.newKey.IsKeyDown(Keys.LeftControl) || Input.newKey.IsKeyDown(Keys.RightControl);

            //if (!ctrl)
                _actorResourceController.setPosition(_actorResourceController.getPosition() + worldDelta);
        }

        // handleMouseDown
        public override void handleLeftMouseDown()
        {
            _actorResourceController.deselectSubController(this);
        }

        // keyDown
        public override void keyDown(Keys key)
        {
            // Update width
            if (key == Keys.A || key == Keys.W)
                _actorResourceController.setRadius(_actorResourceController.getRadius() + _radiusChangeAmount);
            if (key == Keys.D || key == Keys.S)
                _actorResourceController.setRadius(_actorResourceController.getRadius() - _radiusChangeAmount);
        }
    }
}
