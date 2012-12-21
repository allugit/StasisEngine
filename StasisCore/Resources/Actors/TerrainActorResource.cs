using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class TerrainActorResource : ActorResource
    {
        private BodyProperties _bodyProperties;
        private List<Vector2> _points;

        public BodyProperties bodyProperties { get { return _bodyProperties; } }
        public List<Vector2> points { get { return _points; } }

        public TerrainActorResource(List<Vector2> points = null, ActorProperties bodyProperties = null)
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
        public override ActorResource clone()
        {
            return new TerrainActorResource(new List<Vector2>(_points), bodyProperties.clone());
        }
    }
}
