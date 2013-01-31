using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class CircuitConnection
    {
        private EditorCircuitActor _circuitActor;
        private EditorActor _actor;
        private Gate _gate;

        public EditorActor actor { get { return _actor; } }
        public Gate gate { get { return _gate; } }
        public XElement data
        {
            get
            {
                return new XElement("CircuitConnection",
                    new XAttribute("actor_id", _actor.id),
                    new XAttribute("gate_id", _gate.id));
            }
        }

        public CircuitConnection(EditorCircuitActor circuitActor, EditorActor actor, Gate gate)
        {
            _circuitActor = circuitActor;
            _actor = actor;
            _gate = gate;
        }
    }
}
