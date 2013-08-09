using System;
using System.Collections.Generic;
using StasisCore.Models;

namespace StasisGame.States
{
    public class LevelIconState
    {
        private LevelIconDefinition _definition;
        private bool _discovered;
        private bool _finished;

        public LevelIconDefinition definition { get { return _definition; } set { _definition = value; } }
        public bool discovered { get { return _discovered; } set { _discovered = value; } }
        public bool finished { get { return _finished; } set { _finished = value; } }

        public LevelIconState(LevelIconDefinition definition, bool discovered, bool finished)
        {
            _definition = definition;
            _discovered = discovered;
            _finished = finished;
        }
    }
}
