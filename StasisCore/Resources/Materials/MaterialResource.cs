using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace StasisCore.Resources
{
    public enum MaterialType
    {
        Terrain = 0,
        Trees,
        Fluid,
        Items
    };

    public abstract class MaterialResource
    {
        protected string _tag;
        protected MaterialType _type;

        public MaterialType type { get { return _type; } }
        public string tag { get { return _tag; } set { _tag = value; } }

        // Constructor
        public MaterialResource(string tag)
        {
            _tag = tag;
        }

        // copyFrom -- clones a list
        public static List<MaterialResource> copyFrom(IList<MaterialResource> list)
        {
            List<MaterialResource> copy = new List<MaterialResource>(list.Count);
            foreach (MaterialResource material in list)
                copy.Add(material.clone());
            return copy;
        }

        // load
        public static MaterialResource load(string filePath)
        {
            MaterialResource resource = null;

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                XElement element = XElement.Load(stream);
                MaterialType type = (MaterialType)Enum.Parse(typeof(MaterialType), element.Attribute("type").Value);

                switch (type)
                {
                    case MaterialType.Fluid:
                        resource = FluidMaterialResource.fromXML(element);
                        break;

                    case MaterialType.Items:
                        resource = ItemMaterialResource.fromXML(element);
                        break;

                    case MaterialType.Terrain:
                        resource = TerrainMaterialResource.fromXML(element);
                        break;

                    case MaterialType.Trees:
                        resource = TreeMaterialResource.fromXML(element);
                        break;
                }
            }

            return resource;
        }

        // toXML
        abstract public XElement toXML();

        // clone
        public abstract MaterialResource clone();
    }
}
