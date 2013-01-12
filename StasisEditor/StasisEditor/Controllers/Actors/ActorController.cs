using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    abstract public class ActorController
    {
        protected ActorType _type;
        protected LevelController _levelController;

        public ActorType type { get { return _type; } }
        public bool shift { get { return _levelController.shift; } }
        public bool ctrl { get { return _levelController.ctrl; } }
        virtual public List<ActorProperties> properties { get { return new List<ActorProperties>(); } }

        public ActorController(LevelController levelController)
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
        abstract public bool hitTest(Vector2 worldMouse, bool select = true);

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
        abstract public ActorController clone();
    }
}
