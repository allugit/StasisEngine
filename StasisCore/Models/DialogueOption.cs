using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class DialogueOption
    {
        private Func<bool> _conditionsToMeet;
        private string _message;
        private Action _action;

        public Func<bool> conditionsToMeet { get { return _conditionsToMeet; } }
        public string message { get { return _message; } }
        public Action action { get { return _action; } }

        public DialogueOption(Func<bool> conditionsToMeet, string message, Action action)
        {
            _conditionsToMeet = conditionsToMeet;
            _message = message;
            _action = action;
        }
    }
}
