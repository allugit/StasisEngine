using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class InputGate : Gate, IEventHandler
    {
        private GameEventType _listenToEvent;
        private bool _inputState;

        public GameEventType listenToEvent { get { return _listenToEvent; } set { _listenToEvent = value; } }

        public InputGate(Circuit circuit, int id, Vector2 position)
            : base(circuit, id, "input", position)
        {

        }

        public void trigger(GameEvent e)
        {
            _inputState = !_inputState;

            _circuit.updateOutput();
        }

        public override bool calculateState()
        {
            Console.WriteLine("[{0}] {1}", id, _inputState);
            return _inputState;
        }
    }
}
