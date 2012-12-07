using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public enum ActorType
    {
        BoxActor = 0,
        CircleActor
    }

    abstract public class ActorResource
    {
        protected ActorType _type;

        public ActorType type { get { return _type; } }

        public ActorResource()
        {
        }

        // clone
        abstract public ActorResource clone();
    }
}
