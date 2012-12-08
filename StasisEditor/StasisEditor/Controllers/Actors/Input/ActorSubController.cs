using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StasisEditor.Controllers.Actors
{
    abstract public class ActorSubController
    {
        public ActorSubController()
        {
        }

        // handleMouseMove
        abstract public void handleMouseMove(Microsoft.Xna.Framework.Vector2 worldMouse);

        // handleMouseEnterView
        abstract public void handleMouseEnterView();

        // handleMouseLeaveView
        abstract public void handleMouseLeaveView();

        // handleMouseDown
        abstract public void handleMouseDown(MouseEventArgs e);

        // handleMouseUp
        abstract public void handleMouseUp(MouseEventArgs e);

        // handleKeyDown
        abstract public void handleKeyDown();

        // handleKeyUp
        abstract public void handleKeyUp();
    }
}
