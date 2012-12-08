using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public class LinkedPointSubController : ActorSubController
    {
        private Vector2 _position;
        private ILinkedPointSubControllable _actorResourceController;
        private LinkedPointSubController _previous;
        private LinkedPointSubController _next;

        public Vector2 position { get { return _position; } }
        public LinkedPointSubController previous { get { return _previous; } set { _previous = value; } }
        public LinkedPointSubController next { get { return _next; } set { _next = value; } }

        public LinkedPointSubController(Vector2 position, ILinkedPointSubControllable actorResourceController)
            : base()
        {
            _position = position;
            _actorResourceController = actorResourceController;
        }

        #region Input

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            float margin = 8f / _actorResourceController.getLevelController().getScale();
            Vector2 relative = _position - worldMouse;
            float length = relative.Length();

            return length <= margin;
        }

        // handleMouseMove
        public override void handleMouseMove(Vector2 worldDelta)
        {
            _position = _position + worldDelta;
        }

        // handleMouseDown
        public override void handleMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                _actorResourceController.deselectSubController(this);
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
