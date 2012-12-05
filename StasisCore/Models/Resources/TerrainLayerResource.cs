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
        protected List<TerrainLayerResource> _layers;
        protected bool _enabled;

        public TerrainLayerType type { get { return _type; } }
        public LayerProperties properties { get { return _properties; } set { _properties = value; } }
        public List<TerrainLayerResource> layers { get { return _layers; } set { _layers = value; } }
        public bool enabled { get { return _enabled; } set { _enabled = value; } }

        // create
        public static TerrainLayerResource create(TerrainLayerType layerType)
        {
            switch (layerType)
            {
                case TerrainLayerType.Texture:
                    return new TerrainTextureLayerResource();
                case TerrainLayerType.Noise:
                    return new TerrainNoiseLayerResource();
                case TerrainLayerType.DecalSpatter:
                    return new TerrainDecalSpatterLayerResource();
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

        // Constructor
        public TerrainLayerResource(List<TerrainLayerResource> layers, bool enabled)
        {
            // Default layers
            _layers = layers == null ? new List<TerrainLayerResource>() : layers;

            _enabled = enabled;
        }

        // clone
        abstract public TerrainLayerResource clone();
    }
}
