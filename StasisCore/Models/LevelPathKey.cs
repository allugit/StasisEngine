using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class LevelPathKey
    {
        private LevelPathDefinition _definition;
        private Vector2 _p0;
        private Vector2 _p1;
        private Vector2 _p2;
        private Vector2 _p3;

        public LevelPathDefinition definition { get { return _definition; } }
        public Vector2 p0 { get { return _p0; } set { _p0 = value; } }
        public Vector2 p1 { get { return _p1; } set { _p1 = value; } }
        public Vector2 p2 { get { return _p2; } set { _p2 = value; } }
        public Vector2 p3 { get { return _p3; } set { _p3 = value; } }

        public LevelPathKey(LevelPathDefinition definition, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            _definition = definition;
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;
            _p3 = p3;
        }
    }
}
