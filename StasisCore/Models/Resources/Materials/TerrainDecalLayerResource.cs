using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class TerrainDecalSpatterLayerResource : TerrainLayerResource
    {
        public TerrainDecalSpatterLayerResource(LayerProperties properties = null, bool enabled = true)
            : base(enabled)
        {
            // Default options
            if (properties == null)
                properties = new DecalProperties();

            _properties = properties;
            _type = TerrainLayerType.DecalSpatter;
        }

        // toXML
        public override XElement toXML()
        {
            XElement element = new XElement("Layer",
                new XAttribute("type", _type),
                new XAttribute("enabled", _enabled));
            return element;
        }

        // Default string
        public override string ToString()
        {
            return "Decale Spatter Layer";
        }

        // clone
        public override TerrainLayerResource clone()
        {
            return new TerrainDecalSpatterLayerResource(_properties.clone());
        }
    }
}
