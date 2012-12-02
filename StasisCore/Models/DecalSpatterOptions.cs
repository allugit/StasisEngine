using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class DecalSpatterOptions : LayerProperties
    {
        public DecalSpatterOptions() : base()
        {
        }

        public override LayerProperties clone()
        {
            return new DecalSpatterOptions();
        }
    }
}
