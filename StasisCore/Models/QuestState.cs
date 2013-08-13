using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class QuestState
    {
        private QuestDefinition _definition;
        private List<ObjectiveState> _objectiveStates;

        public QuestDefinition definition { get { return _definition; } set { _definition = value; } }
        public List<ObjectiveState> objectiveStates { get { return _objectiveStates; } }

        public QuestState(QuestDefinition definition)
        {
            _definition = definition;
            _objectiveStates = new List<ObjectiveState>();
        }
    }
}
