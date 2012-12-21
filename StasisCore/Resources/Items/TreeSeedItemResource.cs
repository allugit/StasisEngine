using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TreeSeedItemResource : ItemResource
    {
        private TreeProperties _treeProperties;
        private GeneralPlantProperties _generalPlantProperties;

        public TreeProperties treeProperties { get { return _treeProperties; } }
        public GeneralPlantProperties generalPlantProperties { get { return _generalPlantProperties; } }

        public TreeSeedItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag, ActorProperties treeProperties = null, ActorProperties generalPlantProperties = null)
            : base(tag, quantity, worldTextureTag, inventoryTextureTag)
        {
            // Default tree properties
            if (treeProperties == null)
                treeProperties = new TreeProperties(Vector2.Zero);

            // Default plant properties
            if (generalPlantProperties == null)
                generalPlantProperties = new GeneralPlantProperties(true, 0f, "");

            _treeProperties = treeProperties as TreeProperties;
            _generalPlantProperties = generalPlantProperties as GeneralPlantProperties;
            _type = ItemType.TreeSeed;
        }

        // fromXML
        public static TreeSeedItemResource fromXML(XElement element)
        {
            return new TreeSeedItemResource(
                element.Attribute("tag").Value,
                int.Parse(element.Attribute("quantity").Value),
                element.Attribute("worldTextureTag").Value,
                element.Attribute("inventoryTextureTag").Value,
                TreeProperties.fromXML(element.Element("TreeProperties")),
                GeneralPlantProperties.fromXML(element.Element("PlantProperties")));
        }

        // toXML
        public override XElement toXML()
        {
            return new XElement("Item",
                new XAttribute("type", _type),
                new XAttribute("tag", _tag),
                new XAttribute("quantity", _quantity),
                new XAttribute("worldTextureTag", _worldTextureTag),
                new XAttribute("inventoryTextureTag", _inventoryTextureTag),
                _treeProperties.toXML(),
                _generalPlantProperties.toXML());
        }

        // clone
        public override ItemResource clone()
        {
            return new TreeSeedItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _treeProperties.clone(), _generalPlantProperties.clone());
        }
    }
}
