using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class DialogueNode
    {
        private List<DialogueOption> _options;
        private string _uid;
        private string _message;

        public string uid { get { return _uid; } }
        public string message { get { return _message; } }
        public List<DialogueOption> options { get { return _options; } }

        public DialogueNode(string uid, string message)
        {
            _uid = uid;
            _message = message;
            _options = new List<DialogueOption>();
        }
    }
}
