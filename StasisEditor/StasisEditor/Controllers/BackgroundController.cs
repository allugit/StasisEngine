using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisEditor.Views;
using StasisEditor.Models;
using StasisCore.Resources;
using StasisCore.Controllers;

namespace StasisEditor.Controllers
{
    public class BackgroundController : Controller
    {
        private EditorController _editorController;
        private BackgroundView _view;
        private BindingList<EditorBackground> _backgrounds;

        public BindingList<EditorBackground> backgrounds { get { return _backgrounds; } }

        public BackgroundController(EditorController editorController, BackgroundView view)
        {
            _editorController = editorController;
            _view = view;
            _backgrounds = new BindingList<EditorBackground>();

            List<ResourceObject> resources = ResourceController.loadBackgrounds();

            _view.controller = this;

            foreach (ResourceObject resource in resources)
            {
                _backgrounds.Add(new EditorBackground(resource.data));
            }
            _view.backgrounds = _backgrounds;
        }
    }
}
