using System;
using System.Collections.Generic;
using StasisCore.Models;

namespace StasisGame.States
{
    public class LevelPathState
    {
        private LevelPathDefinition _definition;
        private bool _discovered;

        public LevelPathDefinition definition { get { return _definition; } }
        public bool discovered { get { return _discovered; } set { _discovered = value; } }

        public LevelPathState(LevelPathDefinition definition, bool discovered)
        {
            _definition = definition;
            _discovered = discovered;
        }
    }
}
