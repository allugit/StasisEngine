using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StasisEditor.Controllers.Actors
{
    abstract public class ActorSubController
    {
        private bool _selected;
        public bool selected { get { return _selected; } set { _selected = value; } }

        public ActorSubController()
        {
        }

        // hitTest
        abstract public bool hitTest(Microsoft.Xna.Framework.Vector2 worldMouse);

        // handleMouseMove
        virtual public void handleMouseMove(Microsoft.Xna.Framework.Vector2 worldDelta)
        {
        }

        // handleMouseEnterView
        virtual public void handleMouseEnterView()
        {
        }

        // handleMouseLeaveView
        virtual public void handleMouseLeaveView()
        {
        }

        // handleMouseDown
        virtual public void handleMouseDown(MouseEventArgs e)
        {
        }

        // handleMouseUp
        virtual public void handleMouseUp(MouseEventArgs e)
        {
        }

        // checkXNAKeys
        virtual public void checkXNAKeys()
        {
        }
    }
}
