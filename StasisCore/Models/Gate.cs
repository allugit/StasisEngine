using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class Gate
    {
        private int _id;
        private string _type;
        private Vector2 _position;
        private List<Gate> _outputs;

        public string type { get { return _type; } set { _type = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public List<Gate> outputs { get { return _outputs; } set { _outputs = value; } }

        public Gate(int id, string type, Vector2 position)
        {
            _id = id;
            _type = type;
            _position = position;
            _outputs = new List<Gate>();
        }
    }
}
