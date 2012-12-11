using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

        #region Point Insertion/Removal

        // insertPoint -- insert a point between this link and the next
        public void insertPoint(Vector2 point)
        {
            // Create new linked point
            LinkedPointSubController newPoint = new LinkedPointSubController(point, _actorResourceController);

            // Reorganize connections
            newPoint.previous = this;
            newPoint.next = next;
            next.previous = newPoint;
            next = newPoint;
        }

        // removePoint -- remove this point
        public void removePoint()
        {
            // Handle deletion when there are no other linked points
            if (next == null && previous == null)
            {
                if (selected)
                    _actorResourceController.deselectSubController(this);
                _actorResourceController.delete();
            }

            if (previous != null)
                previous.next = next;

            if (next != null)
                next.previous = previous;

            // Find head node
            LinkedPointSubController head = this;
            while (head.previous != null)
                head = head.previous;

            // Update actor resource's head control if necessary
            if (head == this)
                _actorResourceController.setNewLinkedPointSubControllerHead(next);
        }

        #endregion

        #region Input

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            float margin = 8f / _actorResourceController.getLevelController().getScale();
            Vector2 relative = _position - worldMouse;
            float length = relative.Length();

            return length <= margin;
        }

        // linkHitTest -- Create a box between this link and the next, and hit test it
        public bool linkHitTest(Vector2 worldMouse)
        {
            // Treat line as a thin box
            Vector2 boxCenter = (position + next.position) / 2;
            Vector2 linkRelative = next.position - position;
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
            bool ctrl = Input.newKey.IsKeyDown(Keys.LeftControl) || Input.newKey.IsKeyDown(Keys.RightControl);

            if (!ctrl)
                _position = _position + worldDelta;
        }

        // handleLeftMouseDown
        public override void handleLeftMouseDown()
        {
            bool shift = Input.newKey.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);

            // Create new linked points by pressing shift (disabled when both next and previous links exist)
            if (shift && (_next == null || _previous == null))
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

            _actorResourceController.deselectSubController(this);
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
