using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using StasisEditor.Views;
using StasisEditor.Views.Controls;
using StasisEditor.Models;
using StasisCore;

namespace StasisEditor.Controllers
{
    public class EditorController : Controller
    {
        public const string GAME_PATH = @"D:\_C#\StasisEngine\StasisGame\StasisGame\bin\x86\Debug";
        public const float ORIGINAL_SCALE = 35f;
        private MaterialController _materialController;
        private LevelController _levelController;
        private BlueprintController _blueprintController;
        private CircuitController _circuitController;
        private BackgroundController _backgroundController;
        private WorldMapController _worldMapController;
        private GraphicsDeviceService _graphicsDeviceService;

        private EditorView _editorView;
        private ActorToolbar _actorToolbar;

        private float _scale = ORIGINAL_SCALE;

        public EditorView view { get { return _editorView; } }
        public MaterialController materialController { get { return _materialController; } }
        public LevelController levelController { get { return _levelController; } }
        public BlueprintController blueprintController { get { return _blueprintController; } }
        public CircuitController circuitController { get { return _circuitController; } }
        public BackgroundController backgroundController { get { return _backgroundController; } }
        public WorldMapController worldMapController { get { return _worldMapController; } }
        public float scale { get { return _scale; } set { _scale = value; } }

        public EditorController(EditorView view)
        {
            // Initialize graphics device service
            _graphicsDeviceService = GraphicsDeviceService.AddRef(view.Handle, view.Width, view.Height);

            // Initialize core resource controller
            ResourceManager.initialize(_graphicsDeviceService.GraphicsDevice);
            ResourceManager.rootDirectory = EditorResourceManager.RESOURCE_SOURCE_PATH + "\\";

            // Initialize view
            _editorView = view;
            view.setController(this);

            // Create controllers
            _materialController = new MaterialController(this, _editorView.materialView);
            _levelController = new LevelController(this, _editorView.levelView);
            _blueprintController = new BlueprintController(this, _editorView.blueprintView);
            _circuitController = new CircuitController(this, _editorView.circuitsView);
            _backgroundController = new BackgroundController(this, _editorView.backgroundView);
            _worldMapController = new WorldMapController(this, _editorView.worldMapView);
        }

        // setActorToolbarEnabled
        public void setActorToolbarEnabled(bool status)
        {
            _actorToolbar.Enabled = status;
        }

        // openActorProperties
        public void openActorProperties(IActorComponent component, bool closeOpenedProperties = true)
        {
            _editorView.openActorProperties(component, closeOpenedProperties);
        }

        // closeActorProperties
        public void closeActorProperties()
        {
            _editorView.closeActorProperties();
        }

        // refreshActorProperties
        public void refreshActorProperties()
        {
            _editorView.refreshActorProperties();
        }

        // createNewLevel
        public void createNewLevel()
        {
            _levelController.createNewLevel();
            _editorView.enableNewLevel(false);
            _editorView.enableCloseLevel(true);
            _editorView.enableLoadLevel(false);
            _editorView.enableSaveLevel(true);
            _editorView.enablePreviewLevel(true);
            _editorView.addLevelSettings(_levelController.level);
            _actorToolbar = new ActorToolbar();
            _actorToolbar.setController(_levelController);
            _editorView.addActorToolbar(_actorToolbar);
        }

        // loadLevel
        public void loadLevel(string filePath)
        {
            _levelController.loadLevel(filePath);
            _editorView.enableNewLevel(false);
            _editorView.enableCloseLevel(true);
            _editorView.enableLoadLevel(false);
            _editorView.enableSaveLevel(true);
            _editorView.enablePreviewLevel(true);
            _editorView.addLevelSettings(_levelController.level);
            _actorToolbar = new ActorToolbar();
            _actorToolbar.setController(_levelController);
            _editorView.addActorToolbar(_actorToolbar);
        }

        // closeLevel
        public void closeLevel()
        {
            _levelController.closeLevel();
            _editorView.enableNewLevel(true);
            _editorView.enableCloseLevel(false);
            _editorView.enableLoadLevel(true);
            _editorView.enableSaveLevel(false);
            _editorView.enablePreviewLevel(false);
            _editorView.removeLevelSettings();
            _editorView.removeActorToolbar(_actorToolbar);
            _editorView.closeActorProperties();
        }

        // runGame
        public void runGame()
        {
            string levelFileName = _levelController.level.name + ".xml";
            string levelPath = "data\\levels\\" + levelFileName;
            //System.Diagnostics.Process.Start(EditorController.GAME_PATH, String.Format("-l {0}", levelPath));
            ProcessStartInfo startInfo = new ProcessStartInfo(GAME_PATH + "\\StasisGame.exe", "-l " + levelPath);
            startInfo.WorkingDirectory = GAME_PATH;
            System.Diagnostics.Process.Start(startInfo);
        }

        // exit
        public void exit()
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
