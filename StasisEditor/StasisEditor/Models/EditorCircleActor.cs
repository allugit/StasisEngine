using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorCircleActor : EditorActor
    {
        private CircleProperties _circleProperties;
        private BodyProperties _bodyProperties;

        public CircleProperties circleProperties { get { return _circleProperties; } }
        public BodyProperties bodyProperties { get { return _bodyProperties; } }

        public EditorCircleActor(Vector2 position, ActorProperties circleProperties = null, ActorProperties bodyProperties = null)
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
            _type = ActorType.CircleActor;
        }

        // clone
        public override EditorActor clone()
        {
            return new EditorCircleActor(_position, _circleProperties.clone(), _bodyProperties.clone());
        }
    }
}
