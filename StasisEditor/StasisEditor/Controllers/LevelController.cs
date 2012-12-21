using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers.Actors;
using StasisEditor.Views;
using StasisCore.Resources;
using StasisEditor.Views.Controls;

namespace StasisEditor.Controllers
{
    public class LevelController : Controller
    {
        public const float MIN_ACTOR_SIZE = 0.1f;

        private EditorController _editorController;
        private LevelView _levelView;
        private ShapeRenderer _shapeRenderer;
        private bool _xnaInputEnabled = true;
        private bool _xnaDrawingEnabled = true;

        private List<ActorSubController> _selectedSubControllers;
        private List<ActorSubController> _subControllerSelectQueue;
        private List<ActorSubController> _subControllerDeselectQueue;

        private List<ActorResourceController> _actorControllers;
        private List<ActorResourceController> _actorControllersAddQueue;
        private List<ActorResourceController> _actorControllersRemoveQueue;

        private LevelResource _level;

        private System.Drawing.Point _mouse;
        private Vector2 _screenCenter;

        public LevelController(EditorController editorController, LevelView levelView)
        {
            _editorController = editorController;
            _levelView = levelView;
            _levelView.setController(this);
            _levelView.hookToXNA();

            _selectedSubControllers = new List<ActorSubController>();
            _subControllerSelectQueue = new List<ActorSubController>();
            _subControllerDeselectQueue = new List<ActorSubController>();

            _actorControllers = new List<ActorResourceController>();
            _actorControllersAddQueue = new List<ActorResourceController>();
            _actorControllersRemoveQueue = new List<ActorResourceController>();
        }

        #region Getters/Setters

        public float getScale() { return _editorController.getScale(); }
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
            if (_xnaDrawingEnabled)
            {
                if (_level != null)
                    _levelView.handleXNADraw();
            }
        }

        // hookXNAToView
        public void hookXNAToView()
        {
            _levelView.hookToXNA();
        }

        // unhookXNAFromView
        public void unhookXNAFromView()
        {
            _levelView.unhookFromXNA();
        }

        // enableXNAInput
        public void enableXNAInput(bool status)
        {
            _xnaInputEnabled = status;
        }

        // enableXNADrawing
        public void enableXNADrawing(bool status)
        {
            _xnaDrawingEnabled = status;
        }

        // update
        public void update()
        {
            // Selection queue
            while (_subControllerSelectQueue.Count > 0)
            {
                int index = _subControllerSelectQueue.Count - 1;
                _selectedSubControllers.Add(_subControllerSelectQueue[index]);
                _subControllerSelectQueue.Remove(_subControllerSelectQueue[index]);
            }

            // Deselection queue
            while (_subControllerDeselectQueue.Count > 0)
            {
                int index = _subControllerDeselectQueue.Count - 1;
                _selectedSubControllers.Remove(_subControllerDeselectQueue[index]);
                _subControllerDeselectQueue.Remove(_subControllerDeselectQueue[index]);
            }

            // Actor controller add queue
            while (_actorControllersAddQueue.Count > 0)
            {
                int index = _actorControllersAddQueue.Count - 1;
                _actorControllers.Add(_actorControllersAddQueue[index]);
                _actorControllersAddQueue.Remove(_actorControllersAddQueue[index]);
            }

            // Actor controlle remove queue
            while (_actorControllersRemoveQueue.Count > 0)
            {
                int index = _actorControllersRemoveQueue.Count - 1;
                _actorControllers.Remove(_actorControllersRemoveQueue[index]);
                _actorControllersRemoveQueue.Remove(_actorControllersRemoveQueue[index]);
            }

            if (_xnaInputEnabled)
            {
                // Update mouse position
                updateMousePosition();

                // Check XNA keys in selected sub controllers
                foreach (ActorSubController subController in _selectedSubControllers)
                    subController.checkXNAKeys();

                // Let all actor resource controllers listen to key presses
                foreach (ActorResourceController controller in _actorControllers)
                    controller.globalCheckKey();
            }
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

                case "circleButton":
                    actorController = new CircleActorResourceController(this);
                    break;

                case "movingPlatformButton":
                    actorController = new MovingPlatformActorResourceController(this);
                    break;

                case "pressurePlateButton":
                    actorController = new PressurePlateActorResourceController(this);
                    break;

                case "terrainButton":
                    actorController = new TerrainActorResourceController(this);
                    break;

                case "objectSpawnerButton":
                    actorController = new ObjectSpawnerResourceController(this);
                    break;

                case "ropeButton":
                    actorController = new RopeActorResourceController(this);
                    break;

                case "fluidButton":
                    actorController = new FluidActorResourceController(this);
                    break;

                case "playerSpawnButton":
                    // Remove existing player spawns before adding a new one
                    foreach (ActorResourceController controller in _actorControllers)
                    {
                        if (controller.type == ActorType.PlayerSpawn)
                            removeActorController(controller);
                    }
                    actorController = new PlayerSpawnActorResourceController(this);
                    break;
            }

            if (actorController != null)
            {
                // Add actor controller to list
                _actorControllers.Add(actorController);

                // Select all sub controllers
                actorController.selectAllSubControllers();
            }
        }

        // clearSubControllers
        public void clearSubControllers()
        {
            _selectedSubControllers.Clear();
        }

        // selectSubController
        public void selectSubController(ActorSubController subController)
        {
            // actual selection is handled in update() in the 'XNA Methods' region
            _subControllerSelectQueue.Add(subController);
            subController.selected = true;
        }

        // deselectSubController
        public void deselectSubController(ActorSubController subController)
        {
            // actual deselection is handled in update() in the 'XNA Methods' region
            _subControllerDeselectQueue.Add(subController);
            subController.selected = false;
        }

        // addActorController
        public void addActorController(ActorResourceController actorController)
        {
            // actual addition of actor controller is handled in update() in the 'XNA Methods' region
            _actorControllersAddQueue.Add(actorController);
        }

        // removeActorController
        public void removeActorController(ActorResourceController actorController)
        {
            // actual removal of actor controller is handled in update() in the 'XNA Methods' region
            _actorControllersRemoveQueue.Add(actorController);
        }

        // selectPlantType
        public void selectPlantType()
        {
            PlantSelectBox plantSelectBox = new PlantSelectBox();
            ActorResourceController actorController = null;
            if (plantSelectBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PlantType selectedPlantType = plantSelectBox.selectedPlantType;
                switch (selectedPlantType)
                {
                    case PlantType.Tree:
                        actorController = new TreeActorResourceController(this);
                        break;
                }
            }

            if (actorController != null)
            {
                // Add actor to controller list
                addActorController(actorController);

                // Select sub controllers
                actorController.selectAllSubControllers();
            }
        }

        // selectItem
        public void selectItem()
        {
            ItemSelectBox itemSelectBox = new ItemSelectBox(_editorController.getItemController());
            if (itemSelectBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        #endregion

        #region Input

        // Update mouse position
        private void updateMousePosition()
        {
            // View offset
            System.Drawing.Point viewOffset = _levelView.FindForm().PointToClient(_levelView.Parent.PointToScreen(_levelView.Location));

            // Set mouse boundaries
            int x = Math.Min(Math.Max(0, Input.newMouse.X - viewOffset.X), _levelView.getWidth());
            int y = Math.Min(Math.Max(0, Input.newMouse.Y - viewOffset.Y), _levelView.getHeight());

            // Calculate change in mouse position (for screen and world coordinates)
            int deltaX = x - _mouse.X;
            int deltaY = y - _mouse.Y;
            Vector2 worldDelta = new Vector2(deltaX, deltaY) / _editorController.getScale();

            // Move screen
            if (Input.newKey.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                _screenCenter += worldDelta;

            // Store screen space mouse coordinates
            _mouse.X = x;
            _mouse.Y = y;

            // Pass input to selected sub controllers
            foreach (ActorSubController subController in _selectedSubControllers)
                subController.handleMouseMove(worldDelta);
        }

        // handleMouseDown
        public void handleMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            // Handle left mouse down
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_selectedSubControllers.Count == 0)
                {
                    // Try to select a sub controller
                    foreach (ActorResourceController actorResourceController in _actorControllers)
                    {
                        // Stop searching if a hit test returns true (actor controller will handle the selection of the appropriate sub controls)
                        if (actorResourceController.hitTest(getWorldMouse()))
                            break;
                    }
                }
                else
                {
                    // Pass input to selected sub controllers
                    foreach (ActorSubController subController in _selectedSubControllers)
                        subController.handleLeftMouseDown();
                }
            }
        }
        
        #endregion
    }
}
