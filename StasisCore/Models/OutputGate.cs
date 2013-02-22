using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class OutputGate : Gate
    {
        private int _entityId;
        private bool _previousState;
        private bool _state;
        private bool _postEvent;

        public bool previousState { get { return _previousState; } set { _previousState = value; } }
        public bool state { get { return _state; } set { _state = value; } }
        public bool postEvent { get { return _postEvent; } set { _postEvent = value; } }
        public int entityId { get { return _entityId; } set { _entityId = value; } }

        public OutputGate(Circuit circuit, int id, Vector2 position)
            : base(circuit, id, "output", position)
        {
        }

        public override bool calculateState()
        {
            bool state = _inputs[0].calculateState();
            Console.WriteLine("[{0}] {1}", id, state);
            return state;
        }
    }
}
