using System;
using System.Collections.Generic;
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

        // globalCheckKey
        virtual public void globalCheckKey() { }

        // draw
        abstract public void draw();

        // clone
        abstract public ActorResourceController clone();
    }
}
