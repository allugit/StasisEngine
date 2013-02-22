using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class OutputGate : Gate
    {
        private GameEventType _onEnabledEvent;
        private GameEventType _onDisabledEvent;

        public GameEventType onEnabledEvent { get { return _onEnabledEvent; } set { _onEnabledEvent = value; } }
        public GameEventType onDisabledEvent { get { return _onDisabledEvent; } set { _onDisabledEvent = value; } }

        public OutputGate(Circuit circuit, int id, Vector2 position)
            : base(circuit, id, "output", position)
        {
        }
    }
}
