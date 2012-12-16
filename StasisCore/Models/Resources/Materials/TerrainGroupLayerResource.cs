using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class TerrainGroupLayerResource : TerrainLayerResource
    {
        protected List<TerrainLayerResource> _layers;
        public List<TerrainLayerResource> layers { get { return _layers; } }

        public TerrainGroupLayerResource(List<TerrainLayerResource> layers = null, LayerProperties properties = null, bool enabled = true)
            : base(enabled)
        {
            // Default layers
            if (layers == null)
                layers = new List<TerrainLayerResource>();

            // Default properties
            if (properties == null)
                properties = new GroupProperties(TerrainBlendType.Opaque);

            _layers = layers;
            _properties = properties;
            _type = TerrainLayerType.Group;
        }

        // fromXML
        new public static TerrainGroupLayerResource fromXML(XElement element)
        {
            List<TerrainLayerResource> layers = new List<TerrainLayerResource>();
            foreach (XElement layerElement in element.Elements("Layer"))
                layers.Add(TerrainLayerResource.fromXML(layerElement));

            return new TerrainGroupLayerResource(
                layers,
                new GroupProperties((TerrainBlendType)Enum.Parse(typeof(TerrainBlendType), element.Attribute("blendType").Value)),
                bool.Parse(element.Attribute("enabled").Value));
        }

        // toXML
        public override XElement toXML()
        {
            GroupProperties properties = _properties as GroupProperties;

            List<XElement> layersXML = new List<XElement>();
            for (int i = 0; i < _layers.Count; i++)
                layersXML.Add(_layers[i].toXML());

            XElement element = new XElement("Layer",
                new XAttribute("type", _type),
                new XAttribute("enabled", _enabled),
                new XAttribute("blendType", properties.blendType),
                layersXML);

            return element;
        }

        // Default string
        public override string ToString()
        {
            return "Group";
        }

        // clone
        public override TerrainLayerResource clone()
        {
            List<TerrainLayerResource> layersCopy = new List<TerrainLayerResource>();
            foreach (TerrainLayerResource layer in _layers)
                layersCopy.Add(layer.clone());
            return new TerrainGroupLayerResource(layersCopy, _properties.clone(), enabled);
        }
    }
}
