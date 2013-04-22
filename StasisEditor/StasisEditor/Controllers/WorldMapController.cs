using System;
using System.Collections.Generic;
using StasisEditor.Views;
using Microsoft.Xna.Framework;

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
        }
    }
}
