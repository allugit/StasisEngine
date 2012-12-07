using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using StasisCore.Models;
using StasisEditor.Views;
using StasisEditor.Controls;
using StasisEditor.Models;

namespace StasisEditor.Controllers
{
    public class EditorController : IEditorController
    {
        public const string TEXTURE_RESOURCE_DIRECTORY = "E:\\_C#\\StasisEngine\\StasisGame\\StasisGame\\bin\\x86\\Debug\\TextureResources";
        public const string TEMPORARY_TEXTURE_DIRECTORY = "E:\\_C#\\StasisEngine\\StasisEditor\\StasisEditor\\bin\\x86\\Debug\\Temporary";

        private XNAController _xnaController;
        private MaterialController _materialController;
        private TextureController _textureController;
        private LevelController _levelController;

        private IEditorView _editorView;

        private BindingList<TextureResource> _textureResources;

        private float _scale = 35f;

        public EditorController(XNAController xnaController)
        {
            _xnaController = xnaController;

            // Create editor view
            _editorView = new EditorView();
            _editorView.setController(this);

            // Load texture resources
            _textureResources = new BindingList<TextureResource>(TextureResource.loadAll(EditorController.TEXTURE_RESOURCE_DIRECTORY));

            // Initialize core texture controller
            StasisCore.Controllers.TextureController.textureDirectory = EditorController.TEXTURE_RESOURCE_DIRECTORY; // Use the absolute path, since the core uses a relative path by default.
            StasisCore.Controllers.TextureController.graphicsDevice = XNAResources.graphicsDevice;
            StasisCore.Controllers.TextureController.addResources(new List<TextureResource>(_textureResources));

            // Create material controller
            _materialController = new MaterialController(this, _editorView.getMaterialView());

            // Create texture controller
            _textureController = new TextureController(this, _editorView.getTextureView());

            // Create level controller
            _levelController = new LevelController(this, _editorView.getLevelView());
        }

        // getScale
        public float getScale() { return _scale; }

        // getTextureResources
        public BindingList<TextureResource> getTextureResources()
        {
            return _textureResources;
        }

        // addTextureResource
        public void addTextureResource(TextureResource resource)
        {
            _textureResources.Add(resource);
            StasisCore.Controllers.TextureController.addResource(resource);
        }

        // removeTextureResource
        public void removeTextureResource(TextureResource resource)
        {
            _textureResources.Remove(resource);
        }

        // resizeGraphicsDevice
        public void resizeGraphicsDevice(int width, int height)
        {
            _xnaController.resizeGraphicsDevice(width, height);
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
            _editorView.addActorToolbar(_levelController);
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
            _editorView.removeActorToolbar();
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            _levelController.handleXNADraw();
        }

        // exit
        public void exit()
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
