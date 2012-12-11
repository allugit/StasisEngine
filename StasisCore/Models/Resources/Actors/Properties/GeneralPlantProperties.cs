using System;
using System.Collections.Generic;

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
