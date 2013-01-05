using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    abstract public class EditorActor
    {
        protected ActorType _type;
        protected Vector2 _position;

        public ActorType type { get { return _type; } }
        public Vector2 position { get { return _position; } set { _position = value; } }

        public EditorActor(Vector2 position)
        {
            _position = position;
        }

        // clone
        abstract public EditorActor clone();
    }
}
