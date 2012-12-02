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
        public TerrainLayerType type { get { return _type; } }

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
