using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class DialogueOption
    {
        private DialogueNode _dialogueNode;
        private Func<bool> _conditionsToMeet;
        private string _message;
        private Action _action;

        public DialogueNode dialogueNode { get { return _dialogueNode; } }
        public Func<bool> conditionsToMeet { get { return _conditionsToMeet; } }
        public string message { get { return _message; } }
        public Action action { get { return _action; } }

        public DialogueOption(DialogueNode dialogueNode, Func<bool> conditionsToMeet, string message, Action action)
        {
            _dialogueNode = dialogueNode;
            _conditionsToMeet = conditionsToMeet;
            _message = message;
            _action = action;
        }
    }
}
