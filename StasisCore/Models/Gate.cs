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
        private List<Gate> _inputs;

        public int id { get { return _id; } }
        public string type { get { return _type; } set { _type = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public List<Gate> outputs { get { return _outputs; } set { _outputs = value; } }
        public List<Gate> inputs { get { return _inputs; } set { _inputs = value; } }

        public Gate(int id, string type, Vector2 position)
        {
            _id = id;
            _type = type;
            _position = position;
            _outputs = new List<Gate>();
            _inputs = new List<Gate>();
        }
    }
}
