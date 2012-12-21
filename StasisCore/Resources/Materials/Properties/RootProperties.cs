using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class RootProperties : LayerProperties
    {
        public RootProperties()
            : base()
        {
        }

        public override LayerProperties clone()
        {
            return new RootProperties();
        }
    }
}
