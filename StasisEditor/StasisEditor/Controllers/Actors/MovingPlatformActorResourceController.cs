using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Controllers.Actors
{
    public class MovingPlatformActorResourceController : ActorResourceController, IBoxSubControllable
    {
        private MovingPlatformActorResource _movingPlatformActor;
        private BoxSubController _boxSubController;

        public MovingPlatformActorResourceController(ILevelController levelController, ActorResource actorResource = null) : base(levelController)
        {
            // Default actor resource
            if (actorResource == null)
                actorResource = new MovingPlatformActorResource(_levelController.getWorldMouse());

            _actor = actorResource;
            _movingPlatformActor = actorResource as MovingPlatformActorResource;

            // Create sub controllers
            _boxSubController = new BoxSubController(this);
        }

        #region Box Actor Interface

        // getPosition
        public Vector2 getPosition()
        {
            return _actor.position;
        }

        // setPosition
        public void setPosition(Vector2 position)
        {
            _actor.position = position;
        }

        // getHalfWidth
        public float getHalfWidth()
        {
            return _movingPlatformActor.boxProperties.halfWidth;
        }

        // getHalfHeight
        public float getHalfHeight()
        {
            return _movingPlatformActor.boxProperties.halfHeight;
        }

        // getAngle
        public float getAngle()
        {
            return _movingPlatformActor.boxProperties.angle;
        }

        // setHalfWidth
        public void setHalfWidth(float value)
        {
            _movingPlatformActor.boxProperties.halfWidth = value;
        }

        // setHalfHeight
        public void setHalfHeight(float value)
        {
            _movingPlatformActor.boxProperties.halfHeight = value;
        }

        // setAngle
        public void setAngle(float value)
        {
            _movingPlatformActor.boxProperties.angle = value;
        }

        #endregion

        #region Actor Resource Controller Methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_boxSubController);
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_boxSubController);
        }

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            // Box hit test
            bool hit = _boxSubController.hitTest(worldMouse);

            // Select appropriate sub control
            if (hit)
                _levelController.selectSubController(_boxSubController);

            return hit;
        }

        // draw
        public override void draw()
        {
            _renderer.drawBox(_actor.position, _movingPlatformActor.boxProperties.halfWidth, _movingPlatformActor.boxProperties.halfHeight, _movingPlatformActor.boxProperties.angle, Color.Blue);
        }

        // clone
        public override ActorResourceController clone()
        {
            return new MovingPlatformActorResourceController(_levelController, _actor.clone());
        }

        #endregion
    }
}
