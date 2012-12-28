using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisCore.Resources;
using StasisEditor.Views;

namespace StasisEditor.Controllers.Actors
{
    abstract public class ActorResourceController
    {
        protected ActorResource _actor;
        protected LevelController _levelController;
        protected bool _shift;
        protected bool _ctrl;

        public ActorType type { get { return _actor.type; } }
        public bool shift { get { return _shift; } }
        public bool ctrl { get { return _ctrl; } }

        public ActorResourceController(LevelController levelController)
        {
            _levelController = levelController;
        }

        // getLevelController
        public LevelController getLevelController()
        {
            return _levelController;
        }

        // selectAllSubControllers
        abstract public void selectAllSubControllers();

        // deselectAllSubControllers
        abstract public void deselectAllSubControllers();

        // selectSubController
        public void selectSubController(ActorSubController subController)
        {
            _levelController.selectSubController(subController);
        }

        // deselectSubController
        public void deselectSubController(ActorSubController subController)
        {
            _levelController.deselectSubController(subController);
        }

        // hitTest
        abstract public bool hitTest(Vector2 worldMouse);

        // delete
        virtual public void delete()
        {
            deselectAllSubControllers();
            _levelController.removeActorController(this);
        }

        // globalKeyDown
        virtual public void globalKeyDown(Keys key) 
        {
            if (key == Keys.Shift || key == Keys.ShiftKey || key == Keys.LShiftKey || key == Keys.RShiftKey)
                _shift = true;
            else if (key == Keys.Control || key == Keys.ControlKey || key == Keys.LControlKey || key == Keys.RControlKey)
                _ctrl = true;
        }

        // globalKeyUp
        virtual public void globalKeyUp(Keys key)
        {
            if (key == Keys.Shift || key == Keys.ShiftKey || key == Keys.LShiftKey || key == Keys.RShiftKey)
                _shift = false;
            else if (key == Keys.Control || key == Keys.ControlKey || key == Keys.LControlKey || key == Keys.RControlKey)
                _ctrl = false;
        }

        // draw
        abstract public void draw();

        // clone
        abstract public ActorResourceController clone();
    }
}
