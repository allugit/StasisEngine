﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class FluidActorController : ActorController, ILinkedPointSubControllable
    {
        private LinkedPointSubController _headLinkedPointController;

        // Create new
        public FluidActorController(LevelController levelController)
            : base(levelController)
        {
            _type = StasisCore.ActorType.Fluid;

            // Initialize controls
            List<Vector2> actorResourcePoints = new List<Vector2>();
            actorResourcePoints.Add(_levelController.getWorldMouse() - new Vector2(1f, 0));
            actorResourcePoints.Add(_levelController.getWorldMouse() + new Vector2(1f, 0));
            initializeControls(actorResourcePoints);
        }

        // Load from xml
        public FluidActorController(LevelController levelController, XElement data)
            : base(levelController)
        {
            _type = StasisCore.ActorType.Fluid;

            // TODO: Initialize points from xml
            // initializePoints(...);
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
        public override bool hitTest(Vector2 worldMouse)
        {
            // Hit test linked point sub controllers
            LinkedPointSubController current = _headLinkedPointController;
            while (current != null)
            {
                if (current.hitTest(worldMouse))
                {
                    _levelController.selectSubController(current);
                    return true;
                }

                current = current.next;
            }

            // Hit test link lines
            current = _headLinkedPointController;
            while (current.next != null)
            {
                if (current.linkHitTest(worldMouse))
                {
                    _levelController.selectSubController(current);
                    _levelController.selectSubController(current.next);
                    return true;
                }

                current = current.next;
            }

            return false;
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
                _levelController.view.drawLine(current.position, current.next.position, Color.Blue);
                current = current.next;
            }

            // Draw points
            current = _headLinkedPointController;
            while (current != null)
            {
                _levelController.view.drawPoint(current.position, Color.LightBlue);
                current = current.next;
            }
        }

        // clone
        public override ActorController clone()
        {
            return new FluidActorController(_levelController);
        }

        #endregion

    }
}
