using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class QuestDefinition
    {
        private string _uid;
        private string _title;
        private string _description;
        private List<ObjectiveDefinition> _objectiveDefinitions;

        public string uid { get { return _uid; } set { _uid = value; } }
        public string title { get { return _title; } set { _title = value; } }
        public string description { get { return _description; } set { _description = value; } }
        public List<ObjectiveDefinition> objectiveDefinitions { get { return _objectiveDefinitions; } set { _objectiveDefinitions = value; } }

        public QuestDefinition(string uid, string title, string description)
        {
            _uid = uid;
            _title = title;
            _description = description;
            _objectiveDefinitions = new List<ObjectiveDefinition>();
        }
    }
}
