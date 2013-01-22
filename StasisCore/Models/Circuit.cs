using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Resources;

namespace StasisCore.Models
{
    public class Circuit
    {
        protected string _uid;
        protected List<Gate> _gates;

        public string uid { get { return _uid; } set { _uid = value; } }
        public List<Gate> gates { get { return _gates; } }
        public XElement data
        {
            get
            {
                List<XElement> gatesData = new List<XElement>();
                foreach (Gate gate in _gates)
                    gatesData.Add(gate.data);

                return new XElement("Circuit",
                    new XAttribute("uid", _uid),
                    gatesData);
            }
        }

        // Create new circuit
        public Circuit(string uid)
        {
            _uid = uid;
            _gates = new List<Gate>();
        }

        // Create from xml
        public Circuit(XElement data)
        {
            _uid = data.Attribute("uid").Value;
            _gates = new List<Gate>();

            Dictionary<int, Gate> initialGates = new Dictionary<int, Gate>();

            // Create gates
            foreach (XElement gateData in data.Elements("Gate"))
            {
                int id = int.Parse(gateData.Attribute("id").Value);
                initialGates[id] = new Gate(this, id, gateData.Attribute("type").Value, Loader.loadVector2(gateData.Attribute("position"), Vector2.Zero));
            }

            // Associate gates
            foreach (XElement gateData in data.Elements("Gate"))
            {
                Gate gate = initialGates[int.Parse(gateData.Attribute("id").Value)];

                foreach (XElement outputData in gateData.Elements("Output"))
                {
                    Gate output = initialGates[int.Parse(outputData.Attribute("to").Value)];
                    gate.outputs.Add(output);
                    output.inputs.Add(gate);
                }

                _gates.Add(gate);
            }
        }
    }
}
