using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorRopeActor : EditorActor
    {
        private Vector2 _pointA;
        private Vector2 _pointB;
        private BodyProperties _bodyProperties;

        public Vector2 pointA { get { return _pointA; } set { _pointA = value; } }
        public Vector2 pointB { get { return _pointB; } set { _pointB = value; } }
        public BodyProperties bodyProperties { get { return _bodyProperties; } }

        public EditorRopeActor(Vector2 pointA, Vector2 pointB, ActorProperties bodyProperties = null)
            : base(Vector2.Zero)
        {
            // Default body properties
            if (bodyProperties == null)
                bodyProperties = new BodyProperties(CoreBodyType.Dynamic, 1f, 1f, 0f);

            _bodyProperties = bodyProperties as BodyProperties;
            _type = ActorType.Rope;
        }
        
        // clone
        public override EditorActor clone()
        {
            return new EditorRopeActor(_pointA, _pointB, bodyProperties.clone());
        }
    }
}
