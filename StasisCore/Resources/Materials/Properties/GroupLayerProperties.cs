using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Resources
{
    public class GroupLayerProperties : LayerProperties
    {
        private TerrainBlendType _blendType;
        public TerrainBlendType blendType { get { return _blendType; } set { _blendType = value; } }

        public GroupLayerProperties(TerrainBlendType blendType)
            : base()
        {
            _blendType = blendType;
        }

        public override LayerProperties clone()
        {
            return new GroupLayerProperties(_blendType);
        }
    }
}
