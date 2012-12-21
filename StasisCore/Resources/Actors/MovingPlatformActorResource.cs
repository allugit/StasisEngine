using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Resources
{
    public class MovingPlatformActorResource : ActorResource
    {
        private BoxProperties _boxProperties;
        private BodyProperties _bodyProperties;

        public BoxProperties boxProperties { get { return _boxProperties; } }
        public BodyProperties bodyProperties { get { return _bodyProperties; } }

        public MovingPlatformActorResource(Vector2 position, ActorProperties boxProperties = null, ActorProperties bodyProperties = null)
            : base(position)
        {
            // Default box properties
            if (boxProperties == null)
                boxProperties = new BoxProperties(1f, 1f, 0f);

            // Default body properties
            if (bodyProperties == null)
                bodyProperties = new BodyProperties(CoreBodyType.Dynamic, 1f, 1f, 0f);

            _boxProperties = boxProperties as BoxProperties;
            _bodyProperties = bodyProperties as BodyProperties;
            _type = ActorType.MovingPlatform;
        }

        // clone
        public override ActorResource clone()
        {
            return new MovingPlatformActorResource(_position, _boxProperties.clone(), _bodyProperties.clone());
        }
    }
}
