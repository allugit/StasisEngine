using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class Gate
    {
        protected int _id;
        protected string _type;
        protected Vector2 _position;
        protected List<Gate> _outputs;
        protected List<Gate> _inputs;
        protected Circuit _circuit;

        public int id { get { return _id; } }
        public string type { get { return _type; } set { _type = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public List<Gate> outputs { get { return _outputs; } set { _outputs = value; } }
        public List<Gate> inputs { get { return _inputs; } set { _inputs = value; } }
        public Circuit circuit { get { return _circuit; } }
        virtual public XElement data
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
            Console.WriteLine("created gate [{0}], type {1}", id, type);
            _circuit = circuit;
            _id = id;
            _type = type;
            _position = position;
            _outputs = new List<Gate>();
            _inputs = new List<Gate>();
        }

        virtual public bool calculateState()
        {
            bool state = false;

            if (type == "and")
            {
                // This is bizarre... the following line of code should work, but ends up not calling _inputs[1].calculateState()
                //state = _inputs[0].calculateState() && _inputs[1].calculateState();
                bool state1 = _inputs[0].calculateState();
                bool state2 = _inputs[1].calculateState();
                state = state1 && state2;
            }
            else if (type == "or")
                state = _inputs[0].calculateState() || _inputs[1].calculateState();
            else if (type == "not")
                state = !_inputs[0].calculateState();

            Console.WriteLine("[{0}] {1}", id, state);
            return state;
        }
    }
}
