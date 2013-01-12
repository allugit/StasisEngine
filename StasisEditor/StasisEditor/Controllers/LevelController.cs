using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers.Actors;
using StasisEditor.Views;
using StasisEditor.Views.Controls;
using StasisEditor.Models;
using StasisCore;

namespace StasisEditor.Controllers
{
    public class LevelController : Controller
    {
        public const float MIN_ACTOR_SIZE = 0.1f;

        private EditorController _editorController;
        private LevelView _levelView;

        private List<ActorSubController> _selectedSubControllers;
        private List<ActorSubController> _subControllerSelectQueue;
        private List<ActorSubController> _subControllerDeselectQueue;

        private List<ActorController> _actorControllers;
        private List<ActorController> _actorControllersAddQueue;
        private List<ActorController> _actorControllersRemoveQueue;

        private EditorLevel _level;

        private System.Drawing.Point _mouse;
        private System.Drawing.Point _oldMouse;
        private Vector2 _screenCenter;
        private bool _shift;
        private bool _ctrl;

        public System.Drawing.Point mouse
        {
            get { return _mouse; }
            set { _oldMouse = _mouse; _mouse = value; } 
        }
        public EditorLevel level { get { return _level; } set { _level = value; } }
        public LevelView view { get { return _levelView; } }
        public List<ActorSubController> selectedSubControllers { get { return _selectedSubControllers; } }
        public Vector2 screenCenter { get { return _screenCenter; } set { _screenCenter = value; } }
        public bool shift { get { return _shift; } set { _shift = value; } }
        public bool ctrl { get { return _ctrl; } set { _ctrl = value; } }

        public LevelController(EditorController editorController, LevelView levelView)
        {
            _editorController = editorController;
            _levelView = levelView;
            _levelView.setController(this);

            _selectedSubControllers = new List<ActorSubController>();
            _subControllerSelectQueue = new List<ActorSubController>();
            _subControllerDeselectQueue = new List<ActorSubController>();

            _actorControllers = new List<ActorController>();
            _actorControllersAddQueue = new List<ActorController>();
            _actorControllersRemoveQueue = new List<ActorController>();

            System.Windows.Forms.Application.Idle += delegate { processActorControllerQueue(); };
        }

        #region Getters/Setters

        public float getScale() { return _editorController.getScale(); }
        public Vector2 getWorldOffset() { return _screenCenter + (new Vector2(_levelView.Width, _levelView.Height) / 2) / _editorController.getScale(); }
        public Vector2 getWorldMouse() { return new Vector2(_mouse.X, _mouse.Y) / _editorController.getScale() - getWorldOffset(); }
        public Vector2 getOldWorldMouse() { return new Vector2(_oldMouse.X, _oldMouse.Y) / _editorController.getScale() - getWorldOffset(); }

        // getActorControllers
        public List<ActorController> getActorControllers()
        {
            return _actorControllers;
        }

        #endregion

        // Process actor controller queues
        public void processActorControllerQueue()
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
        }

        // createNewLevel
        public void createNewLevel()
        {
            _level = new EditorLevel();
        }

        // closeLevel
        public void closeLevel()
        {
            _level = null;
        }

        // createActorControllerFromToolbar
        public void createActorControllerFromToolbar(string buttonName)
        {
            // Create actor controller based on button name
            ActorController actorController = null;
            switch (buttonName)
            {
                case "boxButton":
                    actorController = new BoxActorController(this);
                    break;

                case "circleButton":
                    actorController = new CircleActorController(this);
                    break;

                case "movingPlatformButton":
                    actorController = new PlatformActorController(this);
                    break;

                case "terrainButton":
                    actorController = new TerrainActorController(this);
                    break;

                case "ropeButton":
                    actorController = new RopeActorController(this);
                    break;

                case "fluidButton":
                    actorController = new FluidActorController(this);
                    break;

                case "playerSpawnButton":
                    // Remove existing player spawns before adding a new one
                    foreach (ActorController controller in _actorControllers)
                    {
                        if (controller.type == ActorType.PlayerSpawn)
                            removeActorController(controller);
                    }
                    actorController = new PlayerSpawnActorController(this);
                    break;

                case "plantsButton":
                    PlantSelectBox plantSelectBox = new PlantSelectBox();
                    if (plantSelectBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        // Adjust mouse position since movement isn't tracked when a form is open
                        _mouse = _levelView.PointToClient(System.Windows.Forms.Cursor.Position);
                        _oldMouse = _mouse;

                        PlantType selectedPlantType = plantSelectBox.selectedPlantType;
                        switch (selectedPlantType)
                        {
                            case PlantType.Tree:
                                actorController = new TreeActorController(this);
                                break;
                        }
                    }
                    break;

                case "itemsButton":
                    // TODO: Implement item actors
                    break;

                case "circuitsButton":
                    SelectCircuit selectCircuitForm = new SelectCircuit(_editorController.circuitController);
                    if (selectCircuitForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {

                    }
                    break;
            }

            if (actorController != null)
            {
                // Add actor controller to list
                addActorController(actorController);

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
        public void addActorController(ActorController actorController)
        {
            _actorControllersAddQueue.Add(actorController);
        }

        // removeActorController
        public void removeActorController(ActorController actorController)
        {
            _actorControllersRemoveQueue.Add(actorController);
        }

        // openActorProperties
        public void openActorProperties(List<ActorProperties> properties)
        {
            _editorController.openActorProperties(properties);
        }

        // closeActorProperties
        public void closeActorProperties()
        {
            _editorController.closeActorProperties();
        }

        // refreshActorProperties
        public void refreshActorProperties()
        {
            _editorController.refreshActorProperties();
        }

        // handleMouseMove
        public void handleMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            // Update mouse position
            mouse = e.Location;
            Vector2 worldDelta = getWorldMouse() - getOldWorldMouse();

            if (ctrl)
            {
                // Move screen
                _screenCenter += worldDelta;
            }
            else
            {
                // Move selected sub controllers
                foreach (ActorSubController subController in _selectedSubControllers)
                    subController.handleMouseMove(worldDelta);
            }
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
                    foreach (ActorController actorResourceController in _actorControllers)
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
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // Try to select a sub controller
                foreach (ActorController actorResourceController in _actorControllers)
                {
                    // Stop searching if a hit test returns true (actor controller will handle the selection of the appropriate sub controls)
                    if (actorResourceController.hitTest(getWorldMouse(), false))
                    {
                        closeActorProperties();
                        openActorProperties(actorResourceController.properties);
                        break;
                    }
                }
            }
        }
    }
}
