using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class CircuitConnection : IActorComponent
    {
        private EditorCircuitActor _circuitActor;
        private EditorActor _actor;
        private Gate _gate;

        [Browsable(false)]
        public EditorActor actor { get { return _actor; } }
        [Browsable(false)]
        public Gate gate { get { return _gate; } }
        [Browsable(false)]
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
