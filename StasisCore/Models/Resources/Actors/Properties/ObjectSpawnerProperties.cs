using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class ObjectSpawnerProperties : ActorProperties
    {
        private ActorType _actorType;
        private int _spawnLimit;

        public ObjectSpawnerProperties(ActorType actorType, int spawnLimit)
            : base()
        {
            _actorType = actorType;
            _spawnLimit = spawnLimit;
        }

        // clone
        public override ActorProperties clone()
        {
            return new ObjectSpawnerProperties(_actorType, _spawnLimit);
        }
    }
}
