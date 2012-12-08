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
            bool shift = Input.newKey.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);

            // Create new linked points by pressing shift (disabled when both next and previous links exist)
            if (shift && e.Button == System.Windows.Forms.MouseButtons.Left &&
                (_next == null || _previous == null))
            {
                // Deselect current controller
                _actorResourceController.deselectSubController(this);

                // Attach cloned controller
                LinkedPointSubController cloned = null;
                if (_next == null)
                    cloned = cloneAsNext();
                else
                    cloned = cloneAsPrevious();

                // Select cloned controller
                _actorResourceController.selectSubController(cloned);

                return;
            }
            
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

        #region Clone methods

        // cloneAsNext
        private LinkedPointSubController cloneAsNext()
        {
            LinkedPointSubController copy = clone();
            _next = copy;
            copy.previous = this;
            return copy;
        }

        // cloneAsPrevious
        private LinkedPointSubController cloneAsPrevious()
        {
            // Create clone and set it up as the previous link
            LinkedPointSubController copy = clone();
            _previous = copy;
            copy.next = this;

            // Modify actor resource controller's head (only necessary when adding a previous link)
            _actorResourceController.setNewLinkedPointSubControllerHead(copy);

            return copy;
        }

        // clone
        private LinkedPointSubController clone()
        {
            return new LinkedPointSubController(position, _actorResourceController);
        }

        #endregion
    }
}
