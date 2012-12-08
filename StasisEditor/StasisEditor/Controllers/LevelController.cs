﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers.Actors;
using StasisEditor.Views;
using StasisCore.Models;

namespace StasisEditor.Controllers
{
    public class LevelController : ILevelController
    {
        private IEditorController _editorController;
        private ILevelView _levelView;
        private ShapeRenderer _shapeRenderer;

        private ISubControllable _selectedSubControllers;
        private List<ActorResourceController> _actorControllers;

        private LevelResource _level;

        private bool _isMouseOverView;
        private System.Drawing.Point _mouse;
        private Vector2 _screenCenter;

        public LevelController(IEditorController editorController, ILevelView levelView)
        {
            _editorController = editorController;
            _levelView = levelView;
            _levelView.setController(this);

            _actorControllers = new List<ActorResourceController>();
        }

        #region Getters/Setters

        public float getScale() { return _editorController.getScale(); }
        public bool getIsMouseOverView() { return _isMouseOverView; }
        public Vector2 getWorldOffset() { return _screenCenter + (new Vector2(_levelView.getWidth(), _levelView.getHeight()) / 2) / _editorController.getScale(); }
        public Vector2 getWorldMouse() { return new Vector2(_mouse.X, _mouse.Y) / _editorController.getScale() - getWorldOffset(); }

        // getLevel
        public LevelResource getLevel()
        {
            return _level;
        }

        // getActorControllers
        public List<ActorResourceController> getActorControllers()
        {
            return _actorControllers;
        }

        // setShapeRenderer
        public void setShapeRenderer(ShapeRenderer shapeRenderer)
        {
            _shapeRenderer = shapeRenderer;
        }

        // getShapeRenderer
        public ShapeRenderer getShapeRenderer()
        {
            return _shapeRenderer;
        }

        #endregion

        #region XNA Methods

        // resizeGraphicsDevice
        public void resizeGraphicsDevice(int width, int height)
        {
            _editorController.resizeGraphicsDevice(width, height);
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            if (_level != null)
                _levelView.handleXNADraw();
        }

        #endregion

        #region Levels

        // createNewLevel
        public void createNewLevel()
        {
            _level = new LevelResource();
        }

        // closeLevel
        public void closeLevel()
        {
            _level = null;
        }

        #endregion

        #region Actor Controllers

        // createActorControllerFromToolbar
        public void createActorControllerFromToolbar(string buttonName)
        {
            // Create actor controller based on button name
            ActorResourceController actorController = null;
            switch (buttonName)
            {
                case "boxButton":
                    actorController = new BoxActorResourceController(this);
                    break;
            }

            if (actorController != null)
            {
                // Add actor controller to list
                _actorControllers.Add(actorController);
            }
        }

        // addActorController
        public void addActorController(ActorResourceController actorController)
        {
            _actorControllers.Add(actorController);
        }

        // removeActorController
        public void removeActorController(ActorResourceController actorController)
        {
            _actorControllers.Remove(actorController);
        }

        // selectActorController
        public void selectActorController(ActorResourceController actorController)
        {
            _editorController.setActorToolbarEnabled(false);
        }

        // deselectActorController
        public void deselectActorController()
        {
            _editorController.setActorToolbarEnabled(true);
        }

        #endregion

        #region Input

        // mouseMove
        public void mouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            // Set mouse boundaries
            int x = Math.Min(Math.Max(0, e.X), _levelView.getWidth());
            int y = Math.Min(Math.Max(0, e.Y), _levelView.getHeight());

            // Calculate change in mouse position (for screen and world coordinates)
            int deltaX = _mouse.X - x;
            int deltaY = _mouse.Y - y;
            Vector2 worldDelta = new Vector2(deltaX, deltaY) / _editorController.getScale();

            // Move screen
            if (Input.newKey.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                _screenCenter -= worldDelta;

            // Store screen space mouse coordinates
            _mouse.X = x;
            _mouse.Y = y;
        }

        // mouseEnter
        public void mouseEnter()
        {
            _isMouseOverView = true;
        }

        // mouseLeave
        public void mouseLeave()
        {
            _isMouseOverView = false;
        }

        #endregion
    }
}
