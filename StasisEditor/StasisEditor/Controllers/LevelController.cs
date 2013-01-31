using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisEditor.Views;
using StasisEditor.Views.Controls;
using StasisEditor.Models;
using StasisCore;

namespace StasisEditor.Controllers
{
    using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
    using Keys = Microsoft.Xna.Framework.Input.Keys;

    public class LevelController : Controller
    {
        public const float MIN_ACTOR_SIZE = 0.1f;

        private EditorController _editorController;
        private LevelView _levelView;
        private EditorLevel _level;
        private System.Drawing.Point _mouse;
        private System.Drawing.Point _oldMouse;
        private Vector2 _screenCenter;
        private EditorActor _selectedActor;
        private KeyboardState _keyState;
        private KeyboardState _oldKeyState;
        private bool _mouseOverView;

        public System.Drawing.Point mouse { get { return _mouse; } set { _oldMouse = _mouse; _mouse = value; } }
        public EditorLevel level { get { return _level; } set { _level = value; } }
        public LevelView view { get { return _levelView; } }
        public Vector2 screenCenter { get { return _screenCenter; } set { _screenCenter = value; } }
        public bool shift { get { return _keyState.IsKeyDown(Keys.LeftShift) || _keyState.IsKeyDown(Keys.RightShift); } }
        public bool ctrl { get { return _keyState.IsKeyDown(Keys.LeftControl) || _keyState.IsKeyDown(Keys.RightControl); } }
        public bool mouseOverView { get { return _mouseOverView; } set { _mouseOverView = value; } }
        public Vector2 worldOffset { get { return _screenCenter + (new Vector2(_levelView.Width, _levelView.Height) / 2) / _editorController.scale; } }
        public Vector2 worldMouse { get { return new Vector2(_mouse.X, _mouse.Y) / _editorController.scale - worldOffset; } }
        public Vector2 oldWorldMouse { get { return new Vector2(_oldMouse.X, _oldMouse.Y) / _editorController.scale - worldOffset; } }
        public Vector2 worldDeltaMouse { get { return new Vector2(_mouse.X - _oldMouse.X, _mouse.Y - _oldMouse.Y) / _editorController.scale; } }
        public float scale { get { return _editorController.scale; } }

        public EditorActor selectedActor { get { return _selectedActor; } set { _selectedActor = value; } }
        public EditorController editorController { get { return _editorController; } }

        public LevelController(EditorController editorController, LevelView levelView)
        {
            _editorController = editorController;
            _levelView = levelView;
            _levelView.setController(this);
            Application.Idle += new EventHandler(update);
            Application.Idle += new EventHandler(draw);
        }

        // Is key pressed
        public bool isKeyPressed(Keys key)
        {
            return _keyState.IsKeyDown(key) && _oldKeyState.IsKeyUp(key);
        }

        // Is key being held
        public bool isKeyHeld(Keys key)
        {
            return _keyState.IsKeyDown(key);
        }

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
            _levelView.Invalidate();
        }

        // Create actor from toolbar
        public void createActorFromToolbar(string buttonName)
        {
            // Create actor controller based on button name
            EditorActor actor = null;
            switch (buttonName)
            {
                case "boxButton":
                    actor = new EditorBoxActor(_level);
                    break;

                case "circleButton":
                    actor = new EditorCircleActor(_level);
                    break;

                case "movingPlatformButton":
                    break;

                case "terrainButton":
                    actor = new EditorTerrainActor(_level);
                    break;

                case "ropeButton":
                    actor = new EditorRopeActor(_level);
                    break;

                case "fluidButton":
                    actor = new EditorFluidActor(_level);
                    break;

                case "playerSpawnButton":
                    // Remove existing player spawns before adding a new one
                    foreach (EditorActor existingActor in _level.actors)
                    {
                        if (existingActor.type == ActorType.PlayerSpawn)
                            _level.removeActor(existingActor);
                    }
                    actor = new EditorPlayerSpawnActor(_level);
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
                                actor = new EditorTreeActor(_level);
                                break;
                        }
                    }
                    break;

                case "itemsButton":
                    SelectItem selectItemForm = new SelectItem(_editorController);
                    if (selectItemForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        actor = new EditorItemActor(_level, selectItemForm.uid);
                    }
                    break;

                case "circuitsButton":
                    SelectCircuit selectCircuitForm = new SelectCircuit(_editorController.circuitController);
                    if (selectCircuitForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        actor = new EditorCircuitActor(_level, selectCircuitForm.circuitUID);
                    }
                    break;
            }

            if (actor != null)
            {
                _editorController.view.selectLevelTab();
                _level.addActor(actor);
                actor.select();
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

        // refreshActorProperties
        public void refreshActorProperties()
        {
            _editorController.refreshActorProperties();
        }

        // Zoom
        public void zoom(int delta)
        {
            float modifier = 0.01f;
            _editorController.scale = Math.Max(_editorController.scale + modifier * delta, 1f);
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

        // Handle mouse down
        public void handleMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            Vector2 worldMouse = this.worldMouse;
            if (_level != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (_selectedActor == null)
                    {
                        // Try to select an actor
                        foreach (EditorActor actor in _level.actors)
                        {
                            if (actor.hitTest(worldMouse))
                            {
                                actor.handleMouseDown();
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Let selected actor handle mouse down
                        _selectedActor.handleMouseDown();
                    }
                }
            }
        }

        // Box hit test
        public bool hitTestBox(Vector2 testPoint, Vector2 boxPosition, float halfWidth, float halfHeight, float angle)
        {
            // Get the mouse position relative to the box's center
            Vector2 relativePosition = boxPosition - worldMouse;

            // Rotate the relative mouse position by the negative angle of the box
            Vector2 transformedRelativePosition = Vector2.Transform(relativePosition, Matrix.CreateRotationZ(-angle));

            // Get the local, cartiasian-aligned bounding-box
            Vector2 topLeft = -(new Vector2(halfWidth, halfHeight));
            Vector2 bottomRight = -topLeft;

            // Check if the relative mouse point is inside the bounding box
            Vector2 d1, d2;
            d1 = transformedRelativePosition - topLeft;
            d2 = bottomRight - transformedRelativePosition;

            // One of these components will be less than zero if the alignedRelative position is outside of the bounds
            if (d1.X < 0 || d1.Y < 0)
                return false;

            if (d2.X < 0 || d2.Y < 0)
                return false;

            return true;
        }

        // Line hit test
        public bool hitTestLine(Vector2 testPoint, Vector2 pointA, Vector2 pointB)
        {
            Vector2 difference = pointB - pointA;
            Vector2 midPoint = (pointA + pointB) / 2;
            float halfWidth = difference.Length() / 2f;
            float halfHeight = 6f / scale;
            float angle = (float)Math.Atan2(difference.Y, difference.X);
            return hitTestBox(testPoint, midPoint, halfWidth, halfHeight, angle);
        }

        // Circle hit test
        public bool hitTestCircle(Vector2 testPoint, Vector2 circlePosition, float radius)
        {
            return (testPoint - circlePosition).Length() <= radius;
        }

        // Point hit test
        public bool hitTestPoint(Vector2 testPoint, Vector2 pointPosition, float marginInPixels = 6f)
        {
            return hitTestCircle(testPoint, pointPosition, marginInPixels / scale);
        }

        // Polygon hit test -- http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
        public bool hitTestPolygon(Vector2 point, List<Vector2> points)
        {
            bool hit = false;
            for (int i = 0, j = points.Count - 1; i < points.Count; j = i++)
            {
                if (((points[i].Y > point.Y) != (points[j].Y > point.Y)) &&
                    (point.X < (points[j].X - points[i].X) * (point.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))
                    hit = !hit;
            }

            return hit;
        }

        // Update
        private void update(object sender, EventArgs e)
        {
            _keyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            if (_level != null)
            {
                if (_mouseOverView)
                {
                    _mouse = _levelView.PointToClient(Cursor.Position);
                }

                if (ctrl)
                {
                    _screenCenter += worldMouse - oldWorldMouse;
                }

                _level.update();

                if (_mouseOverView)
                {
                    _oldMouse = _mouse;
                }
            }

            _oldKeyState = _keyState;
        }

        // Draw
        void draw(object sender, EventArgs e)
        {
            if (_level != null)
            {
                _levelView.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                _level.draw();
                _levelView.spriteBatch.End();
                _levelView.Invalidate();
            }
        }
    }
}
