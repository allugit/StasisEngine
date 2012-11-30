using System;
using System.Diagnostics;
using System.Collections.Generic;
using StasisEditor.Views;
using StasisEditor.Controls;
using StasisEditor.Models;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers
{
    public class EditorController
    {
        private XNAController _xnaController;
        private IEditorView _editorView;

        private Level _level;
        private ILevelView _levelView;
        private bool _isMouseOverView;
        private System.Drawing.Point _mouse;
        private float _scale = 35f;
        private Vector2 _screenCenter;

        public Level level { get { return _level; } }
        public bool isMouseOverView { get { return _isMouseOverView; } }
        public float scale { get { return _scale; } }
        public Vector2 worldOffset { get { return _screenCenter + (new Vector2(_levelView.getWidth(), _levelView.getHeight()) / 2) / scale; } }
        public Vector2 worldMouse { get { return new Vector2(_mouse.X, _mouse.Y) / scale - worldOffset; } }

        public EditorController(XNAController xnaController, IEditorView editorView)
        {
            _xnaController = xnaController;
            _editorView = editorView;
            _editorView.setController(this);
        }

        // resizeGraphicsDevice
        public void resizeGraphicsDevice(int width, int height)
        {
            _xnaController.resizeGraphicsDevice(width, height);
        }

        // createNewLevel
        public void createNewLevel()
        {
            Debug.Assert(_level == null);

            // Create model
            _level = new Level();

            // Create level view
            _levelView = new LevelView();
            _levelView.setController(this);
            
            _editorView.addLevelView(_levelView);
            _editorView.addLevelSettings(level);
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

                _screenCenter -= new Vector2(deltaX, deltaY) / scale;
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

        // handleXNADraw
        public void handleXNADraw()
        {
            if (_levelView != null)
                _levelView.handleXNADraw();
        }

        // exit
        public void exit()
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
