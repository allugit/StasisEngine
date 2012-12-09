using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public enum ActorType
    {
        BoxActor = 0,
        CircleActor,
        MovingPlatform,
        PressurePlate,
        Terrain,
        Rope,
        Fluid,
        PlayerSpawn
    }

    abstract public class ActorResource
    {
        protected ActorType _type;
        protected Vector2 _position;

        public ActorType type { get { return _type; } }
        public Vector2 position { get { return _position; } set { _position = value; } }

        public ActorResource(Vector2 position)
        {
            _position = position;
        }

        // clone
        abstract public ActorResource clone();
    }
}
