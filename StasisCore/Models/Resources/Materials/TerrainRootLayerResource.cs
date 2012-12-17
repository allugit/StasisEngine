using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class TerrainRootLayerResource : TerrainGroupLayerResource
    {
        public TerrainRootLayerResource(List<TerrainLayerResource> layers = null, LayerProperties properties = null)
            : base(layers, properties, true)
        {
            // Default properties
            if (properties == null)
                properties = new RootProperties();

            _properties = properties;
            _type = TerrainLayerType.Root;
        }

        // fromXML
        new public static TerrainRootLayerResource fromXML(XElement element)
        {
            List<TerrainLayerResource> layers = new List<TerrainLayerResource>();
            foreach (XElement layerElement in element.Elements("Layer"))
                layers.Add(TerrainLayerResource.fromXML(layerElement));

            return new TerrainRootLayerResource(layers, new RootProperties());
        }

        // toXML
        public override XElement toXML()
        {
            List<XElement> layersXML = new List<XElement>();
            for (int i = 0; i < _layers.Count; i++)
                layersXML.Add(_layers[i].toXML());

            XElement element = new XElement("Layer",
                new XAttribute("type", _type),
                new XAttribute("enabled", _enabled),
                layersXML);

            return element;
        }

        // Default string
        public override string ToString()
        {
            return "Root";
        }

        // clone
        public override TerrainLayerResource clone()
        {
            List<TerrainLayerResource> layersCopy = new List<TerrainLayerResource>();
            foreach (TerrainLayerResource layer in _layers)
                layersCopy.Add(layer.clone());
            return new TerrainRootLayerResource(layersCopy, _properties.clone());
        }
    }
}
