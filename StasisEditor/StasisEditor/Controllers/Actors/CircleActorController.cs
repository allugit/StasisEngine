using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisCore.Resources;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class CircleActorController : ActorController, ICircleSubControllable
    {
        private Vector2 _position;
        private CircleProperties _circleProperties;
        private BodyProperties _bodyProperties;
        private CircleSubController _circleSubController;

        public override Vector2 connectionPosition { get { return _position; } }
        public override List<ActorProperties> properties
        {
            get
            {
                List<ActorProperties> results = new List<ActorProperties>();
                results.Add(_commonProperties);
                results.Add(_circleProperties);
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
                d.Add(_circleProperties.data);
                d.Add(_bodyProperties.data);
                return d;
            }
        }

        // Create new
        public CircleActorController(LevelController levelController)
            : base(levelController, levelController.getUnusedActorID())
        {
            _position = levelController.getWorldMouse();
            _circleProperties = new CircleProperties(0.5f);
            _bodyProperties = new BodyProperties(CoreBodyType.Static, 1f, 1f, 0f);
            _type = ActorType.Circle;
            initializeControls();
        }

        // Load from xml
        public CircleActorController(LevelController levelController, XElement data)
            : base(levelController, int.Parse(data.Attribute("id").Value))
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _commonProperties = new CommonActorProperties(data);
            _circleProperties = new CircleProperties(data);
            _bodyProperties = new BodyProperties(data);
            _type = ActorType.Circle;
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
        public override List<ActorSubController> hitTest(Vector2 worldMouse)
        {
            List<ActorSubController> results = new List<ActorSubController>();

            if (_circleSubController.hitTest(worldMouse))
                results.Add(_circleSubController);

            return results;
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
