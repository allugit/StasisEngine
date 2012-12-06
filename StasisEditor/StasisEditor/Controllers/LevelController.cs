using System;
using System.Collections.Generic;
using StasisEditor.Views;
using StasisCore.Models;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers
{
    public class LevelController : ILevelController
    {
        private IEditorController _editorController;
        private ILevelView _levelView;

        private Level _level;

        private bool _isMouseOverView;
        private System.Drawing.Point _mouse;
        private Vector2 _screenCenter;

        public LevelController(IEditorController editorController, ILevelView levelView)
        {
            _editorController = editorController;
            _levelView = levelView;
            _levelView.setController(this);
        }

        public float getScale() { return _editorController.getScale(); }
        public bool getIsMouseOverView() { return _isMouseOverView; }
        public Vector2 getWorldOffset() { return _screenCenter + (new Vector2(_levelView.getWidth(), _levelView.getHeight()) / 2) / _editorController.getScale(); }
        public Vector2 getWorldMouse() { return new Vector2(_mouse.X, _mouse.Y) / _editorController.getScale() - getWorldOffset(); }

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

        // createNewLevel
        public void createNewLevel()
        {
            _level = new Level();
        }

        // closeLevel
        public void closeLevel()
        {
            _level = null;
        }

        // mouseMove
        public void mouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            int x = Math.Min(Math.Max(0, e.X), _levelView.getWidth());
            int y = Math.Min(Math.Max(0, e.Y), _levelView.getHeight());

            bool ctrl = Input.newKey.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl);

            if (ctrl)
            {
                int deltaX = _mouse.X - x;
                int deltaY = _mouse.Y - y;

                _screenCenter -= new Vector2(deltaX, deltaY) / _editorController.getScale();
            }

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
    }
}
