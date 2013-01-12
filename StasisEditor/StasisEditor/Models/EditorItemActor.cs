using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorItemActor : EditorActor
    {
        public EditorItemActor(Vector2 position)
            : base(position)
        {
            _type = ActorType.Item;
        }

        public override EditorActor clone()
        {
            return new EditorItemActor(_position);
        }
    }
}
