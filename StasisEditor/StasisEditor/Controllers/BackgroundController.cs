using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Views;
using StasisEditor.Models;
using StasisCore.Resources;
using StasisCore.Controllers;

namespace StasisEditor.Controllers
{
    using Point = System.Drawing.Point;

    public class BackgroundController : Controller
    {
        private EditorController _editorController;
        private BackgroundView _view;
        private BindingList<EditorBackground> _backgrounds;
        private bool _ctrl;
        private Point _mouse;
        private Point _oldMouse;
        private Vector2 _screenCenter;

        public BindingList<EditorBackground> backgrounds { get { return _backgrounds; } }
        public Point mouse
        {
            get { return _mouse; }
            set { _oldMouse = _mouse; _mouse = value; }
        }
        public Vector2 mouseDelta { get { return new Vector2(_mouse.X - _oldMouse.X, _mouse.Y - _oldMouse.Y); } }
        public bool ctrl { get { return _ctrl; } set { _ctrl = value; } }
        public Vector2 screenCenter { get { return _screenCenter; } set { _screenCenter = value; } }

        public BackgroundController(EditorController editorController, BackgroundView view)
        {
            _editorController = editorController;
            _view = view;
            _backgrounds = new BindingList<EditorBackground>();

            List<ResourceObject> resources = ResourceController.loadBackgrounds();

            _view.controller = this;

            foreach (ResourceObject resource in resources)
                _backgrounds.Add(new EditorBackground(resource.data));

            _view.backgrounds = _backgrounds;
        }

        public void saveBackgrounds()
        {
            XElement data = new XElement("Backgrounds");

            foreach (EditorBackground background in _backgrounds)
                data.Add(background.data);

            ResourceController.saveBackgroundResources(data);
        }
    }
}
