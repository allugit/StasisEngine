using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    public class TreeActorController : ActorController, IPointSubControllable, IBoxSubControllable
    {
        private EditorTreeActor _treeActor;

        private BoxSubController _boxController;
        private PointSubController _tropismController;

        public TreeActorController(LevelController levelController, EditorActor actor = null)
            : base(levelController)
        {
            // Default actor resource
            if (actor == null)
                actor = new EditorTreeActor(_levelController.getWorldMouse());

            _actor = actor;
            _treeActor = actor as EditorTreeActor;

            // Create sub controllers
            _tropismController = new PointSubController(actor.position + new Vector2(0f, -1f), this);
            _boxController = new BoxSubController(this, BoxSubControllerAlignment.Edge);
        }

        #region Box Sub Controller interface

        // getPosition
        public Vector2 getPosition()
        {
            return _treeActor.position;
        }

        // setPosition
        public void setPosition(Vector2 position)
        {
            _treeActor.position = position;
        }

        // getHalfWidth
        public float getHalfWidth()
        {
            return _treeActor.treeProperties.maxBaseWidth;
        }

        // getHalfHeight
        public float getHalfHeight()
        {
            return _treeActor.treeProperties.internodeLength;
        }

        // getAngle
        public float getAngle()
        {
            return _treeActor.treeProperties.angle;
        }

        // setHalfWidth
        public void setHalfWidth(float value)
        {
            _treeActor.treeProperties.maxBaseWidth = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setHalfHeight
        public void setHalfHeight(float value)
        {
            _treeActor.treeProperties.internodeLength = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setAngle
        public void setAngle(float value)
        {
            _treeActor.treeProperties.angle = value;
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
            _levelController.view.drawBox(_actor.position + _boxController.alignmentOffset, _treeActor.treeProperties.maxBaseWidth, _treeActor.treeProperties.internodeLength, _treeActor.treeProperties.angle, Color.Green);

            // Draw tropism control
            _levelController.view.drawLine(_actor.position, _tropismController.position, Color.Gray);
            _levelController.view.drawPoint(_tropismController.position, Color.Yellow);
        }

        // clone
        public override ActorController clone()
        {
            return new TreeActorController(_levelController, _actor.clone());
        }

        #endregion
    }
}
