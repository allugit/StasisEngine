using System;
using System.Collections.Generic;

namespace StasisGame
{
    public class Goal
    {
        private int _id;
        private bool _required;
        private string _label;

        public int id { get { return _id; } set { _id = value; } }
        public bool required { get { return _required; } }
        public string label { get { return _label; } }

        public Goal(int id, bool required, string label)
        {
            _id = id;
            _required = required;
            _label = label;
        }
    }
}
