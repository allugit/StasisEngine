using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorTreeActor : EditorActor
    {
        private GeneralPlantProperties _generalPlantProperties;
        private TreeProperties _treeProperties;

        public GeneralPlantProperties generalPlantProperties { get { return _generalPlantProperties; } }
        public TreeProperties treeProperties { get { return _treeProperties; } }

        public EditorTreeActor(Vector2 position, ActorProperties generalPlantProperties = null, ActorProperties treeProperties = null)
            : base(position)
        {
            // Default general plant properties
            if (generalPlantProperties == null)
                generalPlantProperties = new GeneralPlantProperties(true, 0, "");

            // Default tree properties
            if (treeProperties == null)
                treeProperties = new TreeProperties(Vector2.Zero);

            _generalPlantProperties = generalPlantProperties as GeneralPlantProperties;
            _treeProperties = treeProperties as TreeProperties;
            _type = ActorType.Tree;
        }

        // clone
        public override EditorActor clone()
        {
            return new EditorTreeActor(_position, _generalPlantProperties.clone(), _treeProperties.clone());
        }
    }
}
