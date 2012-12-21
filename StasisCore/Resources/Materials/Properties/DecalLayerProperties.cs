using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Resources
{
    public class DecalLayerProperties : LayerProperties
    {
        public DecalLayerProperties() : base()
        {
        }

        public override LayerProperties clone()
        {
            return new DecalLayerProperties();
        }
    }
}
