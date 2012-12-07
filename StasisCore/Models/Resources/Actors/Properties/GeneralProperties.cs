using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class GeneralProperties : ActorProperties
    {
        protected Vector2 _position;

        public Vector2 position { get { return _position; } set { _position = value; } }

        public GeneralProperties(Vector2 position)
            : base()
        {
            _position = position;
        }

        // clone
        public override ActorProperties clone()
        {
            return new GeneralProperties(_position);
        }
    }
}
