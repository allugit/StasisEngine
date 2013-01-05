using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class BoxActorController : ActorController, IBoxSubControllable
    {
        private EditorBoxActor _boxActor;
        private BoxSubController _boxSubController;

        public BoxActorController(LevelController levelController, EditorActor actor = null)
            : base(levelController)
        {
            // Default actor
            if (actor == null)
                actor = new EditorBoxActor(_levelController.getWorldMouse());

            _actor = actor;

            // Store reference to typed actor resource
            _boxActor = _actor as EditorBoxActor;

            // Create input controls
            _boxSubController = new BoxSubController(this);
        }
        
        #region Box Sub Controller interface

        // getPosition
        public Vector2 getPosition()
        {
            return _boxActor.position;
        }

        // setPosition
        public void setPosition(Vector2 position)
        {
            _boxActor.position = position;
        }

        // getHalfWidth
        public float getHalfWidth()
        {
            return _boxActor.boxProperties.halfWidth;
        }

        // getHalfHeight
        public float getHalfHeight()
        {
            return _boxActor.boxProperties.halfHeight;
        }

        // getAngle
        public float getAngle()
        {
            return _boxActor.boxProperties.angle;
        }

        // setHalfWidth
        public void setHalfWidth(float value)
        {
            _boxActor.boxProperties.halfWidth = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setHalfHeight
        public void setHalfHeight(float value)
        {
            _boxActor.boxProperties.halfHeight = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setAngle
        public void setAngle(float value)
        {
            _boxActor.boxProperties.angle = value;
        }

        #endregion

        #region Input

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

        // globalCheckKeys
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
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_boxSubController);
        }

        // draw
        public override void draw()
        {
            _levelController.view.drawBox(_actor.position, _boxActor.boxProperties.halfWidth, _boxActor.boxProperties.halfHeight, _boxActor.boxProperties.angle, Color.LightBlue);
        }

        // clone
        public override ActorController clone()
        {
            return new BoxActorController(_levelController, _actor.clone());
        }

        #endregion
    }
}
