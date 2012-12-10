using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Models;
using StasisEditor.Views;

namespace StasisEditor.Controllers
{
    public class ItemController : Controller
    {
        private EditorController _editorController;
        private BindingList<ItemResource> _items;
        private ItemView _itemView;

        public ItemController(EditorController editorController, ItemView itemView)
        {
            _editorController = editorController;
            _itemView = itemView;
        }

        // Load resources
        protected override void loadResources()
        {
            _items = new BindingList<ItemResource>();
        }
    }
}
