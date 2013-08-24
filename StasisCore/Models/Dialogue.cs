using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class Dialogue
    {
        private string _uid;
        private string _currentNodeUid;
        private Func<bool> _conditionsToMeet;
        private List<DialogueNode> _dialogueNodes;

        public string uid { get { return _uid; } }
        private string currentNodeUid { get { return _currentNodeUid; } }
        public Func<bool> conditionsToMeet { get { return _conditionsToMeet; } }
        public List<DialogueNode> dialogueNodes { get { return _dialogueNodes; } set { _dialogueNodes = value; } }

        public Dialogue(string uid, string currentNodeUid, Func<bool> conditionsToMeet)
        {
            _uid = uid;
            _currentNodeUid = currentNodeUid;
            _conditionsToMeet = conditionsToMeet;
        }
    }
}
