﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using Microsoft.Xna.Framework;
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
        private EditorLevel _level;
        private System.Drawing.Point _mouse;
        private System.Drawing.Point _oldMouse;
        private Vector2 _screenCenter;
        private bool _shift;
        private bool _ctrl;
        private EditorActor _selectedActor;

        public System.Drawing.Point mouse
        {
            get { return _mouse; }
            set { _oldMouse = _mouse; _mouse = value; } 
        }
        public EditorLevel level { get { return _level; } set { _level = value; } }
        public LevelView view { get { return _levelView; } }
        public Vector2 screenCenter { get { return _screenCenter; } set { _screenCenter = value; } }
        public bool shift { get { return _shift; } set { _shift = value; } }
        public bool ctrl { get { return _ctrl; } set { _ctrl = value; } }
        public EditorActor selectedActor { get { return _selectedActor; } }
        public EditorController editorController { get { return _editorController; } }

        public LevelController(EditorController editorController, LevelView levelView)
        {
            _editorController = editorController;
            _levelView = levelView;
            _levelView.setController(this);
        }

        public float getScale() { return _editorController.scale; }
        public Vector2 getWorldOffset() { return _screenCenter + (new Vector2(_levelView.Width, _levelView.Height) / 2) / _editorController.scale; }
        public Vector2 getWorldMouse() { return new Vector2(_mouse.X, _mouse.Y) / _editorController.scale - getWorldOffset(); }
        public Vector2 getOldWorldMouse() { return new Vector2(_oldMouse.X, _oldMouse.Y) / _editorController.scale - getWorldOffset(); }

        // Get actor by id
        public EditorActor getActor(int id)
        {
            return _level.getActor(id);
        }

        // Get unused actor id
        public int getUnusedActorID()
        {
            // Method to test if an id is being used
            Func<int, bool> isIdUsed = (id) =>
                {
                    foreach (EditorActor actor in _level.actors)
                    {
                        if (actor.id == id)
                        {
                            id++;
                            return true;
                        }
                    }
                    return false;
                };

            // Start at zero, and increment until an id is not used
            int current = 0;
            while (isIdUsed(current))
                current++;

            return current;
        }

        // createNewLevel
        public void createNewLevel()
        {
            _level = new EditorLevel(this, "new_level");
        }

        // loadLevel
        public void loadLevel(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                XElement data = XElement.Load(fileStream);
                _level = new EditorLevel(this, data);
            }
        }

        // saveLevel
        public void saveLevel()
        {
            _level.save();
        }

        // closeLevel
        public void closeLevel()
        {
            _level = null;
        }

        // Create actor from toolbar
        public void createActorFromToolbar(string buttonName)
        {
            // Create actor controller based on button name
            EditorActor actor = null;
            switch (buttonName)
            {
                case "boxButton":
                    break;

                case "circleButton":
                    break;

                case "movingPlatformButton":
                    break;

                case "terrainButton":
                    break;

                case "ropeButton":
                    break;

                case "fluidButton":
                    break;

                case "playerSpawnButton":
                    // Remove existing player spawns before adding a new one
                    foreach (EditorActor existingActor in _level.actors)
                    {
                        if (existingActor.type == ActorType.PlayerSpawn)
                            _level.removeActor(existingActor);
                    }
                    //actor = new PlayerSpawnActorController(this);
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
                                break;
                        }
                    }
                    break;

                case "itemsButton":
                    SelectItem selectItemForm = new SelectItem(_editorController);
                    if (selectItemForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                    }
                    break;

                case "circuitsButton":
                    SelectCircuit selectCircuitForm = new SelectCircuit(_editorController.circuitController);
                    if (selectCircuitForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                    }
                    break;
            }

            if (actor != null)
            {
                // Show level tab
                _editorController.view.selectLevelTab();

                // Add actor controller to list
                _level.addActor(actor);

                // Select all sub controllers
                selectActor(actor);
            }
        }

        // openActorProperties
        public void openActorProperties(EditorActor actor)
        {
            _editorController.openActorProperties(actor);
        }

        // closeActorProperties
        public void closeActorProperties()
        {
            _editorController.closeActorProperties();
        }

        // Select actor
        public void selectActor(EditorActor actor)
        {
            Debug.Assert(_selectedActor == null);
            _selectedActor = actor;
        }

        // refreshActorProperties
        public void refreshActorProperties()
        {
            _editorController.refreshActorProperties();
        }

        // zoom
        public void zoom(int delta)
        {
            float modifier = 0.01f;
            _editorController.scale += modifier * delta;
        }

        // Update circuit actor connections
        public void updateCircuitActorConnections()
        {
            foreach (EditorActor actor in _level.actors)
            {
                if (actor.type == ActorType.Circuit)
                    throw new NotImplementedException();
                    //(actor as CircuitActorController).updateConnections();
            }
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
                // Move selected actor
            }
        }

        // handleMouseDown
        public void handleMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (_level != null)
            {
            }
        }
    }
}
