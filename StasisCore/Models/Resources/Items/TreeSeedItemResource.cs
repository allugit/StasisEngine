using System;
using System.Collections.Generic;
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

        // clone
        public override ItemResource clone()
        {
            return new TreeSeedItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _treeProperties.clone(), _generalPlantProperties.clone());
        }
    }
}
