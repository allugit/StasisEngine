using System;
using System.Collections.Generic;
using StasisCore.Models;
using StasisEditor.Views;

namespace StasisEditor.Controllers.Actors
{
    public class BoxController : ActorController
    {
        private BoxActorResource _boxActor;

        public BoxController(ILevelController levelController, ActorResource actor = null)
            : base(levelController)
        {
            // Default actor
            if (actor == null)
                actor = new BoxActorResource();

            _actor = actor;

            // Store reference to typed actor resource
            _boxActor = _actor as BoxActorResource;
        }

        // draw
        public override void draw()
        {
            _renderer.drawBox(_boxActor.boxProperties.halfWidth, _boxActor.boxProperties.halfHeight, _boxActor.boxProperties.angle);
        }

        // clone
        public override ActorController clone()
        {
            return new BoxController(_levelController, _actor.clone());
        }
    }
}
