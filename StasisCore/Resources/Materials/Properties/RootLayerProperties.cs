using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Resources
{
    public class RootLayerProperties : LayerProperties
    {
        public RootLayerProperties()
            : base()
        {
        }

        public override LayerProperties clone()
        {
            return new RootLayerProperties();
        }
    }
}
