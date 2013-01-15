using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorCircuit : Circuit
    {
        public List<Gate> inputGates { get { return new List<Gate>(from Gate gate in _gates where gate.type == "input" select gate); } }
        public List<Gate> outputGates { get { return new List<Gate>(from Gate gate in _gates where gate.type == "output" select gate); } }

        public EditorCircuit(string uid) : base(uid)
        {
        }

        public EditorCircuit(XElement data)
            : base(data)
        {
        }

        public override string ToString()
        {
            return _uid;
        }
    }
}
