using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Models;
using StasisCore;
using StasisCore.Resources;

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

        public override Vector2 connectionPosition { get { return _position; } }
        public override List<ActorProperties> properties
        {
            get
            {
                List<ActorProperties> results = new List<ActorProperties>();
                results.Add(_boxProperties);
                results.Add(_bodyProperties);
                return results;
            }
        }
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.Add(_boxProperties.data);
                d.Add(_bodyProperties.data);
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("axis_position", _axisSubController.position);
                return d;
            }
        }

        // Create new
        public PlatformActorController(LevelController levelController)
            : base(levelController, levelController.getUnusedActorID())
        {
            _position = levelController.getWorldMouse();
            _boxProperties = new BoxProperties(0.5f, 0.5f, 0);
            _bodyProperties = new BodyProperties(CoreBodyType.Static, 1f, 1f, 0f);
            _type = ActorType.MovingPlatform;
            initializeControls(_position + new Vector2(1f, 0));
        }

        // Load from xml
        public PlatformActorController(LevelController levelController, XElement data)
            : base(levelController, int.Parse(data.Attribute("id").Value))
        {
            _position = XmlLoadHelper.getVector2(data.Attribute("position").Value);
            _boxProperties = new BoxProperties(data);
            _bodyProperties = new BodyProperties(data);
            _type = ActorType.MovingPlatform;
            initializeControls(XmlLoadHelper.getVector2(data.Attribute("axis_position").Value));
        }

        // Initialize controls
        private void initializeControls(Vector2 axisPosition)
        {
            _boxSubController = new BoxSubController(this);
            _axisSubController = new PointSubController(axisPosition, this);
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
        public override List<ActorSubController> hitTest(Vector2 worldMouse)
        {
            List<ActorSubController> results = new List<ActorSubController>();

            // Axis control point hit test
            if (_axisSubController.hitTest(worldMouse))
            {
                results.Add(_axisSubController);
                return results;
            }

            // Box hit test
            if (_boxSubController.hitTest(worldMouse))
            {
                results.Add(_boxSubController);
                results.Add(_axisSubController);
            }

            return results;
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
