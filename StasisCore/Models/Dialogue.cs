using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class Dialogue
    {
        private string _uid;
        private string _initialNodeUid;
        private string _currentNodeUid;
        private Func<bool> _conditionsToMeet;
        private Dictionary<string, DialogueNode> _dialogueNodes;

        public string uid { get { return _uid; } }
        public string initialNodeUid { get { return _initialNodeUid; } }
        public Func<bool> conditionsToMeet { get { return _conditionsToMeet; } }
        public Dictionary<string, DialogueNode> dialogueNodes { get { return _dialogueNodes; } }
        public string currentNodeUid { get { return _currentNodeUid; } set { _currentNodeUid = value; } }

        public Dialogue(string uid, string initialNodeUid, Func<bool> conditionsToMeet)
        {
            _uid = uid;
            _initialNodeUid = initialNodeUid;
            _currentNodeUid = initialNodeUid;
            _conditionsToMeet = conditionsToMeet;
            _dialogueNodes = new Dictionary<string, DialogueNode>();
        }
    }
}
