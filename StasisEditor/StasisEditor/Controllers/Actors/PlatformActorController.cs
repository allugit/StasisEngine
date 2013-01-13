using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class PlatformActorController : ActorController, IBoxSubControllable, IPointSubControllable
    {
        private Vector2 _position;
        private BoxProperties _boxProperties;
        private BodyProperties _bodyProperties;
        private BoxSubController _boxSubController;
        private PointSubController _axisSubController;

        public override List<ActorProperties> properties
        {
            get
            {
                List<ActorProperties> results = base.properties;
                results.Add(_boxProperties);
                results.Add(_bodyProperties);
                return results;
            }
        }

        // Create new
        public PlatformActorController(LevelController levelController)
            : base(levelController)
        {
            // Defaults
            _position = levelController.getWorldMouse();
            _boxProperties = new BoxProperties(0.5f, 0.5f, 0);
            _bodyProperties = new BodyProperties(CoreBodyType.Static, 1f, 1f, 0f);
            _type = StasisCore.ActorType.MovingPlatform;

            // Initialize controls
            initializeControls();
        }

        // Load from xml
        public PlatformActorController(LevelController levelController, XElement data)
            : base(levelController)
        {
            // TODO: Initialize from xml
            // _boxProperties = new BoxProperties(data)...
            _boxProperties = new BoxProperties(0.5f, 0.5f, 0);
            _bodyProperties = new BodyProperties(CoreBodyType.Static, 1f, 1f, 0f);

            // Initialize controls
            initializeControls();
        }

        // Initialize controls
        private void initializeControls()
        {
            _boxSubController = new BoxSubController(this);
            _axisSubController = new PointSubController(_position + new Vector2(1f, 0), this);
        }

        #region Box SubController Interface

        // getPosition
        public Vector2 getPosition()
        {
            return _position;
        }

        // setPosition
        public void setPosition(Vector2 position)
        {
            _position = position;
        }

        // getHalfWidth
        public float getHalfWidth()
        {
            return _boxProperties.halfWidth;
        }

        // getHalfHeight
        public float getHalfHeight()
        {
            return _boxProperties.halfHeight;
        }

        // getAngle
        public float getAngle()
        {
            return _boxProperties.angle;
        }

        // setHalfWidth
        public void setHalfWidth(float value)
        {
            _boxProperties.halfWidth = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setHalfHeight
        public void setHalfHeight(float value)
        {
            _boxProperties.halfHeight = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setAngle
        public void setAngle(float value)
        {
            _boxProperties.angle = value;
        }

        #endregion

        #region Input

        // hitTest
        public override bool hitTest(Vector2 worldMouse, bool select = true)
        {
            // Axis control point hit test
            if (_axisSubController.hitTest(worldMouse))
            {
                // Select appropriate controls
                if (select)
                    _levelController.selectSubController(_axisSubController);
                return true;
            }

            // Box hit test
            if (_boxSubController.hitTest(worldMouse))
            {
                // Select appropriate controls
                if (select)
                {
                    _levelController.selectSubController(_boxSubController);
                    _levelController.selectSubController(_axisSubController);
                }
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
            _levelController.view.drawBox(_position, _boxProperties.halfWidth, _boxProperties.halfHeight, _boxProperties.angle, Color.Blue);

            // Draw axis sub controller
            _levelController.view.drawLine(_position, _axisSubController.position, Color.Gray);
            _levelController.view.drawPoint(_axisSubController.position, Color.White);
        }

        // clone
        public override ActorController clone()
        {
            return new PlatformActorController(_levelController);
        }

        #endregion
    }
}
