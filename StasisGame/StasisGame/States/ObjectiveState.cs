using System;
using System.Collections.Generic;
using StasisCore.Models;

namespace StasisGame.States
{
    public class ObjectiveState
    {
        private ObjectiveDefinition _definition;
        private int _currentValue;

        public ObjectiveDefinition definition { get { return _definition; } }
        public int currentValue { get { return _currentValue; } set { _currentValue = value; } }

        public ObjectiveState(ObjectiveDefinition definition, int currentValue)
        {
            _definition = definition;
            _currentValue = currentValue;
        }
    }
}
