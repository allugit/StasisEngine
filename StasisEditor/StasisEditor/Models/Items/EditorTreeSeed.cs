using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Models;
using StasisEditor.Models;
using Microsoft.Xna.Framework;

namespace StasisEditor.Models
{
    public class EditorTreeSeed : EditorItem
    {
        private TreeSeedItemResource _treeSeedItemResource;

        [Browsable(false)]
        public TreeSeedItemResource treeSeedItemResource { get { return _treeSeedItemResource; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Angle")]
        public float angle { get { return _treeSeedItemResource.treeProperties.angle; } set { _treeSeedItemResource.treeProperties.angle = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Seed")]
        public int seed { get { return _treeSeedItemResource.treeProperties.seed; } set { _treeSeedItemResource.treeProperties.seed = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Age")]
        public float age { get { return _treeSeedItemResource.treeProperties.age; } set { _treeSeedItemResource.treeProperties.age = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Internode Length")]
        public float internodeLength { get { return _treeSeedItemResource.treeProperties.internodeLength; } set { _treeSeedItemResource.treeProperties.internodeLength = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Maximum Shoot Length")]
        public int maxShootLength { get { return _treeSeedItemResource.treeProperties.maxShootLength; } set { _treeSeedItemResource.treeProperties.maxShootLength = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Maximum Base Length")]
        public float maxBaseWidth { get { return _treeSeedItemResource.treeProperties.maxBaseWidth; } set { _treeSeedItemResource.treeProperties.maxBaseWidth = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Perception Cone Angle")]
        public float perceptionAngle { get { return _treeSeedItemResource.treeProperties.perceptionAngle; } set { _treeSeedItemResource.treeProperties.perceptionAngle = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Perception Cone Radius")]
        public float perceptionRadius { get { return _treeSeedItemResource.treeProperties.perceptionRadius; } set { _treeSeedItemResource.treeProperties.perceptionAngle = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Occupancy Zone Radius")]
        public float occupancyRadius { get { return _treeSeedItemResource.treeProperties.occupancyRadius; } set { _treeSeedItemResource.treeProperties.occupancyRadius = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Lateral Bud Angle")]
        public float lateralAngle { get { return _treeSeedItemResource.treeProperties.lateralAngle; } set { _treeSeedItemResource.treeProperties.lateralAngle = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Full Exposure")]
        public float fullExposure { get { return _treeSeedItemResource.treeProperties.fullExposure; } set { _treeSeedItemResource.treeProperties.fullExposure = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Penumbra A")]
        public float penumbraA { get { return _treeSeedItemResource.treeProperties.penumbraA; } set { _treeSeedItemResource.treeProperties.penumbraA = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Penumbra B")]
        public float penumbraB { get { return _treeSeedItemResource.treeProperties.penumbraB; } set { _treeSeedItemResource.treeProperties.penumbraB = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Optimal Growth Weight")]
        public float optimalGrowthWeight { get { return _treeSeedItemResource.treeProperties.optimalGrowthWeight; } set { _treeSeedItemResource.treeProperties.optimalGrowthWeight = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Tropism Weight")]
        public float tropismWeight { get { return _treeSeedItemResource.treeProperties.tropismWeight; } set { _treeSeedItemResource.treeProperties.tropismWeight = value; } }

        [CategoryAttribute("Tree Actor Properties")]
        [DisplayName("Tropism")]
        public Vector2 tropism { get { return _treeSeedItemResource.treeProperties.tropism; } set { _treeSeedItemResource.treeProperties.tropism = value; } }

        [CategoryAttribute("General Plant Properties")]
        [DisplayName("Drops Seeds")]
        public bool dropsSeeds { get { return _treeSeedItemResource.generalPlantProperties.dropsSeeds; } set { _treeSeedItemResource.generalPlantProperties.dropsSeeds = value; } }

        [CategoryAttribute("General Plant Properties")]
        [DisplayName("Fruit Frequency")]
        public float fruitFrequency { get { return _treeSeedItemResource.generalPlantProperties.fruitFrequency; } set { _treeSeedItemResource.generalPlantProperties.fruitFrequency = value; } }

        [CategoryAttribute("General Plant Properties")]
        [DisplayName("Fruit Item Tag")]
        public string fruitItemTag { get { return _treeSeedItemResource.generalPlantProperties.fruitItemTag; } set { _treeSeedItemResource.generalPlantProperties.fruitItemTag = value; } }

        public EditorTreeSeed(ItemResource resource)
            : base(resource)
        {
            _treeSeedItemResource = resource as TreeSeedItemResource;
        }
    }
}
