using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame
{
    public class TrunkMetamerConstraint : MetamerConstraint
    {
        public Metamer metamer;
        public Vector2 anchorPoint;
        public float distance;
        public TrunkMetamerConstraint(Metamer metamer, Vector2 anchorPoint, float distance)
            : base(ConstraintType.TRUNK)
        {
            this.metamer = metamer;
            this.anchorPoint = anchorPoint;
            this.distance = distance;
        }

        public override void solve()
        {
            Vector2 relative = metamer.position - anchorPoint;
            float length = relative.Length();
            //Debug.Assert(length != 0);
            //Debug.Assert(!float.IsInfinity(length));
            //Debug.Assert(!float.IsNaN(length));
            float diff = (length - distance) / length;
            metamer.position -= relative * diff;
        }
    }
}
