using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;

namespace StasisCore.Resources
{
    public enum ItemType
    {
        RopeGun,
        GravityGun,
        Grenade,
        HealthPotion,
        TreeSeed,
        BlueprintScrap,
        Blueprint
    };

    abstract public class ItemResource
    {
        protected ItemType _type;
        protected string _tag;
        protected int _quantity;
        protected string _inventoryTextureTag;
        protected string _worldTextureTag;

        public ItemType type { get { return _type; } }
        public string tag { get { return _tag; } set { _tag = value; } }
        public int quantity { get { return _quantity; } set { _quantity = value; } }
        public string inventoryTextureTag { get { return _inventoryTextureTag; } set { _inventoryTextureTag = value; } }
        public string worldTextureTag { get { return _worldTextureTag; } set { _worldTextureTag = value; } }

        public ItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag)
        {
            _tag = tag;
            _quantity = quantity;
            _worldTextureTag = worldTextureTag;
            _inventoryTextureTag = inventoryTextureTag;
        }

        // load
        public static ItemResource load(string filePath)
        {
            ItemResource resource = null;

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                XElement element = XElement.Load(stream);
                ItemType type = (ItemType)Enum.Parse(typeof(ItemType), element.Attribute("type").Value);

                switch (type)
                {
                    case ItemType.Blueprint:
                        resource = BlueprintItemResource.fromXML(element);
                        break;

                    case ItemType.BlueprintScrap:
                        resource = BlueprintScrapItemResource.fromXML(element);
                        break;

                    case ItemType.GravityGun:
                        resource = GravityGunItemResource.fromXML(element);
                        break;

                    case ItemType.Grenade:
                        resource = GrenadeItemResource.fromXML(element);
                        break;

                    case ItemType.HealthPotion:
                        resource = HealthPotionItemResource.fromXML(element);
                        break;

                    case ItemType.RopeGun:
                        resource = RopeGunItemResource.fromXML(element);
                        break;

                    case ItemType.TreeSeed:
                        resource = TreeSeedItemResource.fromXML(element);
                        break;
                }
            }

            return resource;
        }

        // toXML
        abstract public XElement toXML();

        // clone
        abstract public ItemResource clone();
    }
}
