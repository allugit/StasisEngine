using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public enum TerrainLayerType
    {
        Base = 0,
        Noise,
        Outline,
        DecalSpatter
    };

    abstract public class TerrainLayer
    {
        protected TerrainLayerType _type;
        protected LayerProperties _properties;
        public TerrainLayerType type { get { return _type; } }
        public LayerProperties properties { get { return _properties; } set { _properties = value; } }

        // create
        public static TerrainLayer create(TerrainLayerType layerType)
        {
            switch (layerType)
            {
                case TerrainLayerType.Base:
                    return new TerrainBaseLayer();
                case TerrainLayerType.DecalSpatter:
                    return new TerrainDecalSpatterLayer();
                case TerrainLayerType.Noise:
                    return new TerrainNoiseLayer();
                case TerrainLayerType.Outline:
                    return new TerrainOutlineLayer();
            }
            return null;
        }

        // copyFrom -- clones a list
        public static List<TerrainLayer> copyFrom(List<TerrainLayer> list)
        {
            List<TerrainLayer> copy = new List<TerrainLayer>();
            foreach (TerrainLayer layer in list)
                copy.Add(layer.clone());
            return copy;
        }

        // clone
        abstract public TerrainLayer clone();
    }
}
