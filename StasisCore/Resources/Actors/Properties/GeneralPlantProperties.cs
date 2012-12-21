using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public enum PlantType
    {
        Tree = 0
    };

    public class GeneralPlantProperties : ActorProperties
    {
        private bool _dropsSeeds;
        private float _fruitFrequency;
        private string _fruitItemTag;

        public bool dropsSeeds { get { return _dropsSeeds; } set { _dropsSeeds = value; } }
        public float fruitFrequency { get { return _fruitFrequency; } set { _fruitFrequency = value; } }
        public string fruitItemTag { get { return _fruitItemTag; } set { _fruitItemTag = value; } }

        public GeneralPlantProperties(bool dropsSeeds, float fruitFrequency, string fruitItemTag)
            : base()
        {
            _dropsSeeds = dropsSeeds;
            _fruitFrequency = fruitFrequency;
            _fruitItemTag = fruitItemTag;
        }

        // fromXML
        public static GeneralPlantProperties fromXML(XElement element)
        {
            return new GeneralPlantProperties(
                bool.Parse(element.Attribute("dropsSeeds").Value),
                float.Parse(element.Attribute("fruitFrequency").Value),
                element.Attribute("fruitItemTag").Value);
        }

        // toXML
        public XElement toXML()
        {
            return new XElement("PlantProperties",
                new XAttribute("dropsSeeds", _dropsSeeds),
                new XAttribute("fruitFrequency", _fruitFrequency),
                new XAttribute("fruitItemTag", _fruitItemTag));
        }

        // ToString
        public override string ToString()
        {
            return "General Plant Properties";
        }

        // clone
        public override ActorProperties clone()
        {
            return new GeneralPlantProperties(_dropsSeeds, _fruitFrequency, _fruitItemTag);
        }
    }
}
