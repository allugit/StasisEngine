using System;
using System.Collections.Generic;
using StasisEditor.Views;

namespace StasisEditor.Controllers
{
    public class BlueprintController : Controller
    {
        private EditorController _editorController;
        private BlueprintView _blueprintView;

        public BlueprintController(EditorController controller, BlueprintView blueprintView)
            : base()
        {
            _editorController = controller;
            _blueprintView = blueprintView;
            _blueprintView.setController(this);
        }
    }
}
