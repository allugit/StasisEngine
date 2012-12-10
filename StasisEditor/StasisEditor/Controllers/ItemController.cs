﻿using System;
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
            _items[(int)ItemType.GravityGun].Add(new GravityGunItemResource(new GeneralItemProperties("point_gravity_gun", 1, "point_gravity_gun_crate", "point_gravity_gun")));
        }
    }
}
