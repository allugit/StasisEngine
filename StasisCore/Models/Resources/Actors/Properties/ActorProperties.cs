using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    abstract public class ActorProperties : IActorProperties
    {
        public ActorProperties()
        {
        }

        // clone
        abstract public ActorProperties clone();
    }
}
