using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class CircuitInputConnection : CircuitConnection
    {
        private GameEventType _listenToEvent;

        public GameEventType listenToEvent { get { return _listenToEvent; } set { _listenToEvent = value; } }
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("listen_to_event", _listenToEvent);
                return d;
            }
        }

        public CircuitInputConnection(EditorCircuitActor circuitActor, EditorActor actor, Gate gate, GameEventType listenToEvent)
            : base(circuitActor, actor, gate, "input")
        {
            _listenToEvent = listenToEvent;
        }
    }
}
