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
        private Circuit _circuit;

        public int id { get { return _id; } }
        public string type { get { return _type; } set { _type = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public List<Gate> outputs { get { return _outputs; } set { _outputs = value; } }
        public List<Gate> inputs { get { return _inputs; } set { _inputs = value; } }
        public Circuit circuit { get { return _circuit; } }
        public XElement data
        {
            get
            {
                List<XElement> outputsData = new List<XElement>();
                foreach (Gate gate in outputs)
                    outputsData.Add(new XElement("Output", new XAttribute("to", gate.id)));

                return new XElement("Gate",
                    new XAttribute("type", _type),
                    new XAttribute("id", _id),
                    new XAttribute("position", _position),
                    outputsData);
            }
        }

        public Gate(Circuit circuit, int id, string type, Vector2 position)
        {
            _circuit = circuit;
            _id = id;
            _type = type;
            _position = position;
            _outputs = new List<Gate>();
            _inputs = new List<Gate>();
        }
    }
}
