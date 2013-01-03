using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorCircuit : Circuit
    {
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
