using System;
using System.Collections.Generic;
using StasisCore.Models;
using StasisCore;

namespace StasisGame.Components
{
    public class GateOutputComponent : IComponent
    {
        private OutputGate _outputGate;
        private GameEventType _onEnabledEvent;
        private GameEventType _onDisabledEvent;
        private int _entityId;

        public ComponentType componentType { get { return ComponentType.GateOutput; } }
        public OutputGate outputGate { get { return _outputGate; } set { _outputGate = value; } }
        public GameEventType onEnabledEvent { get { return _onEnabledEvent; } set { _onEnabledEvent = value; } }
        public GameEventType onDisabledEvent { get { return _onDisabledEvent; } set { _onDisabledEvent = value; } }
        public int entityId { get { return _entityId; } set { _entityId = value; } }

        public GateOutputComponent()
        {
        }
    }
}
