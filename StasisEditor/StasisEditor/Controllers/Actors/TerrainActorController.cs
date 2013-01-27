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

    public class TerrainActorController : ActorController, ILinkedPointSubControllable
    {
        private BodyProperties _bodyProperties;
        private LinkedPointSubController _headLinkedPointController;

        public override Vector2 connectionPosition { get { return _headLinkedPointController.position; } }
        public override List<ActorProperties> properties
        {
            get
            {
                List<ActorProperties> results = new List<ActorProperties>();
                results.Add(_commonProperties);
                results.Add(_bodyProperties);
                return results;
            }
        }
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.Add(_bodyProperties.data);
                LinkedPointSubController current = _headLinkedPointController;
                while (current != null)
                {
                    d.Add(new XElement("Point", current.position));
                    current = current.next;
                }
                return d;
            }
        }

        // Create new
        public TerrainActorController(LevelController levelController)
            : base(levelController, levelController.getUnusedActorID())
        {
            // Defaults
            _bodyProperties = new BodyProperties(CoreBodyType.Static, 1f, 1f, 0f);
            _commonProperties.depth = 0.1f;
            _type = ActorType.Terrain;

            // Initialize controls
            List<Vector2> actorResourcePoints = new List<Vector2>();
            actorResourcePoints.Add(_levelController.getWorldMouse() - new Vector2(1f, 0));
            actorResourcePoints.Add(_levelController.getWorldMouse() + new Vector2(1f, 0));
            initializeControls(actorResourcePoints);
        }

        // Load from xml
        public TerrainActorController(LevelController levelController, XElement data)
            : base(levelController, int.Parse(data.Attribute("id").Value))
        {
            _bodyProperties = new BodyProperties(data);
            _type = ActorType.Terrain;
            _commonProperties = new CommonActorProperties(data);
            List<Vector2> actorResourcePoints = new List<Vector2>();
            foreach (XElement pointData in data.Elements("Point"))
                actorResourcePoints.Add(Loader.loadVector2(pointData, Vector2.Zero));
            initializeControls(actorResourcePoints);
        }

        // Initialize controls
        private void initializeControls(List<Vector2> points)
        {
            // Create linked point controllers
            _headLinkedPointController = new LinkedPointSubController(points[0], this);
            LinkedPointSubController current = _headLinkedPointController;
            for (int i = 1; i < points.Count; i++)
            {
                current.next = new LinkedPointSubController(points[i], this);
                current.next.previous = current;
                current = current.next;
            }
        }

        #region Linked Point Sub Controller Interface

        // setNewLinkedPointSubControllerHead
        public void setNewLinkedPointSubControllerHead(LinkedPointSubController newHead)
        {
            _headLinkedPointController = newHead;
        }

        #endregion

        #region Input

        // hitTest
        public override List<ActorSubController> hitTest(Vector2 worldMouse)
        {
            List<ActorSubController> results = new List<ActorSubController>();

            // Hit test linked point sub controllers
            LinkedPointSubController current = _headLinkedPointController;
            while (current != null)
            {
                if (current.hitTest(worldMouse))
                    results.Add(current);

                current = current.next;
            }

            // Test links if no results found
            if (results.Count == 0)
            {
                // Hit test link lines
                current = _headLinkedPointController;
                while (current.next != null)
                {
                    if (current.linkHitTest(worldMouse))
                    {
                        results.Add(current);
                        results.Add(current.next);
                        break;
                    }

                    current = current.next;
                }
            }

            return results;
        }

        // globalKeyDown
        public override void globalKeyDown(Keys key)
        {
            ////////////////////////////
            // Test for delete
            LinkedPointSubController current = _headLinkedPointController;
            bool anyLinkSelected = false;
            while (current != null)
            {
                if (current.selected)
                {
                    anyLinkSelected = true;
                    break;
                }
                current = current.next;
            }
            if (anyLinkSelected && key == Keys.Delete)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this controller and all the points? \n (Use the minus key [-] to remove single points)", "Remove entire controller?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    delete();
            }

            //////////////////////////////////////
            // Test for point insertion / removal
            if (!anyLinkSelected)
            {
                bool plusPressed = key == Keys.Oemplus;
                bool minusPressed = key == Keys.OemMinus;
                Vector2 worldMouse = _levelController.getWorldMouse();

                // Only test for insertions if there are no links selected -- insertion while selected is handled in the sub controller's
                // checkXNAKeys method since this methods requires a hit test
                if (plusPressed)
                {
                    // Hit test link line
                    current = _headLinkedPointController;
                    while (current.next != null)
                    {
                        if (current.linkHitTest(worldMouse))
                        {
                            current.insertPoint(worldMouse);
                            return;
                        }
                        current = current.next;
                    }
                }

                if (minusPressed)
                {
                    // Hit test points
                    current = _headLinkedPointController;
                    while (current != null)
                    {
                        if (current.hitTest(worldMouse))
                        {
                            current.removePoint();
                            return;
                        }

                        current = current.next;
                    }
                }
            }

            base.globalKeyDown(key);
        }

        #endregion

        #region Actor Resource Controller Methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            LinkedPointSubController current = _headLinkedPointController;
            while (current != null)
            {
                _levelController.selectSubController(current);
                current = current.next;
            }
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            LinkedPointSubController current = _headLinkedPointController;
            while (current != null)
            {
                _levelController.deselectSubController(current);
                current = current.next;
            }
        }

        // draw
        public override void draw()
        {
            // Draw links
            LinkedPointSubController current = _headLinkedPointController;
            while (current.next != null)
            {
                _levelController.view.drawLine(current.position, current.next.position, Color.Orange, _commonProperties.depth);
                current = current.next;
            }

            // Draw points
            current = _headLinkedPointController;
            while (current != null)
            {
                _levelController.view.drawPoint(current.position, Color.Yellow, _commonProperties.depth);
                current = current.next;
            }
        }

        // clone
        public override ActorController clone()
        {
            return new TerrainActorController(_levelController);
        }

        #endregion

    }
}
