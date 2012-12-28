using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using StasisCore.Controllers;
using StasisCore.Resources;
using StasisEditor.Views;
using StasisEditor.Views.Controls;

namespace StasisEditor.Controllers
{
    public class EditorController : Controller
    {
        private MaterialController _materialController;
        private LevelController _levelController;
        private BlueprintController _blueprintController;
        private GraphicsDeviceService _graphicsDeviceService;

        private EditorView _editorView;
        private ActorToolbar _actorToolbar;

        private float _scale = 35f;

        public EditorView view { get { return _editorView; } }

        public EditorController(EditorView view)
        {
            // Initialize graphics device service
            _graphicsDeviceService = GraphicsDeviceService.AddRef(view.Handle, view.Width, view.Height);

            // Initialize core resource controller
            ResourceController.initialize(_graphicsDeviceService.GraphicsDevice);

            // Initialize view
            _editorView = view;
            view.setController(this);

            // Create material controller
            _materialController = new MaterialController(this, _editorView.getMaterialView());

            // Create level controller
            _levelController = new LevelController(this, _editorView.getLevelView());

            // Create blueprint controller
            _blueprintController = new BlueprintController(this, _editorView.getBlueprintView());

        }

        // getScale
        public float getScale() { return _scale; }

        // setScale
        public void setScale(float value) { _scale = value; }

        // setActorToolbarEnabled
        public void setActorToolbarEnabled(bool status)
        {
            _actorToolbar.Enabled = status;
        }

        // createNewLevel
        public void createNewLevel()
        {
            _levelController.createNewLevel();

            // Modify menu items
            _editorView.enableNewLevel(false);
            _editorView.enableCloseLevel(true);
            _editorView.enableLoadLevel(false);
            _editorView.enableSaveLevel(true);

            // Add level settings
            _editorView.addLevelSettings(_levelController.level);

            // Create actor toolbar
            _actorToolbar = new ActorToolbar();
            _actorToolbar.setController(_levelController);
            _editorView.addActorToolbar(_actorToolbar);
        }

        // closeLevel
        public void closeLevel()
        {
            _levelController.closeLevel();

            // Modify menu
            _editorView.enableNewLevel(true);
            _editorView.enableCloseLevel(false);
            _editorView.enableLoadLevel(true);
            _editorView.enableSaveLevel(false);

            // Remove level settings
            _editorView.removeLevelSettings();

            // Remove actor toolbar
            _editorView.removeActorToolbar(_actorToolbar);
        }

        // exit
        public void exit()
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
