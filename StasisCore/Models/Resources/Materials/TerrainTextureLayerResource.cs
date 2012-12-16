using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class TerrainTextureLayerResource : TerrainLayerResource
    {
        public TerrainTextureLayerResource(LayerProperties properties = null, bool enabled = true)
            : base(enabled)
        {
            // Default options
            if (properties == null)
                properties = new TextureProperties(TerrainBlendType.Opaque, 1, 1, "");

            _properties = properties;
            _type = TerrainLayerType.Texture;
        }

        // toXML
        public override XElement toXML()
        {
            TextureProperties properties = _properties as TextureProperties;
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
