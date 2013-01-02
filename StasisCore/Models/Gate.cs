using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class Gate
    {
        private int _id;
        private string _type;
        private List<Gate> _outputs;

        public string type { get { return _type; } set { _type = value; } }
        public List<Gate> outputs { get { return _outputs; } set { _outputs = value; } }

        public Gate(int id, string type)
        {
            _id = id;
            _type = type;
            _outputs = new List<Gate>();
        }
    }
}
