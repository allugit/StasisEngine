using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Controllers.Actors
{
    public class CircleActorResourceController : ActorResourceController, ICircleSubControllable
    {
        private CircleActorResource _circleActorResource;
        private CircleSubController _circleSubController;

        public CircleActorResourceController(ILevelController levelController, ActorResource actor = null)
            : base(levelController)
        {
            // Default actor resource
            if (actor == null)
                actor = new CircleActorResource(_levelController.getWorldMouse());

            _actor = actor;
            _circleActorResource = actor as CircleActorResource;

            // Create subcontrollers
            _circleSubController = new CircleSubController(this);
        }

        #region General Subcontroller interface

        // getPosition
        public Vector2 getPosition() { return _circleActorResource.position; }
        
        // setPosition
        public void setPosition(Vector2 position) { _circleActorResource.position = position; }

        #endregion

        #region Circle Subcontroller interface

        // getRadius
        public float getRadius()
        {
            return _circleActorResource.circleProperties.radius;
        }

        // setRadius
        public void setRadius(float radius)
        {
            _circleActorResource.circleProperties.radius = radius;
        }

        #endregion

        #region Actor Resource Controller methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_circleSubController);
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_circleSubController);
        }

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            if (_circleSubController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_circleSubController);
                return true;
            }

            return false;
        }

        // draw
        public override void draw()
        {
            // Draw circle
            _renderer.drawCircle(_circleActorResource.position, _circleActorResource.circleProperties.radius, Color.LightBlue);
        }

        // clone
        public override ActorResourceController clone()
        {
            return new CircleActorResourceController(_levelController, _actor.clone());
        }

        #endregion
    }
}
