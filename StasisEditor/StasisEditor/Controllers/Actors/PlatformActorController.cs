using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class PlatformActorController : ActorController, IBoxSubControllable, IPointSubControllable
    {
        private EditorPlatformActor _platformActor;
        private BoxSubController _boxSubController;
        private PointSubController _axisSubController;

        public PlatformActorController(LevelController levelController, EditorActor actor = null)
            : base(levelController)
        {
            // Default actor resource
            if (actor == null)
                actor = new EditorPlatformActor(_levelController.getWorldMouse());

            _actor = actor;
            _platformActor = actor as EditorPlatformActor;

            // Create sub controllers
            _boxSubController = new BoxSubController(this);
            _axisSubController = new PointSubController(actor.position + new Vector2(1f, 0), this);
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
            return _platformActor.boxProperties.halfWidth;
        }

        // getHalfHeight
        public float getHalfHeight()
        {
            return _platformActor.boxProperties.halfHeight;
        }

        // getAngle
        public float getAngle()
        {
            return _platformActor.boxProperties.angle;
        }

        // setHalfWidth
        public void setHalfWidth(float value)
        {
            _platformActor.boxProperties.halfWidth = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setHalfHeight
        public void setHalfHeight(float value)
        {
            _platformActor.boxProperties.halfHeight = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setAngle
        public void setAngle(float value)
        {
            _platformActor.boxProperties.angle = value;
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

        // globalKeyDown
        public override void globalKeyDown(Keys key)
        {
            // Delete test
            if (_boxSubController.selected && key == Keys.Delete)
                delete();

            base.globalKeyDown(key);
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
            _levelController.view.drawBox(_actor.position, _platformActor.boxProperties.halfWidth, _platformActor.boxProperties.halfHeight, _platformActor.boxProperties.angle, Color.Blue);

            // Draw axis sub controller
            _levelController.view.drawLine(_actor.position, _axisSubController.position, Color.Gray);
            _levelController.view.drawPoint(_axisSubController.position, Color.White);
        }

        // clone
        public override ActorController clone()
        {
            return new PlatformActorController(_levelController, _actor.clone());
        }

        #endregion
    }
}
