using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Resources;

namespace StasisEditor.Controllers.Actors
{
    public class TreeActorResourceController : ActorResourceController, IPointSubControllable, IBoxSubControllable
    {
        private TreeActorResource _treeActorResource;

        private BoxSubController _boxController;
        private PointSubController _tropismController;

        public TreeActorResourceController(LevelController levelController, ActorResource actorResource = null)
            : base(levelController)
        {
            // Default actor resource
            if (actorResource == null)
                actorResource = new TreeActorResource(_levelController.getWorldMouse());

            _actor = actorResource;
            _treeActorResource = actorResource as TreeActorResource;

            // Create sub controllers
            _tropismController = new PointSubController(actorResource.position + new Vector2(0f, -1f), this);
            _boxController = new BoxSubController(this, BoxSubControllerAlignment.Edge);
        }

        #region Box Sub Controller interface

        // getPosition
        public Vector2 getPosition()
        {
            return _treeActorResource.position;
        }

        // setPosition
        public void setPosition(Vector2 position)
        {
            _treeActorResource.position = position;
        }

        // getHalfWidth
        public float getHalfWidth()
        {
            return _treeActorResource.treeProperties.maxBaseWidth;
        }

        // getHalfHeight
        public float getHalfHeight()
        {
            return _treeActorResource.treeProperties.internodeLength;
        }

        // getAngle
        public float getAngle()
        {
            return _treeActorResource.treeProperties.angle;
        }

        // setHalfWidth
        public void setHalfWidth(float value)
        {
            _treeActorResource.treeProperties.maxBaseWidth = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setHalfHeight
        public void setHalfHeight(float value)
        {
            _treeActorResource.treeProperties.internodeLength = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setAngle
        public void setAngle(float value)
        {
            _treeActorResource.treeProperties.angle = value;
        }

        #endregion

        #region Input

        public override bool hitTest(Vector2 worldMouse)
        {
            // Hit test tropism control
            if (_tropismController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_tropismController);
                return true;
            }

            // Hit test box
            if (_boxController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_boxController);
                _levelController.selectSubController(_tropismController);
                return true;
            }

            return false;
        }

        #endregion

        #region Actor Resource Controller methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_boxController);
            _levelController.selectSubController(_tropismController);
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_boxController);
            _levelController.deselectSubController(_tropismController);
        }

        // draw
        public override void draw()
        {
            // Draw base circle
            _levelController.view.drawPoint(_actor.position, Color.DarkGray);

            // Draw box
            _levelController.view.drawBox(_actor.position + _boxController.alignmentOffset, _treeActorResource.treeProperties.maxBaseWidth, _treeActorResource.treeProperties.internodeLength, _treeActorResource.treeProperties.angle, Color.Green);

            // Draw tropism control
            _levelController.view.drawLine(_actor.position, _tropismController.position, Color.Gray);
            _levelController.view.drawPoint(_tropismController.position, Color.Yellow);
        }

        // clone
        public override ActorResourceController clone()
        {
            return new TreeActorResourceController(_levelController, _actor.clone());
        }

        #endregion
    }
}
