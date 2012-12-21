using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Resources
{
    public class PlayerSpawnActorResource : ActorResource
    {
        public PlayerSpawnActorResource(Vector2 position)
            : base(position)
        {
            _type = ActorType.PlayerSpawn;
        }

        // clone
        public override ActorResource clone()
        {
            return new PlayerSpawnActorResource(_position);
        }
    }
}
