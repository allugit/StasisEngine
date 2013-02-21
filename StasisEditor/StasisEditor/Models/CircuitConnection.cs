using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor.Models
{
    abstract public class CircuitConnection : IActorComponent
    {
        protected EditorCircuitActor _circuitActor;
        protected EditorActor _actor;
        protected Gate _gate;
        protected string _type;

        [Browsable(false)]
        public EditorActor actor { get { return _actor; } }
        [Browsable(false)]
        public Gate gate { get { return _gate; } }
        [Browsable(false)]
        virtual public XElement data
        {
            get
            {
                return new XElement("CircuitConnection",
                    new XAttribute("actor_id", _actor.id),
                    new XAttribute("gate_id", _gate.id),
                    new XAttribute("type", _type));
            }
        }

        public CircuitConnection(EditorCircuitActor circuitActor, EditorActor actor, Gate gate, string type)
        {
            _circuitActor = circuitActor;
            _actor = actor;
            _gate = gate;
            _type = type;
        }
    }
}
