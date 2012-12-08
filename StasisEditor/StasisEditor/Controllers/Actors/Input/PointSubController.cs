using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public class PointSubController : ActorSubController
    {
        private Vector2 _position;
        private IPointSubControllable _actorResourceController;

        public Vector2 position { get { return _position; } }

        public PointSubController(Vector2 position, IPointSubControllable actorResourceController)
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
        public override void handleMouseMove(Vector2 worldMouse)
        {
            _position = worldMouse;
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
