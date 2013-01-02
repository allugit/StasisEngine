using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class Circuit
    {
        private string _uid;
        private List<Gate> _gates;

        public string uid { get { return _uid; } set { _uid = value; } }
        public List<Gate> gates { get { return _gates; } }

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
                initialGates[id] = new Gate(id, gateData.Attribute("type").Value);
            }

            // Associate gates
            foreach (XElement gateData in data.Elements("Gate"))
            {
                Gate gate = initialGates[int.Parse(gateData.Attribute("id").Value)];
                
                // Outputs
                foreach (XElement outputData in gateData.Elements("Output"))
                    gate.outputs.Add(initialGates[int.Parse(outputData.Attribute("to").Value)]);
            }
        }
    }
}
