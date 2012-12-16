using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class BlueprintScrapCraftingProperties : ItemProperties
    {
        private Vector2 _position;
        private float _angle;

        public Vector2 position { get { return _position; } set { _position = value; } }
        public float angle { get { return _angle; } set { _angle = value; } }

        public BlueprintScrapCraftingProperties(Vector2 position, float angle)
            : base()
        {
            _position = position;
            _angle = angle;
        }

        // clone
        public override ItemProperties clone()
        {
            return new BlueprintScrapCraftingProperties(_position, _angle);
        }
    }
}
