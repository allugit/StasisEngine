﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class CircleActorController : ActorController, ICircleSubControllable
    {
        private Vector2 _position;
        private CircleProperties _circleProperties;
        private CircleSubController _circleSubController;

        public override List<ActorProperties> properties
        {
            get
            {
                List<ActorProperties> results = base.properties;
                results.Add(_circleProperties);
                return results;
            }
        }

        // Create new
        public CircleActorController(LevelController levelController)
            : base(levelController)
        {
            // Defaults
            _position = levelController.getWorldMouse();
            _circleProperties = new CircleProperties(0.5f);
            _type = StasisCore.ActorType.Circle;

            // Initialize controls
            initializeControls();
        }

        // Load from xml
        public CircleActorController(LevelController levelController, XElement data)
            : base(levelController)
        {
            // TODO: Initialize circle properties from xml
            // _circleProperties = new CircleProperties(data)
            _circleProperties = new CircleProperties(0.5f);
            _type = StasisCore.ActorType.Circle;

            // Initialize controls
            initializeControls();
        }

        // Initialize controls
        private void initializeControls()
        {
            _circleSubController = new CircleSubController(this);
        }

        #region General Subcontroller interface

        // getPosition
        public Vector2 getPosition() { return _position; }
        
        // setPosition
        public void setPosition(Vector2 position) { _position = position; }

        #endregion

        #region Circle Subcontroller interface

        // getRadius
        public float getRadius()
        {
            return _circleProperties.radius;
        }

        // setRadius
        public void setRadius(float radius)
        {
            _circleProperties.radius = radius;
        }

        #endregion

        #region Input

        // hitTest
        public override bool hitTest(Vector2 worldMouse, bool select = true)
        {
            if (_circleSubController.hitTest(worldMouse))
            {
                // Select appropriate control
                if (select)
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
            _levelController.view.drawCircle(_position, _circleProperties.radius, Color.LightBlue);
        }

        // clone
        public override ActorController clone()
        {
            return new CircleActorController(_levelController);
        }

        #endregion
    }
}
