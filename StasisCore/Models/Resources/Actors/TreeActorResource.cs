using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TreeActorResource : ActorResource
    {
        private GeneralPlantProperties _generalPlantProperties;
        private TreeProperties _treeProperties;

        public GeneralPlantProperties generalPlantProperties { get { return _generalPlantProperties; } }
        public TreeProperties treeProperties { get { return _treeProperties; } }

        public TreeActorResource(Vector2 position, ActorProperties generalPlantProperties = null, ActorProperties treeProperties = null)
            : base(position)
        {
            // Default general plant properties
            if (generalPlantProperties == null)
                generalPlantProperties = new GeneralPlantProperties(true, 0, "");

            // Default tree properties
            if (treeProperties == null)
                treeProperties = new TreeProperties(0, 1, 0f, 1f, 4, 1f, 0.6f, 4f, 3f, 0.6f, 1f, 2f, 2f, 1f, 1f, Vector2.Zero);

            _generalPlantProperties = generalPlantProperties as GeneralPlantProperties;
            _treeProperties = treeProperties as TreeProperties;
            _type = ActorType.Tree;
        }

        // clone
        public override ActorResource clone()
        {
            return new TreeActorResource(_position, _generalPlantProperties.clone(), _treeProperties.clone());
        }
    }
}
