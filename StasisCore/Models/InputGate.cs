using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class InputGate : Gate
    {
        public InputGate(Circuit circuit, int id, Vector2 position)
            : base(circuit, id, "input", position)
        {

        }
    }
}
