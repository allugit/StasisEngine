using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorFluidActor : EditorActor
    {
        private List<Vector2> _points;
        private FluidProperties _fluidProperties;

        public List<Vector2> points { get { return _points; } }
        public FluidProperties fluidProperties { get { return _fluidProperties; } }

        public EditorFluidActor(List<Vector2> points = null, ActorProperties fluidProperties = null)
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
        public override EditorActor clone()
        {
            return new EditorTerrainActor(new List<Vector2>(_points), fluidProperties.clone());
        }
    }
}
