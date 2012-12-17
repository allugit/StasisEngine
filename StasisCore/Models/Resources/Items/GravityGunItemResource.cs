using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class GravityGunItemResource : ItemResource
    {
        public bool _wellGun;
        private float _range;
        private float _radius;
        private float _strength;

        public bool wellGun { get { return _wellGun; } set { _wellGun = value; } }
        public float range { get { return _range; } set { _range = value; } }
        public float radius { get { return _radius; } set { _radius = value; } }
        public float strength { get { return _strength; } set { _strength = value; } }

        public GravityGunItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag, bool wellGun, float range, float radius, float strength)
            : base(tag, quantity, worldTextureTag, inventoryTextureTag)
        {
            _wellGun = wellGun;
            _range = range;
            _radius = radius;
            _strength = strength;
            _type = ItemType.GravityGun;
        }

        // clone
        public override ItemResource clone()
        {
            return new GravityGunItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _wellGun, _range, _radius, _strength);
        }
    }
}
