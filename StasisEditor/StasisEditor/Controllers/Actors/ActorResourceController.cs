using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;
using StasisEditor.Views;

namespace StasisEditor.Controllers.Actors
{
    abstract public class ActorResourceController
    {
        protected ActorResource _actor;
        protected ShapeRenderer _renderer;
        protected ILevelController _levelController;

        public ActorType type { get { return _actor.type; } }

        public ActorResourceController(ILevelController levelController)
        {
            _levelController = levelController;
            _renderer = levelController.getShapeRenderer();
        }

        // getLevelController
        public ILevelController getLevelController()
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

        // globalCheckKey
        virtual public void globalCheckKey() { }

        // draw
        abstract public void draw();

        // clone
        abstract public ActorResourceController clone();
    }
}
