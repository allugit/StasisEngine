using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class TerrainMaterialResource : MaterialResource
    {
        private TerrainRootLayerResource _rootLayer;

        public TerrainRootLayerResource rootLayer { get { return _rootLayer; } }

        // Constructor
        public TerrainMaterialResource(string tag, TerrainRootLayerResource rootLayer = null) : base(tag)
        {
            // Default root layer
            if (rootLayer == null)
                rootLayer = new TerrainRootLayerResource();

            _rootLayer = rootLayer;
            _type = MaterialType.Terrain;
        }

        // fromXML
        public static TerrainMaterialResource fromXML(XElement element)
        {
            TerrainRootLayerResource rootLayer = TerrainRootLayerResource.fromXML(element.Element("Layer"));
            return new TerrainMaterialResource(
                element.Attribute("tag").Value,
                rootLayer);
        }

        // toXML
        public override XElement toXML()
        {
            XElement element = new XElement("Material",
                new XAttribute("type", _type),
                new XAttribute("tag", _tag),
                _rootLayer.toXML());

            return element;
        }

        // clone
        public override MaterialResource clone()
        {
            return new TerrainMaterialResource(_tag, _rootLayer.clone() as TerrainRootLayerResource);
        }
    }
}
