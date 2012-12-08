using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Controllers.Actors
{
    public class TerrainActorResourceController : ActorResourceController, ILinkedPointSubControllable
    {
        private TerrainActorResource _terrainActorResource;
        private LinkedPointSubController _headLinkedPointController;

        public TerrainActorResourceController(ILevelController levelController, ActorResource actorResource = null)
            : base(levelController)
        {
            // Default actor
            if (actorResource == null)
                actorResource = new TerrainActorResource();

            // Set actor resources
            _actor = actorResource;
            _terrainActorResource = actorResource as TerrainActorResource;

            // Initialize points
            List<Vector2> actorResourcePoints = new List<Vector2>();
            if (_terrainActorResource.points.Count == 0)
            {
                actorResourcePoints.Add(_levelController.getWorldMouse() - new Vector2(1f, 0));
                actorResourcePoints.Add(_levelController.getWorldMouse() + new Vector2(1f, 0));
            }
            else
                actorResourcePoints = _terrainActorResource.points;

            // Create linked point controllers
            _headLinkedPointController = new LinkedPointSubController(actorResourcePoints[0], this);
            LinkedPointSubController current = _headLinkedPointController;
            for (int i = 1; i < actorResourcePoints.Count; i++)
            {
                current.next = new LinkedPointSubController(actorResourcePoints[i], this);
                current.next.previous = current;
                current = current.next;
            }
        }

        #region Actor Resource Controller Methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            LinkedPointSubController current = _headLinkedPointController;
            while (current != null)
            {
                _levelController.selectSubController(current);
                current = current.next;
            }
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            LinkedPointSubController current = _headLinkedPointController;
            while (current != null)
            {
                _levelController.deselectSubController(current);
                current = current.next;
            }
        }

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            // Hit test linked point sub controllers
            LinkedPointSubController current = _headLinkedPointController;
            while (current != null)
            {
                if (current.hitTest(worldMouse))
                {
                    _levelController.selectSubController(current);
                    return true;
                }

                current = current.next;
            }

            return false;
        }

        // draw
        public override void draw()
        {
            // Draw links
            LinkedPointSubController current = _headLinkedPointController;
            while (current.next != null)
            {
                _renderer.drawLine(current.position, current.next.position, Color.Orange);
                current = current.next;
            }

            // Draw points
            current = _headLinkedPointController;
            while (current != null)
            {
                _renderer.drawPoint(current.position, Color.Yellow);
                current = current.next;
            }
        }

        // clone
        public override ActorResourceController clone()
        {
            return new TerrainActorResourceController(_levelController, _actor.clone());
        }

        #endregion
    }
}
