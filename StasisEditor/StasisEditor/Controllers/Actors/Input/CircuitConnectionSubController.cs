using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Controllers.Actors
{
    public class CircuitConnectionSubController : PointSubController
    {
        private Gate _gate;

        public CircuitConnectionSubController(Vector2 position, IPointSubControllable actorController, Gate gate)
            : base(position, actorController)
        {
            _gate = gate;
        }
    }
}
