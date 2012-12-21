using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Resources;

namespace StasisEditor.Controllers.Actors
{
    public class ObjectSpawnerResourceController : ActorResourceController, ICircleSubControllable, IPointSubControllable
    {
        private ObjectSpawnerResource _objectSpawnerResource;
        private CircleSubController _circleSubController;
        private PointSubController _velocitySubController;

        public ObjectSpawnerResourceController(LevelController levelController, ActorResource actor = null)
            : base(levelController)
        {
            // Default actor resource
            if (actor == null)
                actor = new ObjectSpawnerResource(_levelController.getWorldMouse());

            _actor = actor;
            _objectSpawnerResource = actor as ObjectSpawnerResource;

            // Create subcontrollers
            _circleSubController = new CircleSubController(this);
            _velocitySubController = new PointSubController(_levelController.getWorldMouse(), this);
        }

        #region General Subcontroller interface

        // getPosition
        public Vector2 getPosition() { return _objectSpawnerResource.position; }
        
        // setPosition
        public void setPosition(Vector2 position) { _objectSpawnerResource.position = position; }

        #endregion

        #region Circle Subcontroller interface

        // getRadius
        public float getRadius()
        {
            return 0.5f;
        }

        // setRadius
        public void setRadius(float radius)
        {
            return;
        }

        #endregion

        #region Actor Resource Controller methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_circleSubController);
            _levelController.selectSubController(_velocitySubController);
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_circleSubController);
            _levelController.deselectSubController(_velocitySubController);
        }

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            // Hit test velocity control point
            if (_velocitySubController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_velocitySubController);
                return true;
            }

            // Hit test circle
            if (_circleSubController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_circleSubController);
                _levelController.selectSubController(_velocitySubController);
                return true;
            }

            return false;
        }

        // draw
        public override void draw()
        {
            // Draw circle
            _renderer.drawCircle(_objectSpawnerResource.position, 0.5f, Color.LightGreen);

            // Draw velocity control
            _renderer.drawLine(_actor.position, _velocitySubController.position, Color.Gray);
            _renderer.drawPoint(_velocitySubController.position, Color.White);
        }

        // clone
        public override ActorResourceController clone()
        {
            return new CircleActorResourceController(_levelController, _actor.clone());
        }

        #endregion
    }
}
