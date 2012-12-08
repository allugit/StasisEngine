﻿using System;
using System.Collections.Generic;
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
        abstract public void mouseMove();

        // mouseEnterView
        abstract public void mouseEnterView();

        // mouseLeaveView
        abstract public void mouseLeaveView();

        // keyDown
        abstract public void keyDown();

        // keyUp
        abstract public void keyUp();

        // selectAllSubControllers
        abstract public void selectAllSubControllers();

        // draw
        abstract public void draw();

        // clone
        abstract public ActorResourceController clone();
    }
}
