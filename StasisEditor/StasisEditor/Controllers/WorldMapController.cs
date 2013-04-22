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

        public BindingList<EditorWorldMap> worldMaps { get { return _worldMaps; } }

        public WorldMapController(EditorController editorController, WorldMapView worldMapView)
        {
            _editorController = editorController;
            _view = worldMapView;
            _view.controller = this;
            _worldMaps = new BindingList<EditorWorldMap>();

            List<XElement> worldMapData;

            ResourceManager.loadAllWorldMaps(new FileStream(ResourceManager.worldMapPath, FileMode.Open));
            worldMapData = ResourceManager.worldMapResources;
            foreach (XElement data in worldMapData)
                _worldMaps.Add(new EditorWorldMap(data));

            _view.worldMaps = _worldMaps;
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
    }
}
