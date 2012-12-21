using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Resources
{
    public class TerrainTextureLayerResource : TerrainLayerResource
    {
        public TerrainTextureLayerResource(LayerProperties properties = null, bool enabled = true)
            : base(enabled)
        {
            // Default options
            if (properties == null)
                properties = new TextureLayerProperties(TerrainBlendType.Opaque, 1, 1, "");

            _properties = properties;
            _type = TerrainLayerType.Texture;
        }

        // fromXML
        new public static TerrainTextureLayerResource fromXML(XElement element)
        {
            TerrainTextureLayerResource textureLayer = new TerrainTextureLayerResource(
                new TextureLayerProperties(
                    (TerrainBlendType)Enum.Parse(typeof(TerrainBlendType), element.Attribute("blendType").Value),
                    float.Parse(element.Attribute("scale").Value),
                    float.Parse(element.Attribute("multiplier").Value),
                    element.Attribute("textureTag").Value),
                bool.Parse(element.Attribute("enabled").Value));
            return textureLayer;
        }

        // toXML
        public override XElement toXML()
        {
            TextureLayerProperties properties = _properties as TextureLayerProperties;
            XElement element = new XElement("Layer",
                new XAttribute("type", _type),
                new XAttribute("enabled", _enabled),
                new XAttribute("blendType", properties.blendType),
                new XAttribute("scale", properties.scale),
                new XAttribute("multiplier", properties.multiplier),
                new XAttribute("textureTag", properties.textureTag));
            return element;
        }

        // Default string
        public override string ToString()
        {
            return "Texture Layer";
        }

        // clone
        public override TerrainLayerResource clone()
        {
            return new TerrainTextureLayerResource(properties.clone());
        }
    }
}
