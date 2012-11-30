using System;
using System.Diagnostics;
using System.Collections.Generic;
using StasisEditor.View;

namespace StasisEditor.Controller
{
    public class EditorController
    {
        private XNAController _xnaController;
        private IEditorView _view;
        private LevelController _levelController;
        public System.Drawing.Point levelContainerSize { get { return _view.getLevelContainerSize(); } }

        public EditorController(XNAController xnaController, IEditorView view)
        {
            _xnaController = xnaController;
            _view = view;
            _view.setController(this);
        }

        // resizeGraphicsDevice
        public void resizeGraphicsDevice(int width, int height)
        {
            _xnaController.resizeGraphicsDevice(width, height);
        }

        // createNewLevel
        public void createNewLevel()
        {
            // There should only be one level controller at a time
            Debug.Assert(_levelController == null);

            // Create view and controller
            LevelView levelView = new LevelView();
            _levelController = new LevelController(this, levelView);

            // Add level view to editor view
            _view.addLevelView(levelView);
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            if (_levelController != null)
                _levelController.handleXNADraw();
        }

        // exit
        public void exit()
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
