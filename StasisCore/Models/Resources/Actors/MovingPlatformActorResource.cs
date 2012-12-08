using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class MovingPlatformActorResource : ActorResource
    {
        private BoxProperties _boxProperties;

        public BoxProperties boxProperties { get { return _boxProperties; } }

        public MovingPlatformActorResource(Vector2 position, ActorProperties boxProperties = null)
            : base(position)
        {
            // Default box properties
            if (boxProperties == null)
                boxProperties = new BoxProperties(1f, 1f, 0f);

            _boxProperties = boxProperties as BoxProperties;
            _type = ActorType.MovingPlatform;
        }

        // clone
        public override ActorResource clone()
        {
            return new MovingPlatformActorResource(_position, _boxProperties.clone());
        }
    }
}
