using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;
using StasisEditor.Views;

namespace StasisEditor.Controllers.Actors
{
    public class BoxActorController : ActorController, IGeneralActorController, IBoxActorController
    {
        private BoxActorResource _boxActor;
        private GeneralActorInputController _generalInputController;
        private BoxActorInputController _boxInputController;

        public BoxActorController(ILevelController levelController, ActorResource actor = null)
            : base(levelController)
        {
            // Default actor
            if (actor == null)
            {
                GeneralProperties generalProperties = new GeneralProperties(_levelController.getWorldMouse());
                actor = new BoxActorResource(generalProperties);
            }

            _actor = actor;

            // Store reference to typed actor resource
            _boxActor = _actor as BoxActorResource;

            // Create input controls
            _generalInputController = new GeneralActorInputController(this);
            _boxInputController = new BoxActorInputController(this);
        }

        #region General Actor Interface

        // getPosition
        public Vector2 getPosition()
        {
            return _boxActor.properties.position;
        }

        // setPosition
        public void setPosition(Vector2 position)
        {
            _boxActor.properties.position = position;
        }

        #endregion

        #region Box Actor Interface

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
            _boxActor.boxProperties.halfWidth = value;
        }

        // setHalfHeight
        public void setHalfHeight(float value)
        {
            _boxActor.boxProperties.halfHeight = value;
        }

        // setAngle
        public void setAngle(float value)
        {
            _boxActor.boxProperties.angle = value;
        }

        #endregion

        #region Input

        // mouseMove
        public override void mouseMove()
        {
            _generalInputController.handleMouseMove();
        }

        // mouseEnterView
        public override void mouseEnterView()
        {
        }

        // mouseLeaveView
        public override void mouseLeaveView()
        {
        }

        // keyDown
        public override void keyDown()
        {
        }

        // keyUp
        public override void keyUp()
        {
        }

        #endregion

        // draw
        public override void draw()
        {
            _renderer.drawBox(_actor.properties.position, _boxActor.boxProperties.halfWidth, _boxActor.boxProperties.halfHeight, _boxActor.boxProperties.angle);
        }

        // clone
        public override ActorController clone()
        {
            return new BoxActorController(_levelController, _actor.clone());
        }
    }
}
