using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;
using StasisEditor.Views;
using StasisEditor.Views.Controls;

namespace StasisEditor.Controllers
{
    public class ItemController : Controller
    {
        private EditorController _editorController;
        private ItemView _itemView;
        private List<ItemResource>[] _items;

        public ItemController(EditorController editorController, ItemView itemView)
        {
            _editorController = editorController;
            _itemView = itemView;
            _itemView.setController(this);
        }

        // getItem
        public ItemResource getItem(string tag)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                foreach (ItemResource itemResource in _items[i])
                    if (itemResource.tag == tag)
                        return itemResource;
            }

            return null;
        }

        // getItems
        public List<ItemResource> getItems(ItemType type)
        {
            return _items[(int)type];
        }

        // getAllItems
        public List<ItemResource> getAllItems()
        {
            List<ItemResource> allItems = new List<ItemResource>();
            for (int i = 0; i < _items.Length; i++)
                allItems.AddRange(_items[i]);
            return allItems;
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
            int numItemTypes = Enum.GetValues(typeof(ItemType)).Length;
            _items = new List<ItemResource>[numItemTypes];
            for (int i = 0; i < numItemTypes; i++)
                _items[i] = new List<ItemResource>();

            // Test data
            _items[(int)ItemType.RopeGun].Add(new RopeGunItemResource("rope_gun", 1, "single_anchor_rope_gun_crate", "single_anchor_rope_gun", false, 32f));
            _items[(int)ItemType.GravityGun].Add(new GravityGunItemResource("gravity_gun", 1, "gravity_gun_crate", "gravity_gun", false, 32f, 4f, 1f));
            _items[(int)ItemType.Grenade].Add(new GrenadeItemResource("grenade", 1, "grenade_crate", "grenade", false, 2f, 1f));
            _items[(int)ItemType.HealthPotion].Add(new HealthPotionItemResource("small_health_potion", 1, "small_health_potion", "small_health_potion", 20));
            _items[(int)ItemType.HealthPotion].Add(new HealthPotionItemResource("medium_health_potion", 1, "medium_health_potion", "medium_health_potion", 40));
            _items[(int)ItemType.HealthPotion].Add(new HealthPotionItemResource("large_health_potion", 1, "large_health_potion", "large_health_potion", 60));
            _items[(int)ItemType.TreeSeed].Add(new TreeSeedItemResource("accuminate_tree_seed", 1, "tree_seed", "tree_seed", null, null));
            _items[(int)ItemType.Blueprint].Add(new BlueprintItemResource("test_blueprint_1", 1, "blueprint", "blueprint", "rope_gun"));
            _items[(int)ItemType.BlueprintScrap].Add(new BlueprintScrapItemResource("test_scrap_1", 1, "blueprint_scrap", "blueprint_scrap", "test_blueprint_1", "test_scrap_1", Vector2.Zero, 0));
            _items[(int)ItemType.BlueprintScrap].Add(new BlueprintScrapItemResource("test_scrap_2", 1, "blueprint_scrap", "blueprint_scrap", "test_blueprint_1", "test_scrap_2", Vector2.Zero, 0));
            _items[(int)ItemType.BlueprintScrap].Add(new BlueprintScrapItemResource("test_scrap_3", 1, "blueprint_scrap", "blueprint_scrap", "test_blueprint_1", "test_scrap_3", Vector2.Zero, 0));
            _items[(int)ItemType.BlueprintScrap].Add(new BlueprintScrapItemResource("test_scrap_4", 1, "blueprint_scrap", "blueprint_scrap", "test_blueprint_1", "test_scrap_4", Vector2.Zero, 0));
        }

        // getBlueprintScrapResources
        public List<BlueprintScrapItemResource> getBlueprintScrapResources()
        {
            List<BlueprintScrapItemResource> list = new List<BlueprintScrapItemResource>();
            List<ItemResource> scrapList = _items[(int)ItemType.BlueprintScrap];

            for (int i = 0; i < scrapList.Count; i++)
                list.Add(scrapList[i] as BlueprintScrapItemResource);

            return list;
        }
        public List<BlueprintScrapItemResource> getBlueprintScrapResources(string blueprintTag)
        {
            List<BlueprintScrapItemResource> list = new List<BlueprintScrapItemResource>();
            List<ItemResource> scrapList = _items[(int)ItemType.BlueprintScrap];

            for (int i = 0; i < scrapList.Count; i++)
            {
                BlueprintScrapItemResource scrapItem = scrapList[i] as BlueprintScrapItemResource;
                if (scrapItem.blueprintTag == blueprintTag)
                    list.Add(scrapItem);
            }

            return list;
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
