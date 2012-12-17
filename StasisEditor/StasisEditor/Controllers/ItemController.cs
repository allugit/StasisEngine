using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;
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

        // Load resources
        protected override void loadResources()
        {
            // Initialize items list
            _items = new List<EditorItem>();

            // Test data
            _items.Add(new EditorItem(new RopeGunItemResource("rope_gun", 1, "single_anchor_rope_gun_crate", "single_anchor_rope_gun", false, 32f)));
            _items.Add(new EditorItem(new GravityGunItemResource("gravity_gun", 1, "gravity_gun_crate", "gravity_gun", false, 32f, 4f, 1f)));
            _items.Add(new EditorItem(new GrenadeItemResource("grenade", 1, "grenade_crate", "grenade", false, 2f, 1f)));
            _items.Add(new EditorItem(new HealthPotionItemResource("small_health_potion", 1, "small_health_potion", "small_health_potion", 20)));
            _items.Add(new EditorItem(new HealthPotionItemResource("medium_health_potion", 1, "medium_health_potion", "medium_health_potion", 40)));
            _items.Add(new EditorItem(new HealthPotionItemResource("large_health_potion", 1, "large_health_potion", "large_health_potion", 60)));
            _items.Add(new EditorItem(new TreeSeedItemResource("accuminate_tree_seed", 1, "tree_seed", "tree_seed", null, null)));
            _items.Add(new EditorBlueprint(new BlueprintItemResource("test_blueprint_1", 1, "blueprint", "blueprint", "rope_gun")));
            _items.Add(new EditorBlueprintScrap(new BlueprintScrapItemResource("test_scrap_1", 1, "blueprint_scrap", "blueprint_scrap", "test_blueprint_1", "test_scrap_1", Vector2.Zero, 0)));
            _items.Add(new EditorBlueprintScrap(new BlueprintScrapItemResource("test_scrap_2", 1, "blueprint_scrap", "blueprint_scrap", "test_blueprint_1", "test_scrap_2", Vector2.Zero, 0)));
            _items.Add(new EditorBlueprintScrap(new BlueprintScrapItemResource("test_scrap_3", 1, "blueprint_scrap", "blueprint_scrap", "test_blueprint_1", "test_scrap_3", Vector2.Zero, 0)));
            _items.Add(new EditorBlueprintScrap(new BlueprintScrapItemResource("test_scrap_4", 1, "blueprint_scrap", "blueprint_scrap", "test_blueprint_1", "test_scrap_4", Vector2.Zero, 0)));
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
