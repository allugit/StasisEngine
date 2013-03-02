using System;
using System.Collections.Generic;
using StasisEditor.Views;

namespace StasisEditor.Controllers
{
    public class BackgroundController : Controller
    {
        private EditorController _editorController;
        private BackgroundView _view;

        public BackgroundController(EditorController editorController, BackgroundView view)
        {
            _editorController = editorController;
            _view = view;
        }
    }
}
