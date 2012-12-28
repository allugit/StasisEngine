using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore.Resources;

namespace StasisEditor.Controllers.Actors
{
    public class PressurePlateActorResourceController : ActorResourceController, IBoxSubControllable, IPointSubControllable
    {
        private PressurePlateActorResource _pressurePlateActor;
        private BoxSubController _boxSubController;
        private PointSubController _axisSubController;

        public PressurePlateActorResourceController(LevelController levelController, ActorResource actorResource = null)
            : base(levelController)
        {
            // Default actor resource
            if (actorResource == null)
                actorResource = new PressurePlateActorResource(_levelController.getWorldMouse());

            _actor = actorResource;
            _pressurePlateActor = actorResource as PressurePlateActorResource;

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
            return _pressurePlateActor.boxProperties.halfWidth;
        }

        // getHalfHeight
        public float getHalfHeight()
        {
            return _pressurePlateActor.boxProperties.halfHeight;
        }

        // getAngle
        public float getAngle()
        {
            return _pressurePlateActor.boxProperties.angle;
        }

        // setHalfWidth
        public void setHalfWidth(float value)
        {
            _pressurePlateActor.boxProperties.halfWidth = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setHalfHeight
        public void setHalfHeight(float value)
        {
            _pressurePlateActor.boxProperties.halfHeight = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setAngle
        public void setAngle(float value)
        {
            _pressurePlateActor.boxProperties.angle = value;
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
            _levelController.view.drawBox(_actor.position, _pressurePlateActor.boxProperties.halfWidth, _pressurePlateActor.boxProperties.halfHeight, _pressurePlateActor.boxProperties.angle, Color.Aqua);

            // Draw axis sub controller
            _levelController.view.drawLine(_actor.position, _axisSubController.position, Color.Gray);
            _levelController.view.drawPoint(_axisSubController.position, Color.White);
        }

        // clone
        public override ActorResourceController clone()
        {
            return new PressurePlateActorResourceController(_levelController, _actor.clone());
        }

        #endregion
    }
}
