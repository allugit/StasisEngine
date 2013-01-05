using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class CircleActorController : ActorController, ICircleSubControllable
    {
        private EditorCircleActor _circleActor;
        private CircleSubController _circleSubController;

        public CircleActorController(LevelController levelController, EditorActor actor = null)
            : base(levelController)
        {
            // Default actor resource
            if (actor == null)
                actor = new EditorCircleActor(_levelController.getWorldMouse());

            _actor = actor;
            _circleActor = actor as EditorCircleActor;

            // Create subcontrollers
            _circleSubController = new CircleSubController(this);
        }

        #region General Subcontroller interface

        // getPosition
        public Vector2 getPosition() { return _circleActor.position; }
        
        // setPosition
        public void setPosition(Vector2 position) { _circleActor.position = position; }

        #endregion

        #region Circle Subcontroller interface

        // getRadius
        public float getRadius()
        {
            return _circleActor.circleProperties.radius;
        }

        // setRadius
        public void setRadius(float radius)
        {
            _circleActor.circleProperties.radius = radius;
        }

        #endregion

        #region Input

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            if (_circleSubController.hitTest(worldMouse))
            {
                _levelController.selectSubController(_circleSubController);
                return true;
            }

            return false;
        }

        // globalCheckKeys
        public override void globalKeyDown(Keys key)
        {
            // Delete test
            if (_circleSubController.selected && key == Keys.Delete)
                delete();

            base.globalKeyDown(key);
        }

        #endregion

        #region Actor Resource Controller methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_circleSubController);
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_circleSubController);
        }

        // draw
        public override void draw()
        {
            // Draw circle
            _levelController.view.drawCircle(_circleActor.position, _circleActor.circleProperties.radius, Color.LightBlue);
        }

        // clone
        public override ActorController clone()
        {
            return new CircleActorController(_levelController, _actor.clone());
        }

        #endregion
    }
}
