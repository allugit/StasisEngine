using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorPlayerSpawnActor : EditorActor
    {
        public EditorPlayerSpawnActor(Vector2 position)
            : base(position)
        {
            _type = ActorType.PlayerSpawn;
        }

        // clone
        public override EditorActor clone()
        {
            return new EditorPlayerSpawnActor(_position);
        }
    }
}
