using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StasisEditor.Controllers.Actors
{
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
            if (Input.newKey.IsKeyDown(Keys.A) || Input.newKey.IsKeyDown(Keys.W))
                _actorResourceController.setRadius(_actorResourceController.getRadius() + _radiusChangeAmount);
            if (Input.newKey.IsKeyDown(Keys.D) || Input.newKey.IsKeyDown(Keys.S))
                _actorResourceController.setRadius(_actorResourceController.getRadius() - _radiusChangeAmount);
        }
    }
}
