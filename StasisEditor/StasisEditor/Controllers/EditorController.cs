using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using StasisCore.Models;
using StasisEditor.Views;
using StasisEditor.Views.Controls;

namespace StasisEditor.Controllers
{
    public class EditorController : Controller
    {
        public const string TEXTURE_RESOURCE_DIRECTORY = "E:\\_C#\\StasisEngine\\StasisGame\\StasisGame\\bin\\x86\\Debug\\TextureResources";
        public const string TEMPORARY_TEXTURE_DIRECTORY = "E:\\_C#\\StasisEngine\\StasisEditor\\StasisEditor\\bin\\x86\\Debug\\Temporary";

        private XNAController _xnaController;
        private MaterialController _materialController;
        private TextureController _textureController;
        private LevelController _levelController;
        private ItemController _itemController;

        private EditorView _editorView;
        private ShapeRenderer _shapeRenderer;
        private ActorToolbar _actorToolbar;

        private float _scale = 35f;

        public EditorController(XNAController xnaController)
        {
            _xnaController = xnaController;

            // Create editor view
            _editorView = new EditorView();
            _editorView.setController(this);

            // Create material controller
            _materialController = new MaterialController(this, _editorView.getMaterialView());

            // Create texture controller
            _textureController = new TextureController(this, _editorView.getTextureView());

            // Create item controller
            _itemController = new ItemController(this, _editorView.getItemView());

            // Create level controller
            _levelController = new LevelController(this, _editorView.getLevelView());

            // Create shape renderer
            _shapeRenderer = new ShapeRenderer(_levelController);
            _levelController.setShapeRenderer(_shapeRenderer);
        }

        // getScale
        public float getScale() { return _scale; }

        // setScale
        public void setScale(float value) { _scale = value; }

        // loadResources
        protected override void loadResources()
        {
        }

        // resizeGraphicsDevice
        public void resizeGraphicsDevice(int width, int height)
        {
            _xnaController.resizeGraphicsDevice(width, height);
        }

        // setActorToolbarEnabled
        public void setActorToolbarEnabled(bool status)
        {
            _actorToolbar.Enabled = status;
        }

        // unhookXNAFromLevel
        public void unhookXNAFromLevel()
        {
            _levelController.unhookXNAFromView();
        }

        // hookXNAToLevel
        public void hookXNAToLevel()
        {
            _levelController.hookXNAToView();
        }

        // enableLevelInput
        public void enableLevelInput(bool status)
        {
            _levelController.enableInput(status);
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
            _editorView.addLevelSettings(_levelController.getLevel());

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

        // handleXNADraw
        public void handleXNADraw()
        {
            // Level controller draw
            _levelController.handleXNADraw();

            // Item controller draw
            _itemController.handleXNADraw();
        }

        // update
        public void update()
        {
            // Handle mouse wheel event through XNA
            if (Input.deltaScrollValue != 0)
                _scale += (float)Input.deltaScrollValue * 0.01f;
            
            // Level controller update
            _levelController.update();

            // Item controller update
            _itemController.update();
        }

        // exit
        public void exit()
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
