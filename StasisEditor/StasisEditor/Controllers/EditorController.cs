using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using StasisCore.Models;
using StasisEditor.Views;
using StasisEditor.Controls;
using StasisEditor.Models;

namespace StasisEditor.Controllers
{
    public class EditorController
    {
        private XNAController _xnaController;
        private MaterialController _materialController;
        private IEditorView _editorView;

        private Level _level;
        private ILevelView _levelView;
        public Level level { get { return _level; } }
        //private IMaterialView _materialView;
        //private List<Material>[] _materials;

        private bool _isMouseOverView;
        private System.Drawing.Point _mouse;
        private float _scale = 35f;
        private Vector2 _screenCenter;

        public bool isMouseOverView { get { return _isMouseOverView; } }
        public float scale { get { return _scale; } }
        public Vector2 worldOffset { get { return _screenCenter + (new Vector2(_levelView.getWidth(), _levelView.getHeight()) / 2) / scale; } }
        public Vector2 worldMouse { get { return new Vector2(_mouse.X, _mouse.Y) / scale - worldOffset; } }

        public EditorController(XNAController xnaController)
        {
            _xnaController = xnaController;

            // Create editor view
            _editorView = new EditorView();
            _editorView.setController(this);

            // Create material controller
            _materialController = new MaterialController(this);
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
            
            // Add level view to editor view
            _editorView.addLevelView(_levelView);
            _editorView.addLevelSettings(level);

            // Create brush toolbar
            _editorView.addBrushToolbar();

            // Modify menu items
            _editorView.enableNewLevel(false);
            _editorView.enableCloseLevel(true);
            _editorView.enableLoadLevel(false);
            _editorView.enableSaveLevel(true);
        }

        // closeLevel
        public void closeLevel()
        {
            // Remove level views
            _editorView.removeLevelSettings();
            _editorView.removeLevelView();

            // Remove toolbar
            _editorView.removeBrushToolbar();

            // Modify menu
            _editorView.enableNewLevel(true);
            _editorView.enableCloseLevel(false);
            _editorView.enableLoadLevel(true);
            _editorView.enableSaveLevel(false);

            // Remove model
            _level = null;
        }

        // openMaterialView
        public void openMaterialView()
        {
            _materialController.openView();
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
