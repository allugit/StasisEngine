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
