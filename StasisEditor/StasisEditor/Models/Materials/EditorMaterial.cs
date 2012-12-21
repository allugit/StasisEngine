using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Resources;

namespace StasisEditor.Models
{
    abstract public class EditorMaterial
    {
        private MaterialResource _resource;
        private bool _changed;

        [Browsable(false)]
        public MaterialResource resource { get { return _resource; } }

        [Browsable(false)]
        public bool changed { get { return _changed; } set { _changed = value; } }

        [Browsable(false)]
        public MaterialType type { get { return _resource.type; } }

        [CategoryAttribute("General")]
        [DisplayName("Tag")]
        public string tag { get { return _resource.tag; } set { _resource.tag = value; } }

        public EditorMaterial(MaterialResource resource)
        {
            _resource = resource;
        }

        // create
        public static EditorMaterial create(MaterialResource resource)
        {
            EditorMaterial material = null;
            switch (resource.type)
            {
                case MaterialType.Fluid:
                    material = new EditorFluidMaterial(resource);
                    break;

                case MaterialType.Items:
                    material = new EditorItemMaterial(resource);
                    break;

                case MaterialType.Terrain:
                    material = new EditorTerrainMaterial(resource);
                    break;

                case MaterialType.Trees:
                    material = new EditorTreeMaterial(resource);
                    break;
            }
            return material;
        }

        // ToString
        public override string ToString()
        {
            return tag;
        }
    }
}
