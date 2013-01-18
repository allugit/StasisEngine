using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisCore.Resources;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    public class TreeActorController : ActorController, IPointSubControllable, IBoxSubControllable
    {
        private Vector2 _position;
        private TreeProperties _treeProperties;
        private BoxSubController _boxController;
        private PointSubController _tropismController;

        public override Vector2 connectionPosition { get { return _position; } }
        public override List<ActorProperties> properties
        {
            get
            {
                List<ActorProperties> results = new List<ActorProperties>();
                results.Add(_treeProperties);
                return results;
            }
        }
        public override XElement data
        {
            get
            {
                _treeProperties.tropism = _tropismController.position - _position;
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                d.Add(_treeProperties.data);
                return d;
            }
        }

        // Create new
        public TreeActorController(LevelController levelController)
            : base(levelController, levelController.getUnusedActorID())
        {
            _position = levelController.getWorldMouse();
            _treeProperties = new TreeProperties(new Vector2(0, -1f));
            _type = ActorType.Tree;
            initializeControls(_position + new Vector2(0f, -1f));
        }

        // Load from xml
        public TreeActorController(LevelController levelController, XElement data)
            : base(levelController, int.Parse(data.Attribute("id").Value))
        {
            _position = XmlLoadHelper.getVector2(data.Attribute("position").Value);
            _treeProperties = new TreeProperties(data);
            _type = ActorType.Tree;
            initializeControls(_treeProperties.tropism);
        }

        // Initialize controls
        private void initializeControls(Vector2 tropism)
        {
            _tropismController = new PointSubController(_position + tropism, this);
            _boxController = new BoxSubController(this, BoxSubControllerAlignment.Edge);
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
            return _treeProperties.maxBaseWidth;
        }

        // getHalfHeight
        public float getHalfHeight()
        {
            return _treeProperties.internodeLength;
        }

        // getAngle
        public float getAngle()
        {
            return _treeProperties.angle;
        }

        // setHalfWidth
        public void setHalfWidth(float value)
        {
            _treeProperties.maxBaseWidth = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setHalfHeight
        public void setHalfHeight(float value)
        {
            _treeProperties.internodeLength = Math.Max(value, LevelController.MIN_ACTOR_SIZE);
        }

        // setAngle
        public void setAngle(float value)
        {
            _treeProperties.angle = value;
        }

        #endregion

        #region Input

        public override List<ActorSubController> hitTest(Vector2 worldMouse)
        {
            List<ActorSubController> results = new List<ActorSubController>();

            if (_tropismController.hitTest(worldMouse))
            {
                // Tropism controller hit
                results.Add(_tropismController);
            }
            else if (_boxController.hitTest(worldMouse))
            {
                // Box controller hit
                results.Add(_boxController);
                results.Add(_tropismController);
            }

            return results;
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
            _levelController.view.drawPoint(_position, Color.DarkGray);

            // Draw box
            _levelController.view.drawBox(_position + _boxController.alignmentOffset, _treeProperties.maxBaseWidth, _treeProperties.internodeLength, _treeProperties.angle, Color.Green);

            // Draw tropism control
            _levelController.view.drawLine(_position, _tropismController.position, Color.Gray);
            _levelController.view.drawPoint(_tropismController.position, Color.Yellow);
        }

        // clone
        public override ActorController clone()
        {
            return new TreeActorController(_levelController);
        }

        #endregion
    }
}
