using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class ObjectiveDefinition
    {
        private string _uid;
        private QuestDefinition _definition;
        private string _label;
        private int _startingValue;
        private int _endValue;
        private bool _optional;

        public string uid { get { return _uid; } set { _uid = value; } }
        public QuestDefinition definition { get { return _definition; } set { _definition = value; } }
        public string label { get { return _label; } set { _label = value; } }
        public int startingValue { get { return _startingValue; } set { _startingValue = value; } }
        public int endValue { get { return _endValue; } set { _endValue = value; } }
        public bool optional { get { return _optional; } set { _optional = value; } }

        public ObjectiveDefinition(string uid, string label, int startingValue, int endValue, bool optional)
        {
            _uid = uid;
            _label = label;
            _startingValue = startingValue;
            _endValue = endValue;
            _optional = optional;
        }
    }
}
