using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Resources;
using StasisEditor.Models;
using StasisEditor.Views;
using StasisEditor.Views.Controls;

namespace StasisEditor.Controllers
{
    public class ItemController : Controller
    {
        private EditorController _editorController;
        private ItemView _itemView;
        private List<EditorItem> _items;

        public ItemController(EditorController editorController, ItemView itemView)
        {
            _editorController = editorController;
            _itemView = itemView;
            _itemView.setController(this);
            _items = new List<EditorItem>();
        }

        // getItem
        public EditorItem getItem(string tag)
        {
            foreach (EditorItem item in _items)
                if (item.tag == tag)
                    return item;

            return null;
        }

        // getItems
        public List<EditorItem> getItems()
        {
            return _items;
        }
        public List<EditorItem> getItems(ItemType type)
        {
            List<EditorItem> results = new List<EditorItem>();
            foreach (EditorItem item in _items)
            {
                if (item.type == type)
                    results.Add(item);
            }
            return results;
        }

        // setChangesMade
        public void setChangesMade(bool status)
        {
            _itemView.setChangesMade(status);
        }

        // getAssociatedBlueprintScraps
        public List<EditorBlueprintScrap> getAssociatedBlueprintScraps(EditorBlueprint blueprint)
        {
            List<EditorBlueprintScrap> results = new List<EditorBlueprintScrap>();
            foreach (EditorItem item in _items)
            {
                if (item.type == ItemType.BlueprintScrap)
                {
                    EditorBlueprintScrap scrap = item as EditorBlueprintScrap;
                    if (scrap.blueprintScrapResource.blueprintTag == blueprint.blueprintResource.tag)
                        results.Add(scrap);
                }
            }
            return results;
        }

        // unhookXNAFromLevel
        public void unhookXNAFromLevel()
        {
            _editorController.unhookXNAFromLevel();
        }

        // hookXNAToLevel
        public void hookXNAToLevel()
        {
            _editorController.hookXNAToLevel();
        }

        // enableLevelXNAInput
        public void enableLevelXNAInput(bool status)
        {
            _editorController.enableLevelXNAInput(status);
        }

        // enableLevelXNADrawing
        public void enableLevelXNADrawing(bool status)
        {
            _editorController.enableLevelXNADrawing(status);
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            _itemView.handleXNADraw();
        }

        // update
        public void update()
        {
            // Update mouse position
            _itemView.updateMousePosition();
        }

        // resizeGraphicsDevice
        public void resizeGraphicsDevice(int width, int height)
        {
            _editorController.resizeGraphicsDevice(width, height);
        }
    }
}
