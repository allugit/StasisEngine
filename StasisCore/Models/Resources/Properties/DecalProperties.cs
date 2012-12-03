using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class DecalProperties : LayerProperties
    {
        public DecalProperties() : base()
        {
        }

        public override LayerProperties clone()
        {
            return new DecalProperties();
        }
    }
}
