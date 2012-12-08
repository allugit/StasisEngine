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

        // mouseMove
        abstract public void mouseMove(Vector2 worldMouse);

        // mouseEnterView
        abstract public void mouseEnterView();

        // mouseLeaveView
        abstract public void mouseLeaveView();

        // mouseDown
        abstract public void mouseDown(System.Windows.Forms.MouseEventArgs e);

        // mouseUp
        abstract public void mouseUp(System.Windows.Forms.MouseEventArgs e);

        // keyDown
        abstract public void keyDown();

        // keyUp
        abstract public void keyUp();

        // selectAllSubControllers
        abstract public void selectAllSubControllers();

        // deselectAllSubControllers
        abstract public void deselectAllSubControllers();

        // draw
        abstract public void draw();

        // clone
        abstract public ActorResourceController clone();
    }
}
