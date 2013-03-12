using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace StasisGame
{
    public class DistanceMetamerConstraint : MetamerConstraint
    {
        public Metamer metamerA;
        public Metamer metamerB;
        public float distance;
        public float stiffness;
        private float distanceSq;
        public DistanceMetamerConstraint(Metamer metamerA, Metamer metamerB, float distance, float stiffness)
            : base(ConstraintType.DISTANCE)
        {
            this.metamerA = metamerA;
            this.metamerB = metamerB;
            this.distance = distance;
            this.stiffness = stiffness;

            Debug.Assert(!metamerA.isBroken);
            Debug.Assert(!metamerB.isBroken);

            // Square root approximations variables
            distanceSq = distance * distance;

            metamerB.relatedConstraints.Add(this);
        }

        public override void solve()
        {
            // Square root approximated distance solver
            Vector2 delta = metamerB.position - metamerA.position;
            float deltaLengthSq = delta.LengthSquared();
            float massesSq = (metamerA.inverseMass + metamerB.inverseMass);
            massesSq *= massesSq;
            float difference = (deltaLengthSq - distanceSq) / (deltaLengthSq * massesSq);
            metamerA.position += delta * difference * metamerA.inverseMassSq * stiffness;
            metamerB.position -= delta * difference * metamerB.inverseMassSq * stiffness;
        }

        public override bool isRelatedTo(List<Metamer> metamers)
        {
            return metamers.Contains(metamerB);
        }
    }
}
