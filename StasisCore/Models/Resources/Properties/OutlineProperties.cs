using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class OutlineProperties : LayerProperties
    {
        private Vector2 _normal;

        [CategoryAttribute("General")]
        [DisplayName("Outline Normal")]
        public Vector2 normal
        {
            get { return _normal; }
            set
            {
                Vector2 val = value;
                val.Normalize();
                _normal = val;
            }
        }

        public OutlineProperties(Vector2 normal)
            : base()
        {
            _normal = normal;
        }

        // clone
        override public LayerProperties clone()
        {
            return new OutlineProperties(_normal);
        }
    }
}
