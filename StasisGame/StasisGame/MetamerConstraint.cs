using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame
{
    public enum ConstraintType
    {
        TRUNK,
        DISTANCE,
        ANGULAR
    };

    public abstract class MetamerConstraint
    {
        public ConstraintType type;
        public MetamerConstraint(ConstraintType type)
        {
            this.type = type;
        }

        public virtual void solve()
        {
        }

        public virtual bool isRelatedTo(List<Metamer> metamers)
        {
            return false;
        }
    }
}
