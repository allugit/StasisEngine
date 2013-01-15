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

        // Handle mouse down
        public override void handleMouseDown()
        {
            // Deselect like normal
            base.handleMouseDown();

            // Hit test actor controllers (excluding circuit actor controllers)
            foreach (ActorController actorController in _actorController.getLevelController().getActorControllers())
            {
                if (!(actorController is CircuitActorController))
                {
                    if (actorController.hitTest(_actorController.getLevelController().getWorldMouse()).Count > 0)
                        Console.WriteLine("form connection with: {0}", actorController);
                }
            }
        }
    }
}
