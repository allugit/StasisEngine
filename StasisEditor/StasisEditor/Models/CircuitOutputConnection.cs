using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class CircuitOutputConnection : CircuitConnection
    {
        private GameEventType _onEnabledEvent;
        private GameEventType _onDisabledEvent;

        public GameEventType onEnabledEvent { get { return _onEnabledEvent; } set { _onEnabledEvent = value; } }
        public GameEventType onDisabledEvent { get { return _onDisabledEvent; } set { _onDisabledEvent = value; } }
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("on_enabled_event", _onEnabledEvent);
                d.SetAttributeValue("on_disabled_event", _onDisabledEvent);
                return d;
            }
        }

        public CircuitOutputConnection(EditorCircuitActor circuitActor, EditorActor actor, Gate gate, GameEventType onEnabledEvent, GameEventType onDisabledEvent)
            : base(circuitActor, actor, gate, "output")
        {
            _onEnabledEvent = onEnabledEvent;
            _onDisabledEvent = onDisabledEvent;
        }
    }
}
