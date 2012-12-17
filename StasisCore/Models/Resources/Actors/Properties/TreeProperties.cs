using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TreeProperties : ActorProperties
    {
        private float _angle;
        private int _seed;
        private float _age;
        private float _internodeLength;
        private int _maxShootLength;
        private float _maxBaseWidth;
        private float _perceptionAngle;
        private float _perceptionRadius;
        private float _occupancyRadius;
        private float _lateralAngle;
        private float _fullExposure;
        private float _penumbraA;
        private float _penumbraB;
        private float _optimalGrowthWeight;
        private float _tropismWeight;
        private Vector2 _tropism;

        public float angle { get { return _angle; } set { _angle = value; } }
        public int seed { get { return _seed; } set { _seed = value; } }
        public float age { get { return _age; } set { _age = value; } }
        public float internodeLength { get { return _internodeLength; } set { _internodeLength = value; } }
        public int maxShootLength { get { return _maxShootLength; } set { _maxShootLength = value; } }
        public float maxBaseWidth { get { return _maxBaseWidth; } set { _maxBaseWidth = value; } }
        public float perceptionAngle { get { return _perceptionAngle; } set { _perceptionAngle = value; } }
        public float perceptionRadius { get { return _perceptionRadius; } set { _perceptionAngle = value; } }
        public float occupancyRadius { get { return _occupancyRadius; } set { _occupancyRadius = value; } }
        public float lateralAngle { get { return _lateralAngle; } set { _lateralAngle = value; } }
        public float fullExposure { get { return _fullExposure; } set { _fullExposure = value; } }
        public float penumbraA { get { return _penumbraA; } set { _penumbraA = value; } }
        public float penumbraB { get { return _penumbraB; } set { _penumbraB = value; } }
        public float optimalGrowthWeight { get { return _optimalGrowthWeight; } set { _optimalGrowthWeight = value; } }
        public float tropismWeight { get { return _tropismWeight; } set { _tropismWeight = value; } }
        public Vector2 tropism { get { return _tropism; } set { _tropism = value; } }

        public TreeProperties(
            Vector2 tropism,
            float angle = (float)(-Math.PI / 2),
            int seed = 1,
            float age = 0f,
            float internodeLength = 1f,
            int maxShootLength = 4,
            float maxBaseWidth = 1f,
            float perceptionAngle = 0.6f,
            float perceptionRadius = 4f,
            float occupancyRadius = 1f,
            float lateralAngle = 0.6f,
            float fullExposure = 1f,
            float penumbraA = 2f,
            float penumbraB = 2f,
            float optimalGrowthWeight = 1f,
            float tropismWeight = 1f)
            : base()
        {
            _angle = angle;
            _seed = seed;
            _age = age;
            _internodeLength = internodeLength;
            _maxShootLength = maxShootLength;
            _maxBaseWidth = maxBaseWidth;
            _perceptionAngle = perceptionAngle;
            _perceptionRadius = perceptionRadius;
            _occupancyRadius = occupancyRadius;
            _lateralAngle = lateralAngle;
            _fullExposure = fullExposure;
            _penumbraA = penumbraA;
            _penumbraB = penumbraB;
            _optimalGrowthWeight = optimalGrowthWeight;
            _tropismWeight = tropismWeight;
            _tropism = tropism;
        }

        // toXML
        public XElement toXML()
        {
            return new XElement("TreeProperties",
                new XAttribute("angle", _angle),
                new XAttribute("seed", _seed),
                new XAttribute("age", _age),
                new XAttribute("internodeLength", _internodeLength),
                new XAttribute("maxShootLength", _maxShootLength),
                new XAttribute("maxBaseWidth", _maxBaseWidth),
                new XAttribute("perceptionAngle", _perceptionAngle),
                new XAttribute("perceptionRadius", _perceptionRadius),
                new XAttribute("occupancyRadius", _occupancyRadius),
                new XAttribute("lateralAngle", _lateralAngle),
                new XAttribute("fullExposure", _fullExposure),
                new XAttribute("penumbraA", _penumbraA),
                new XAttribute("penumbraB", _penumbraB),
                new XAttribute("optimalGrowthWeight", _optimalGrowthWeight),
                new XAttribute("tropismWeight", _tropismWeight),
                new XAttribute("tropism", _tropism));
        }

        // ToString
        public override string ToString()
        {
            return "Tree Properties";
        }

        // clone
        public override ActorProperties clone()
        {
            return new TreeProperties(_tropism, _angle, _seed, _age, _internodeLength, _maxShootLength, _maxBaseWidth, _perceptionAngle, _perceptionRadius, _occupancyRadius, _lateralAngle, _fullExposure, _penumbraA, _penumbraB, _optimalGrowthWeight, _tropismWeight);
        }
    }
}
