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

        // getItems
        public List<ItemResource> getItems(ItemType type)
        {
            return _items[(int)type];
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
            _items[(int)ItemType.RopeGun].Add(new RopeGunItemResource(new GeneralItemProperties("single_anchor_rope_gun", 1, "single_anchor_rope_gun_crate", "single_anchor_rope_gun")));
            _items[(int)ItemType.GravityGun].Add(new GravityGunItemResource(new GeneralItemProperties("gravity_gun", 1, "gravity_gun_crate", "gravity_gun")));
            _items[(int)ItemType.Grenade].Add(new GrenadeItemResource(new GeneralItemProperties("grenade", 1, "grenade_crate", "grenade")));
            _items[(int)ItemType.HealthPotion].Add(new HealthPotionItemResource(new GeneralItemProperties("small_health_potion", 1, "small_health_potion", "small_health_potion"), new HealthPotionProperties(20)));
            _items[(int)ItemType.HealthPotion].Add(new HealthPotionItemResource(new GeneralItemProperties("medium_health_potion", 1, "medium_health_potion", "medium_health_potion"), new HealthPotionProperties(40)));
            _items[(int)ItemType.HealthPotion].Add(new HealthPotionItemResource(new GeneralItemProperties("large_health_potion", 1, "large_health_potion", "large_health_potion"), new HealthPotionProperties(60)));
            _items[(int)ItemType.TreeSeed].Add(new TreeSeedItemResource(
                new GeneralItemProperties("accuminate_tree_seed", 1, "tree_seed", "tree_seed"),
                new TreeProperties(Vector2.Zero),
                new GeneralPlantProperties(true, 0f, "")));

            _items[(int)ItemType.BlueprintScrap].Add(new BlueprintScrapItemResource(
                null,
                null,
                new GeneralItemProperties("test_scrap_1", 1, "blueprint_scrap", "blueprint_scrap"),
                new BlueprintScrapProperties("test_scrap_1", "test_blueprint_1")));

            _items[(int)ItemType.BlueprintScrap].Add(new BlueprintScrapItemResource(
                null,
                null,
                new GeneralItemProperties("test_scrap_2", 1, "blueprint_scrap", "blueprint_scrap"),
                new BlueprintScrapProperties("test_scrap_2", "test_blueprint_1")));

            _items[(int)ItemType.BlueprintScrap].Add(new BlueprintScrapItemResource(
                null,
                null,
                new GeneralItemProperties("test_scrap_3", 1, "blueprint_scrap", "blueprint_scrap"),
                new BlueprintScrapProperties("test_scrap_3", "test_blueprint_1")));

            _items[(int)ItemType.Blueprint].Add(new BlueprintItemResource(
                new GeneralItemProperties("test_blueprint_1", 1, "blueprint", "blueprint"),
                new BlueprintProperties("test_item")));
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
                if (scrapItem.blueprintScrapProperties.blueprintTag == blueprintTag)
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
