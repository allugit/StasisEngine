using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using StasisEditor.Views;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Controllers
{
    using Point = System.Drawing.Point;

    public class WorldMapController : Controller
    {
        private EditorController _editorController;
        private WorldMapView _view;
        private BindingList<EditorWorldMap> _worldMaps;
        private IWorldControl _selectedControl;
        private bool _drawingWorldPath;
        private List<Vector2> _worldPathConstructionPoints;

        public BindingList<EditorWorldMap> worldMaps { get { return _worldMaps; } }
        public IWorldControl selectedControl { get { return _selectedControl; } set { _selectedControl = value; } }
        public bool drawingWorldPath { get { return _drawingWorldPath; } set { _drawingWorldPath = value; } }
        public List<Vector2> worldPathConstructionPoints { get { return _worldPathConstructionPoints; } }

        public WorldMapController(EditorController editorController, WorldMapView worldMapView)
        {
            _editorController = editorController;
            _view = worldMapView;
            _view.controller = this;
            _worldMaps = new BindingList<EditorWorldMap>();
            _worldPathConstructionPoints = new List<Vector2>();

            List<XElement> worldMapData;

            ResourceManager.loadAllWorldMaps(new FileStream(ResourceManager.worldMapPath, FileMode.Open));
            worldMapData = ResourceManager.worldMapResources;
            foreach (XElement data in worldMapData)
                _worldMaps.Add(new EditorWorldMap(data));

            _view.worldMaps = _worldMaps;
        }

        // Get unused  id
        public int getUnusedId(EditorWorldMap worldMap)
        {
            // Method to test if an id is being used
            Func<int, bool> isIdUsed = (id) =>
            {
                foreach (LevelIcon levelIcon in worldMap.levelIcons)
                {
                    if (levelIcon.id == id)
                    {
                        id++;
                        return true;
                    }
                }

                foreach (WorldPath worldPath in worldMap.worldPaths)
                {
                    if (worldPath.id == id)
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

        private bool isUnsavedResourceUsed(string uid)
        {
            // Check unsaved materials
            foreach (EditorWorldMap wm in _worldMaps)
            {
                if (wm.uid == uid)
                    return true;
            }

            return false;
        }

        // Move selected control
        public void moveSelectedControl(Vector2 delta)
        {
            if (_selectedControl != null)
                _selectedControl.position += delta;
        }

        // Hit test controls
        public IWorldControl hitTestControls(Vector2 mouseWorld)
        {
            // Hit test controls by checking for the the shortest distance between the mouse and the control
            float tolerance = 10f;
            float shortestDistance = 999999999f;
            IWorldControl closestControl = null;

            // Test level icons...
            foreach (LevelIcon levelIcon in _view.selectedWorldMap.levelIcons)
            {
                float distance = (levelIcon.position - mouseWorld).Length();
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestControl = levelIcon as EditorLevelIcon;
                }
            }

            // Test world path points...
            foreach (WorldPath worldPath in _view.selectedWorldMap.worldPaths)
            {
                EditorWorldPathPoint[] points = new EditorWorldPathPoint[]
                    {
                        worldPath.controlA as EditorWorldPathPoint,
                        worldPath.controlB as EditorWorldPathPoint,
                        worldPath.pointA as EditorWorldPathPoint,
                        worldPath.pointB as EditorWorldPathPoint
                    };

                foreach (EditorWorldPathPoint point in points)
                {
                    float distance = (point.position - mouseWorld).Length();
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        closestControl = point;
                    }
                }
            }

            // Set selected control
            if (shortestDistance < tolerance)
            {
                return closestControl;
            }

            return null;
        }

        // Handle left click
        public void handleLeftClick(Vector2 mouseWorld)
        {
            if (_drawingWorldPath)
            {
                if (_worldPathConstructionPoints.Count < 2)
                    _worldPathConstructionPoints.Add(mouseWorld);

                if (_worldPathConstructionPoints.Count == 2)
                {
                    Vector2 pointA = _worldPathConstructionPoints[0];
                    Vector2 pointB = _worldPathConstructionPoints[1];
                    Vector2 controlA = (pointA - pointB) + pointA;
                    Vector2 controlB = (pointB - pointA) + pointB;
                    EditorWorldPath worldPath = new EditorWorldPath(_view.selectedWorldMap, controlA, controlB, pointA, pointB, getUnusedId(_view.selectedWorldMap));

                    addWorldPath(worldPath);
                    _worldPathConstructionPoints.Clear();
                    _drawingWorldPath = false;
                }
            }
            else if (_selectedControl == null)
            {
                _selectedControl = hitTestControls(mouseWorld);
            }
            else
            {
                _selectedControl = null;
            }
        }

        // Handle right click
        public void handleRightClick(Vector2 mouseWorld)
        {
            if (!_drawingWorldPath && _selectedControl == null)
            {
                IWorldControl result = hitTestControls(mouseWorld);

                if (result == null)
                    _view.properties = _view.selectedWorldMap;
                else
                    _view.properties = result.self;
            }
        }

        // createWorldMap
        public EditorWorldMap createWorldMap(string uid)
        {
            // Check unsaved resources
            if (isUnsavedResourceUsed(uid))
            {
                System.Windows.Forms.MessageBox.Show(String.Format("An unsaved resource with the uid [{0}] already exists.", uid), "WorldMap Error", System.Windows.Forms.MessageBoxButtons.OK);
                return null;
            }

            EditorWorldMap worldMap = new EditorWorldMap(uid);
            _worldMaps.Add(worldMap);
            return worldMap;
        }

        // removeWorldMap
        public void removeWorldMap(string uid, bool destroy = true)
        {
            EditorWorldMap worldMapToRemove = null;
            foreach (EditorWorldMap worldMap in _worldMaps)
            {
                if (worldMap.uid == uid)
                {
                    worldMapToRemove = worldMap;
                    break;
                }
            }

            System.Diagnostics.Debug.Assert(worldMapToRemove != null);

            _worldMaps.Remove(worldMapToRemove);
        }

        // saveWorldMaps
        public void saveWorldMaps()
        {
            XElement data = new XElement("WorldMaps");

            foreach (EditorWorldMap worldMap in _worldMaps)
                data.Add(worldMap.data);

            EditorResourceManager.saveWorldMapResources(data, true);
        }

        // Delete selected control
        public void deleteSelected()
        {
            if (_selectedControl != null)
                _selectedControl.delete();
            _selectedControl = null;
        }

        // Add level icon
        public void addLevelIcon(EditorLevelIcon levelIcon)
        {
            _view.selectedWorldMap.levelIcons.Add(levelIcon);
        }

        // Add world path
        public void addWorldPath(EditorWorldPath worldPath)
        {
            _view.selectedWorldMap.worldPaths.Add(worldPath);
        }
    }
}
