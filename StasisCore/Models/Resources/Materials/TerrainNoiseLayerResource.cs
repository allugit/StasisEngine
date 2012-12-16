using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TerrainNoiseLayerResource : TerrainLayerResource
    {
        public TerrainNoiseLayerResource(LayerProperties properties = null, bool enabled = true)
            : base(enabled)
        {
            // Default options
            if (properties == null)
            {
                properties = new NoiseProperties(
                    NoiseType.Perlin,
                    TerrainBlendType.Opaque,
                    WorleyFeature.F1,
                    Vector2.Zero,
                    1f,
                    Vector2.Zero,
                    1.1f,
                    0.5f,
                    2f,
                    1f,
                    Color.Black,
                    Color.White,
                    1);
            }

            _properties = properties;
            _type = TerrainLayerType.Noise;
        }

        // fromXML
        new public static TerrainNoiseLayerResource fromXML(XElement element)
        {
            TerrainNoiseLayerResource noiseLayer = new TerrainNoiseLayerResource(
                new NoiseProperties(
                    (NoiseType)Enum.Parse(typeof(NoiseType), element.Attribute("noiseType").Value),
                    (TerrainBlendType)Enum.Parse(typeof(TerrainBlendType), element.Attribute("blendType").Value),
                    (WorleyFeature)Enum.Parse(typeof(WorleyFeature), element.Attribute("worleyFeature").Value),
                    XmlLoadHelper.getVector2(element.Attribute("position").Value),
                    float.Parse(element.Attribute("scale").Value),
                    XmlLoadHelper.getVector2(element.Attribute("fbmOffset").Value),
                    float.Parse(element.Attribute("noiseFrequency").Value),
                    float.Parse(element.Attribute("noiseGain").Value),
                    float.Parse(element.Attribute("noiseLacunarity").Value),
                    float.Parse(element.Attribute("multiplier").Value),
                    XmlLoadHelper.getColor(element.Attribute("colorRangeLow").Value),
                    XmlLoadHelper.getColor(element.Attribute("colorRangeLow").Value),
                    int.Parse(element.Attribute("iterations").Value)),
                bool.Parse(element.Attribute("enabled").Value));
            return noiseLayer;
        }

        // toXML
        public override XElement toXML()
        {
            NoiseProperties properties = _properties as NoiseProperties;
            XElement element = new XElement("Layer",
                new XAttribute("type", _type),
                new XAttribute("enabled", _enabled),
                new XAttribute("noiseType", properties.noiseType),
                new XAttribute("blendType", properties.blendType),
                new XAttribute("worleyFeature", properties.worleyFeature),
                new XAttribute("position", properties.position),
                new XAttribute("scale", properties.scale),
                new XAttribute("fbmOffset", properties.fbmOffset),
                new XAttribute("noiseFrequency", properties.noiseFrequency),
                new XAttribute("noiseGain", properties.noiseGain),
                new XAttribute("noiseLacunarity", properties.noiseLacunarity),
                new XAttribute("multiplier", properties.multiplier),
                new XAttribute("colorRangeLow", properties.colorRangeLow),
                new XAttribute("colorRangeHigh", properties.colorRangeHigh),
                new XAttribute("iterations", properties.iterations));
            return element;
        }

        // Default string
        public override string ToString()
        {
            return "Noise Layer";
        }

        // clone
        public override TerrainLayerResource clone()
        {
            return new TerrainNoiseLayerResource(_properties.clone());
        }
    }
}
