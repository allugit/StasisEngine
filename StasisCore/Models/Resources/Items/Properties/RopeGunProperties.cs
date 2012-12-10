using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class RopeGunProperties : ItemProperties
    {
        private bool _doubleAnchor;

        public bool doubleAnchor { get { return _doubleAnchor; } set { _doubleAnchor = value; } }

        public RopeGunProperties(bool doubleAnchor)
            : base()
        {
            _doubleAnchor = doubleAnchor;
        }

        // ToString
        public override string ToString()
        {
            return "Rope Gun Properties";
        }

        // clone
        public override ItemProperties clone()
        {
            return new RopeGunProperties(_doubleAnchor);
        }
    }
}
