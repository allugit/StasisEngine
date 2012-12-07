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
        protected GeneralProperties _generalProperties;
        protected ActorType _type;

        public ActorType type { get { return _type; } }
        public GeneralProperties properties { get { return _generalProperties; } set { _generalProperties = value; } }

        public ActorResource(ActorProperties generalProperties)
        {
            _generalProperties = generalProperties as GeneralProperties;
        }

        // clone
        abstract public ActorResource clone();
    }
}
