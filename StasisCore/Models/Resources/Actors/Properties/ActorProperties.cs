using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
