using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore.Resources;

namespace StasisEditor.Controllers.Actors
{
    public class MovingPlatformActorResourceController : ActorResourceController, IBoxSubControllable, IPointSubControllable
    {
        private MovingPlatformActorResource _movingPlatformActor;
        private BoxSubController _boxSubController;
        private PointSubController _axisSubController;

        public MovingPlatformActorResourceController(LevelController levelController, ActorResource actorResource = null)
            : base(levelController)
        {
            // Default actor resource
            if (actorResource == null)
                actorResource = new MovingPlatformActorResource(_levelController.getWorldMouse());

            _actor = actorResource;
            _movingPlatformActor = actorResource as MovingPlatformActorResource;

            // Create sub controllers
            _boxSubController = new BoxSubController(this);
            _axisSubController = new PointSubController(actorResource.position + new Vector2(1f, 0), this);
        }

        #region Box SubController Interface

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
            _movingPlatformActor.boxProperties.halfWidth = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setHalfHeight
        public void setHalfHeight(float value)
        {
            _movingPlatformActor.boxProperties.halfHeight = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setAngle
        public void setAngle(float value)
        {
            _movingPlatformActor.boxProperties.angle = value;
        }

        #endregion

        #region Input

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            // Axis control point hit test
            if (_axisSubController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_axisSubController);
                return true;
            }

            // Box hit test
            if (_boxSubController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_boxSubController);
                _levelController.selectSubController(_axisSubController);
                return true;
            }

            return false;
        }

        // globalCheckKeys
        public override void globalCheckKey()
        {
            // Delete test
            if (_boxSubController.selected && Input.newKey.IsKeyDown(Keys.Delete) && Input.oldKey.IsKeyUp(Keys.Delete))
                delete();
        }

        #endregion

        #region Actor Resource Controller Methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_boxSubController);
            _levelController.selectSubController(_axisSubController);
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_boxSubController);
            _levelController.deselectSubController(_axisSubController);
        }

        // draw
        public override void draw()
        {
            // Draw box
            _renderer.drawBox(_actor.position, _movingPlatformActor.boxProperties.halfWidth, _movingPlatformActor.boxProperties.halfHeight, _movingPlatformActor.boxProperties.angle, Color.Blue);

            // Draw axis sub controller
            _renderer.drawLine(_actor.position, _axisSubController.position, Color.Gray);
            _renderer.drawPoint(_axisSubController.position, Color.White);
        }

        // clone
        public override ActorResourceController clone()
        {
            return new MovingPlatformActorResourceController(_levelController, _actor.clone());
        }

        #endregion
    }
}
