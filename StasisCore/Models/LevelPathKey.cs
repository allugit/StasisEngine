using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class LevelPathKey
    {
        private LevelPathDefinition _definition;
        public Vector2 p0;
        public Vector2 p1;
        public Vector2 p2;
        public Vector2 p3;

        public LevelPathDefinition definition { get { return _definition; } }

        public LevelPathKey(LevelPathDefinition definition, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            _definition = definition;
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }
    }
}
