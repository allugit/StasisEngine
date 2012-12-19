using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.ComponentModel;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorBlueprint : EditorItem
    {
        private BlueprintItemResource _blueprintResource;

        [Browsable(false)]
        public BlueprintItemResource blueprintResource { get { return _blueprintResource; } }

        [CategoryAttribute("Blueprint Properties")]
        [DisplayName("Item Tag")]
        public string itemTag { get { return _blueprintResource.itemTag; } set { _blueprintResource.itemTag = value; } }

        public EditorBlueprint(ItemResource resource)
            : base(resource)
        {
            _blueprintResource = resource as BlueprintItemResource;
        }

        // toXML
        public override XElement toXML()
        {
            return _blueprintResource.toXML();
        }
    }
}
