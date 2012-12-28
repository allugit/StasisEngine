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

        public ActorType type { get { return _actor.type; } }
        public bool shift { get { return _levelController.shift; } }
        public bool ctrl { get { return _levelController.ctrl; } }

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
        virtual public void globalKeyDown(Keys key) { }

        // globalKeyUp
        //virtual public void globalKeyUp(Keys key) { }

        // draw
        abstract public void draw();

        // clone
        abstract public ActorResourceController clone();
    }
}
