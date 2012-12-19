using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore.Models;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    abstract public class EditorItem
    {
        private ItemResource _resource;
        private bool _changed;

        [Browsable(false)]
        public ItemResource itemResource { get { return _resource; } }

        [Browsable(false)]
        public bool changed { get { return _changed; } set { _changed = value; } }

        [Browsable(false)]
        public ItemType type { get { return _resource.type; } }

        [CategoryAttribute("General")]
        [DisplayName("Tag")]
        public string tag { get { return _resource.tag; } set { _resource.tag = value; } }

        [CategoryAttribute("General")]
        [DisplayName("Quantity")]
        public int quantity { get { return _resource.quantity; } set { _resource.quantity = value; } }

        [CategoryAttribute("General")]
        [DisplayName("Inventory Texture Tag")]
        public string inventoryTextureTag { get { return _resource.inventoryTextureTag; } set { _resource.inventoryTextureTag = value; } }

        [CategoryAttribute("General")]
        [DisplayName("World Texture Tag")]
        public string worldTextureTag { get { return _resource.worldTextureTag; } set { _resource.worldTextureTag = value; } }

        public EditorItem(ItemResource resource)
        {
            _resource = resource;
        }

        // create
        public static EditorItem create(ItemController itemController, ItemResource resource)
        {
            EditorItem item = null;
            switch (resource.type)
            {
                case ItemType.Blueprint:
                    item = new EditorBlueprint(resource);
                    break;

                case ItemType.BlueprintScrap:
                    item = new EditorBlueprintScrap(itemController, resource);
                    break;

                case ItemType.GravityGun:
                    item = new EditorGravityGun(resource);
                    break;

                case ItemType.Grenade:
                    item = new EditorGrenade(resource);
                    break;

                case ItemType.HealthPotion:
                    item = new EditorHealthPotion(resource);
                    break;

                case ItemType.RopeGun:
                    item = new EditorRopeGun(resource);
                    break;

                case ItemType.TreeSeed:
                    item = new EditorTreeSeed(resource);
                    break;
            }
            return item;
        }

        // toXML
        abstract public XElement toXML();

        // ToString
        public override string ToString()
        {
            return tag;
        }
    }
}
