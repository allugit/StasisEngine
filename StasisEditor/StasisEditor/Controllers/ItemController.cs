using System;
using System.Collections.Generic;
using StasisCore.Models;

namespace StasisEditor.Controllers
{
    public class ItemController : Controller
    {
        private EditorController _editorController;
        private List<ItemResource> _items;

        public ItemController(EditorController editorController)
        {
            _editorController = editorController;
        }

        // Load resources
        protected override void loadResources()
        {
            _items = new List<ItemResource>();
        }
    }
}
