using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Resources
{
    public class CircleActorResource : ActorResource
    {
        private CircleProperties _circleProperties;
        private BodyProperties _bodyProperties;

        public CircleProperties circleProperties { get { return _circleProperties; } }
        public BodyProperties bodyProperties { get { return _bodyProperties; } }

        public CircleActorResource(Vector2 position, ActorProperties circleProperties = null, ActorProperties bodyProperties = null)
            : base(position)
        {
            // Default circle properties
            if (circleProperties == null)
                circleProperties = new CircleProperties(1f);

            // Default body properties
            if (bodyProperties == null)
                bodyProperties = new BodyProperties(CoreBodyType.Dynamic, 1f, 1f, 0f);

            _circleProperties = circleProperties as CircleProperties;
            _bodyProperties = bodyProperties as BodyProperties;
        }

        // clone
        public override ActorResource clone()
        {
            return new CircleActorResource(_position, _circleProperties.clone(), _bodyProperties.clone());
        }
    }
}
