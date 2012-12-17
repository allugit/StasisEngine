using System;
using System.Collections.Generic;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Models
{
    public class EditorTreeSeed : EditorItem
    {
        private TreeSeedItemResource _treeSeedItemResource;

        public TreeSeedItemResource treeSeedItemResource { get { return _treeSeedItemResource; } }

        public EditorTreeSeed(ItemResource resource)
            : base(resource)
        {
            _treeSeedItemResource = resource as TreeSeedItemResource;
        }
    }
}
