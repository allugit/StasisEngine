using System;
using System.Collections.Generic;
using StasisCore.Models;
using StasisEditor.Views;

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
        }
    }
}
