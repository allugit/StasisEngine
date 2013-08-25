using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class DialogueState
    {
        private DialogueDefinition _definition;
        private string _currentNodeUid;

        public DialogueDefinition definition { get { return _definition; } }
        public string currentNodeUid { get { return _currentNodeUid; } set { _currentNodeUid = value; } }

        public DialogueState(DialogueDefinition definition, string currentNodeUid)
        {
            _definition = definition;
            _currentNodeUid = currentNodeUid;
        }
    }
}
