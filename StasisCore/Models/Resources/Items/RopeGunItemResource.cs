using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class RopeGunItemResource : ItemResource
    {
        private bool _doubleAnchor;
        private float _range;

        public bool doubleAnchor { get { return _doubleAnchor; } set { _doubleAnchor = value; } }
        public float range { get { return _range; } set { _range = value; } }

        public RopeGunItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag, bool doubleAnchor, float range)
            : base(tag, quantity, worldTextureTag, inventoryTextureTag)
        {
            _doubleAnchor = doubleAnchor;
            _range = range;
            _type = ItemType.RopeGun;
        }

        // clone
        public override ItemResource clone()
        {
            return new RopeGunItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _doubleAnchor, _range);
        }
    }
}
