using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class DialogueNode
    {
        private DialogueDefinition _definition;
        private List<DialogueOption> _options;
        private string _uid;
        private string _message;

        public DialogueDefinition definition { get { return _definition; } }
        public string uid { get { return _uid; } }
        public string message { get { return _message; } }
        public List<DialogueOption> options { get { return _options; } }

        public DialogueNode(DialogueDefinition definition, string uid, string message)
        {
            _definition = definition;
            _uid = uid;
            _message = message;
            _options = new List<DialogueOption>();
        }
    }
}
