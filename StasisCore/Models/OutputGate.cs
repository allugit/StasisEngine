using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class OutputGate : Gate
    {
        public OutputGate(Circuit circuit, int id, Vector2 position)
            : base(circuit, id, "output", position)
        {
        }
    }
}
