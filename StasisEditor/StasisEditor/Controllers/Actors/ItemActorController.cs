using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    public class ItemActorController : ActorController, IPointSubControllable
    {
        private ItemProperties _itemProperties;
        private PointSubController _positionSubController;

        // Properties
        public override List<ActorProperties> properties
        {
            get { return new List<ActorProperties>(new[] { _itemProperties }); }
        }

        // Data
        public override XElement data
        {
            get { throw new NotImplementedException(); }
        }

        // Create new
        public ItemActorController(LevelController levelController, string itemUID)
            : base(levelController)
        {
            _itemProperties = new ItemProperties(itemUID, 1);
            _positionSubController = new PointSubController(levelController.getWorldMouse(), this);
        }

        // Load from xml
        public ItemActorController(LevelController levelController, XElement data)
            : base(levelController)
        {
            throw new NotImplementedException();
            // TODO: Initialize from xml
            // ...
        }

        #region Input

        // Hit test
        public override bool hitTest(Vector2 worldMouse, bool select = true)
        {
            // Hit test
            if (_positionSubController.hitTest(worldMouse))
            {
                // Select appropriate controls
                if (select)
                    _levelController.selectSubController(_positionSubController);
                return true;
            }

            return false;
        }

        #endregion

        #region Point controller

        // Select all sub controllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_positionSubController);
        }

        // Deselect all sub controllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_positionSubController);
        }

        #endregion

        // Draw
        public override void draw()
        {
            _levelController.view.drawIcon(StasisCore.ActorType.Item, _positionSubController.position);
        }

        // Clone
        public override ActorController clone()
        {
            return new ItemActorController(_levelController, data);
        }
    }
}
