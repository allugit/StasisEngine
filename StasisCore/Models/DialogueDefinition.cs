using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class DialogueDefinition
    {
        private string _uid;
        private Func<bool> _conditionsToMeet;
        private List<DialogueNode> _dialogueNodes;

        public string uid { get { return _uid; } }
        public Func<bool> conditionsToMeet { get { return _conditionsToMeet; } }
        public List<DialogueNode> dialogueNodes { get { return _dialogueNodes; } set { _dialogueNodes = value; } }

        public DialogueDefinition(string uid, Func<bool> conditionsToMeet)
        {
            _uid = uid;
            _conditionsToMeet = conditionsToMeet;
            _dialogueNodes = new List<DialogueNode>();
        }
    }
}
