using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class GrenadeItemResource : ItemResource
    {
        private bool _sticky;
        private float _radius;
        private float _strength;

        public bool sticky { get { return _sticky; } set { _sticky = value; } }
        public float radius { get { return _radius; } set { _radius = value; } }
        public float strength { get { return _strength; } set { _strength = value; } }

        public GrenadeItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag, bool sticky, float radius, float strength)
            : base(tag, quantity, worldTextureTag, inventoryTextureTag)
        {
            _sticky = sticky;
            _radius = radius;
            _strength = strength;
            _type = ItemType.Grenade;
        }

        // clone
        public override ItemResource clone()
        {
            return new GrenadeItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _sticky, _radius, _strength);
        }
    }
}
