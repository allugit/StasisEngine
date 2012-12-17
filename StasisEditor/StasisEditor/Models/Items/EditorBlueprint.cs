using System;
using System.Collections.Generic;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorBlueprint : EditorItem
    {
        private BlueprintItemResource _blueprintResource;

        public BlueprintItemResource blueprintResource { get { return _blueprintResource; } }

        public EditorBlueprint(ItemResource resource)
            : base(resource)
        {
            _blueprintResource = resource as BlueprintItemResource;
        }
    }
}
