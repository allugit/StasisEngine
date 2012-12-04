using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public enum TerrainLayerType
    {
        Texture = 0,
        Noise,
        DecalSpatter
    };

    public enum TerrainBlendType
    {
        Opaque = 0,
        Overlay,
        Additive
    };

    abstract public class TerrainLayerResource
    {
        protected TerrainLayerType _type;
        protected LayerProperties _properties;
        public TerrainLayerType type { get { return _type; } }
        public LayerProperties properties { get { return _properties; } set { _properties = value; } }

        // create
        public static TerrainLayerResource create(TerrainLayerType layerType)
        {
            switch (layerType)
            {
                //case TerrainLayerType.Base:
                //    return new TerrainPrimitivesLayerResource();
                case TerrainLayerType.Texture:
                    return new TerrainTextureLayerResource();
                case TerrainLayerType.Noise:
                    return new TerrainNoiseLayerResource();
                case TerrainLayerType.DecalSpatter:
                    return new TerrainDecalSpatterLayerResource();
                //case TerrainLayerType.Outline:
                //    return new TerrainOutlineLayerResource();
            }
            return null;
        }

        // copyFrom -- clones a list
        public static List<TerrainLayerResource> copyFrom(List<TerrainLayerResource> list)
        {
            List<TerrainLayerResource> copy = new List<TerrainLayerResource>();
            foreach (TerrainLayerResource layer in list)
                copy.Add(layer.clone());
            return copy;
        }

        // clone
        abstract public TerrainLayerResource clone();
    }
}
