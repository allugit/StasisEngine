using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class BoxActorController : ActorController, IBoxSubControllable
    {
        private Vector2 _position;
        private BoxProperties _boxProperties;
        private BoxSubController _boxSubController;

        // Create new
        public BoxActorController(LevelController levelController)
            : base(levelController)
        {
            // Defaults
            _position = levelController.getWorldMouse();
            _boxProperties = new BoxProperties(0.5f, 0.5f, 0);
            _type = StasisCore.ActorType.Box;

            // Initialize controls
            initializeControls();
        }

        // Load from xml
        public BoxActorController(LevelController levelController, XElement data)
            : base(levelController)
        {
            // TODO: Initialize from xml
            // _boxProperites = new BoxProperties(data)...
            _boxProperties = new BoxProperties(0.5f, 0.5f, 0);
            _type = StasisCore.ActorType.Box;

            // Initialize controls
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
            _levelController.view.drawBox(_position, _boxProperties.halfWidth, _boxProperties.halfHeight, _boxProperties.angle, Color.LightBlue);
        }

        // clone
        public override ActorController clone()
        {
            return new BoxActorController(_levelController);
        }

        #endregion
    }
}
