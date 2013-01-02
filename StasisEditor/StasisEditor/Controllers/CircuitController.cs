using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisEditor.Views;

namespace StasisEditor.Controllers
{
    public class CircuitController : Controller
    {
        private EditorController _editorController;
        private CircuitsView _view;
        private System.Drawing.Point _mouse;
        private System.Drawing.Point _oldMouse;
        private Vector2 _screenCenter;
        private float _scale = 35;

        public float getScale() { return _scale; }
        public Vector2 getWorldOffset() { return _screenCenter + (new Vector2(_view.Width, _view.Height) / 2) / _scale; }
        public Vector2 getWorldMouse() { return new Vector2(_mouse.X, _mouse.Y) / _scale - getWorldOffset(); }
        public Vector2 getOldWorldMouse() { return new Vector2(_oldMouse.X, _oldMouse.Y) / _scale - getWorldOffset(); }

        public CircuitController(EditorController editorController, CircuitsView circuitsView) : base()
        {
            _editorController = editorController;
            _view = circuitsView;
            _view.setController(this);
        }
    }
}
