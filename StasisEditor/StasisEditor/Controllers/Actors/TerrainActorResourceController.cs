using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Controllers.Actors
{
    public class TerrainActorResourceController : ActorResourceController, ILinkedPointSubControllable
    {
        TerrainActorResource _terrainActorResource;

        public TerrainActorResourceController(ILevelController levelController, ActorResource actorResource = null)
            : base(levelController)
        {
            // Default actor
            if (actorResource == null)
                actorResource = new TerrainActorResource();

            _actor = actorResource;
            _terrainActorResource = actorResource as TerrainActorResource;
        }

        #region Actor Resource Controller Methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
        }

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            return false;
        }

        // draw
        public override void draw()
        {
        }

        // clone
        public override ActorResourceController clone()
        {
            return new TerrainActorResourceController(_levelController, _actor.clone());
        }

        #endregion
    }
}
