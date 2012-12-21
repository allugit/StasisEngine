using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class GroupProperties : LayerProperties
    {
        private TerrainBlendType _blendType;
        public TerrainBlendType blendType { get { return _blendType; } set { _blendType = value; } }

        public GroupProperties(TerrainBlendType blendType)
            : base()
        {
            _blendType = blendType;
        }

        public override LayerProperties clone()
        {
            return new GroupProperties(_blendType);
        }
    }
}
