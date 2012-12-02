using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class OutlineOptions
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

        public OutlineOptions(Vector2 normal)
        {
            _normal = normal;
        }

        // clone
        public OutlineOptions clone()
        {
            return new OutlineOptions(_normal);
        }
    }
}
