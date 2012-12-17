using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorItem
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

        public EditorItem(ItemResource resource)
        {
            _resource = resource;
        }

        // create
        public static EditorItem create(ItemResource resource)
        {
            EditorItem item = null;
            switch (resource.type)
            {
                case ItemType.Blueprint:
                    item = new EditorBlueprint(resource);
                    break;

                case ItemType.BlueprintScrap:
                    item = new EditorBlueprintScrap(resource);
                    break;

                default:
                    item = new EditorItem(resource);
                    break;
            }
            return item;
        }

        // ToString
        public override string ToString()
        {
            return tag;
        }
    }
}
