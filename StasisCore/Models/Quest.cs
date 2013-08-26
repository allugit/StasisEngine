using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class Quest
    {
        private string _uid;
        private string _title;
        private string _description;
        private bool _received;
        private Dictionary<string, Objective> _objectives;

        public string uid { get { return _uid; } set { _uid = value; } }
        public string title { get { return _title; } set { _title = value; } }
        public string description { get { return _description; } set { _description = value; } }
        public bool received { get { return _received; } set { _received = value; } }
        public Dictionary<string, Objective> objectives { get { return _objectives; } set { _objectives = value; } }

        public Quest(string uid, string title, string description)
        {
            _uid = uid;
            _title = title;
            _description = description;
            _objectives = new Dictionary<string, Objective>();
        }
    }
}
