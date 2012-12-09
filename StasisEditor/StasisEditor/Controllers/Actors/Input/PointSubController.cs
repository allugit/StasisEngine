using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StasisEditor.Controllers.Actors
{
    public class PointSubController : ActorSubController
    {
        private Vector2 _position;
        private IPointSubControllable _actorResourceController;
        private float _marginInPixels;

        public Vector2 position { get { return _position; } }

        public PointSubController(Vector2 position, IPointSubControllable actorResourceController, float marginInPixels = 8f)
            : base()
        {
            _position = position;
            _actorResourceController = actorResourceController;
            _marginInPixels = marginInPixels;
        }

        #region Input

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            float margin = _marginInPixels / _actorResourceController.getLevelController().getScale();
            Vector2 relative = _position - worldMouse;
            float length = relative.Length();

            return length <= margin;
        }

        // handleMouseMove
        public override void handleMouseMove(Vector2 worldDelta)
        {
            bool ctrl = Input.newKey.IsKeyDown(Keys.LeftControl) || Input.newKey.IsKeyDown(Keys.RightControl);

            if (!ctrl)
                _position = _position + worldDelta;
        }

        // handleMouseDown
        public override void handleMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                _actorResourceController.deselectSubController(this);
        }

        #endregion
    }
}
