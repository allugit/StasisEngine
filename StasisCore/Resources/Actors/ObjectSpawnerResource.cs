using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Resources
{
    public class ObjectSpawnerResource : ActorResource
    {
        private ObjectSpawnerProperties _spawnerProperties;
        private ActorProperties _spawnedActorProperties;

        public ObjectSpawnerProperties spawnerProperties { get { return _spawnerProperties; } }
        public ActorProperties spawnedActorProperties { get { return _spawnedActorProperties; } }

        public ObjectSpawnerResource(Vector2 position, ActorProperties spawnerProperties = null, ActorProperties spawnedActorProperties = null)
            : base(position)
        {
            // Default spawner properties
            if (spawnerProperties == null)
                spawnerProperties = new ObjectSpawnerProperties(ActorType.CircleActor, 0);

            // Default spawned actor properties
            if (spawnedActorProperties == null)
                spawnedActorProperties = new CircleProperties(1f);

            _spawnerProperties = spawnerProperties as ObjectSpawnerProperties;
            _spawnedActorProperties = spawnedActorProperties;
        }

        // clone
        public override ActorResource clone()
        {
            return new ObjectSpawnerResource(_position, _spawnerProperties.clone(), _spawnedActorProperties.clone());
        }
    }
}
