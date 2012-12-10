using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Controllers.Actors
{
    public class TreeActorResourceController : ActorResourceController, ICircleSubControllable, IPointSubControllable
    {
        private TreeActorResource _treeActorResource;

        private CircleSubController _circleSubController;
        private PointSubController _tropismSubController;

        public TreeActorResourceController(LevelController levelController, ActorResource actorResource = null)
            : base(levelController)
        {
            // Default actor resource
            if (actorResource == null)
                actorResource = new TreeActorResource(_levelController.getWorldMouse());

            _actor = actorResource;
            _treeActorResource = actorResource as TreeActorResource;

            // Create sub controllers
            _circleSubController = new CircleSubController(this);
            _tropismSubController = new PointSubController(actorResource.position + new Vector2(0f, -1f), this);
        }


        #region General Subcontroller interface

        // getPosition
        public Vector2 getPosition() { return _actor.position; }

        // setPosition
        public void setPosition(Vector2 position) { _actor.position = position; }

        #endregion

        #region Circle Subcontroller interface

        // getRadius
        public float getRadius()
        {
            return _treeActorResource.treeProperties.maxBaseWidth;
        }

        // setRadius
        public void setRadius(float radius)
        {
            _treeActorResource.treeProperties.maxBaseWidth = radius;
        }

        #endregion

        #region Input

        public override bool hitTest(Vector2 worldMouse)
        {
            // Hit test tropism control
            if (_tropismSubController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_tropismSubController);
                return true;
            }

            // Hit test circle
            if (_circleSubController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_circleSubController);
                _levelController.selectSubController(_tropismSubController);
                return true;
            }

            return false;
        }

        #endregion

        #region Actor Resource Controller methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_circleSubController);
            _levelController.selectSubController(_tropismSubController);
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_circleSubController);
            _levelController.deselectSubController(_tropismSubController);
        }

        // draw
        public override void draw()
        {
            // Draw circle
            _renderer.drawCircle(_actor.position, _treeActorResource.treeProperties.maxBaseWidth, Color.Brown);

            // Draw tropism control
            _renderer.drawLine(_actor.position, _tropismSubController.position, Color.Gray);
            _renderer.drawPoint(_tropismSubController.position, Color.Yellow);
        }

        // clone
        public override ActorResourceController clone()
        {
            return new TreeActorResourceController(_levelController, _actor.clone());
        }

        #endregion
    }
}
