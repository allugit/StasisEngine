using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using StasisEditor.Views;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisEditor.Models;

namespace StasisEditor.Controllers
{
    using Point = System.Drawing.Point;

    public class WorldMapController : Controller
    {
        private EditorController _editorController;
        private WorldMapView _view;
        private bool _ctrl;
        private Point _mouse;
        private Point _oldMouse;
        private Vector2 _screenCenter;
        private BindingList<EditorWorldMap> _worldMaps;

        public Point mouse
        {
            get { return _mouse; }
            set { _oldMouse = _mouse; _mouse = value; }
        }
        public Vector2 mouseDelta { get { return new Vector2(_mouse.X - _oldMouse.X, _mouse.Y - _oldMouse.Y); } }
        public bool ctrl { get { return _ctrl; } set { _ctrl = value; } }
        public Vector2 screenCenter { get { return _screenCenter; } set { _screenCenter = value; } }

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
    }
}
