using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorTerrainActor : EditorActor
    {
        private BodyProperties _bodyProperties;
        private List<Vector2> _points;

        public BodyProperties bodyProperties { get { return _bodyProperties; } }
        public List<Vector2> points { get { return _points; } }

        public EditorTerrainActor(List<Vector2> points = null, ActorProperties bodyProperties = null)
            : base(Vector2.Zero)
        {
            // Default points
            if (points == null)
                points = new List<Vector2>();

            // Default body properties
            if (bodyProperties == null)
                bodyProperties = new BodyProperties(CoreBodyType.Static, 1f, 1f, 0f);

            _points = points;
            _bodyProperties = bodyProperties as BodyProperties;
            _type = ActorType.Terrain;
        }

        // clone
        public override EditorActor clone()
        {
            return new EditorTerrainActor(new List<Vector2>(_points), bodyProperties.clone());
        }
    }
}
