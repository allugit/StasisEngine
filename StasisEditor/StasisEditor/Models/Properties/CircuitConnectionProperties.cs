using System;
using System.Collections.Generic;

namespace StasisEditor.Models
{
    public class CircuitConnectionProperties : ActorProperties
    {
        private string _message;

        public string message { get { return _message; } set { _message = value; } }

        public CircuitConnectionProperties(string message)
            : base()
        {
            _message = message;
        }

        // Clone properties
        public override ActorProperties clone()
        {
            return new CircuitConnectionProperties(_message);
        }
    }
}
