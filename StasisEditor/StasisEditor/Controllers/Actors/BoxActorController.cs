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

    public class BoxActorController : ActorController, IBoxSubControllable
    {
        private Vector2 _position;
        private BoxProperties _boxProperties;
        private BodyProperties _bodyProperties;
        private BoxSubController _boxSubController;

        public override Vector2 connectionPosition { get { return _position; } }
        public override List<ActorProperties> properties
        {
            get
            {
                List<ActorProperties> results = new List<ActorProperties>();
                results.Add(_commonProperties);
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
                d.SetAttributeValue("position", _position);
                d.Add(_boxProperties.data);
                d.Add(_bodyProperties.data);
                return d;
            }
        }

        // Create new
        public BoxActorController(LevelController levelController)
            : base(levelController, levelController.getUnusedActorID())
        {
            _position = levelController.getWorldMouse();
            _commonProperties.depth = 0.1f;
            _boxProperties = new BoxProperties(0.5f, 0.5f, 0);
            _bodyProperties = new BodyProperties(CoreBodyType.Dynamic, 1f, 1f, 0f);
            _type = ActorType.Box;
            initializeControls();
        }

        // Load from xml
        public BoxActorController(LevelController levelController, XElement data)
            : base(levelController, int.Parse(data.Attribute("id").Value))
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _commonProperties = new CommonActorProperties(data);
            _boxProperties = new BoxProperties(data);
            _bodyProperties = new BodyProperties(data);
            _type = ActorType.Box;
            initializeControls();

        }

        // Initialize controls
        private void initializeControls()
        {
            _boxSubController = new BoxSubController(this);
        }
        
        #region Box Sub Controller interface

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

            // Box hit test
            if (_boxSubController.hitTest(worldMouse))
                results.Add(_boxSubController);

            return results;
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
            _levelController.view.drawBox(_position, _boxProperties.halfWidth, _boxProperties.halfHeight, _boxProperties.angle, Color.LightBlue, _commonProperties.depth);
        }

        // clone
        public override ActorController clone()
        {
            return new BoxActorController(_levelController);
        }

        #endregion
    }
}
