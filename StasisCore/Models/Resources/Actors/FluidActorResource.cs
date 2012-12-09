using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class FluidActorResource : ActorResource
    {
        private List<Vector2> _points;
        private FluidProperties _fluidProperties;

        public List<Vector2> points { get { return _points; } }
        public FluidProperties fluidProperties { get { return _fluidProperties; } }

        public FluidActorResource(List<Vector2> points = null, ActorProperties fluidProperties = null)
            : base(Vector2.Zero)
        {
            // Default points
            if (points == null)
                points = new List<Vector2>();

            // Default fluid properties
            if (fluidProperties == null)
                fluidProperties = new FluidProperties(0.001f);

            _points = points;
            _fluidProperties = fluidProperties as FluidProperties;
            _type = ActorType.Fluid;
        }

        // clone
        public override ActorResource clone()
        {
            return new TerrainActorResource(new List<Vector2>(_points), fluidProperties.clone());
        }
    }
}
