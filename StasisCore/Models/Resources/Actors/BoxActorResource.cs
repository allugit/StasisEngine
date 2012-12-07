using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class BoxActorResource : ActorResource
    {
        private BoxProperties _boxProperties;
        private BodyProperties _bodyProperties;

        public BoxProperties boxProperties { get { return _boxProperties; } }
        public BodyProperties bodyProperties { get { return _bodyProperties; } }

        public BoxActorResource(ActorProperties generalProperties, ActorProperties boxProperties = null, ActorProperties bodyProperties = null)
            : base(generalProperties)
        {
            // Default box properties
            if (boxProperties == null)
                boxProperties = new BoxProperties(1f, 1f, 0f);

            // Default body properties
            if (bodyProperties == null)
                bodyProperties = new BodyProperties(CoreBodyType.Static, 1f, 1f, 0f);

            _generalProperties = generalProperties as GeneralProperties;
            _boxProperties = boxProperties as BoxProperties;
            _bodyProperties = bodyProperties as BodyProperties;
        }

        // clone
        public override ActorResource clone()
        {
            return new BoxActorResource(_generalProperties.clone(), _boxProperties.clone(), _bodyProperties.clone());
        }
    }
}
