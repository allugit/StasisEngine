using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.Components
{
    public class TreeComponent : IComponent
    {
        private Tree _tree;

        public ComponentType componentType { get { return ComponentType.Tree; } }
        public Tree tree { get { return _tree; } }

        public TreeComponent(Tree tree)
        {
            _tree = tree;
        }
    }
}
