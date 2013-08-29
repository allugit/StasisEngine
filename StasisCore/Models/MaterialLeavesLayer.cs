using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class MaterialLeavesLayer : MaterialScatterLayer
    {
        public MaterialLeavesLayer()
            : base("leaves")
        {
        }

        public MaterialLeavesLayer(XElement data)
            : base(data)
        {
        }

        public override MaterialLayer clone()
        {
            return new MaterialLeavesLayer(data);
        }
    }
}
