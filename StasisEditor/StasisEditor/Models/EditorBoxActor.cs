using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorBoxActor : EditorActor
    {
        private BoxProperties _boxProperties;
        private BodyProperties _bodyProperties;

        public BoxProperties boxProperties { get { return _boxProperties; } }
        public BodyProperties bodyProperties { get { return _bodyProperties; } }

        public EditorBoxActor(Vector2 position, ActorProperties boxProperties = null, ActorProperties bodyProperties = null)
            : base(position)
        {
            // Default box properties
            if (boxProperties == null)
                boxProperties = new BoxProperties(1f, 1f, 0f);

            // Default body properties
            if (bodyProperties == null)
                bodyProperties = new BodyProperties(CoreBodyType.Static, 1f, 1f, 0f);

            _boxProperties = boxProperties as BoxProperties;
            _bodyProperties = bodyProperties as BodyProperties;
            _type = ActorType.Box;
        }

        // clone
        public override EditorActor clone()
        {
            return new EditorBoxActor(_position, _boxProperties.clone(), _bodyProperties.clone());
        }
    }
}
