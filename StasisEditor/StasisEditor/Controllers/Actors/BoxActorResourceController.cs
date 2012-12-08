using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;
using StasisEditor.Views;

namespace StasisEditor.Controllers.Actors
{
    public class BoxActorResourceController : ActorResourceController, IGeneralSubControllable, IBoxSubControllable
    {
        private BoxActorResource _boxActor;
        private GeneralSubController _generalSubController;
        private BoxSubController _boxSubController;

        public BoxActorResourceController(ILevelController levelController, ActorResource actor = null)
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
            _generalSubController = new GeneralSubController(this);
            _boxSubController = new BoxSubController(this);
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
        public override void mouseMove(Vector2 worldMouse)
        {
            _generalSubController.handleMouseMove(worldMouse);
        }

        // mouseEnterView
        public override void mouseEnterView()
        {
        }

        // mouseLeaveView
        public override void mouseLeaveView()
        {
        }

        // mouseDown
        public override void mouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            _generalSubController.handleMouseDown(e);
        }

        // mouseUp
        public override void mouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            _generalSubController.handleMouseUp(e);
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

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_generalSubController);
            _levelController.selectSubController(_boxSubController);
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_generalSubController);
            _levelController.deselectSubController(_boxSubController);
        }

        // draw
        public override void draw()
        {
            _renderer.drawBox(_actor.properties.position, _boxActor.boxProperties.halfWidth, _boxActor.boxProperties.halfHeight, _boxActor.boxProperties.angle);
        }

        // clone
        public override ActorResourceController clone()
        {
            return new BoxActorResourceController(_levelController, _actor.clone());
        }
    }
}
