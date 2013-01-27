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

    public class ItemActorController : ActorController, IPointSubControllable
    {
        private ItemProperties _itemProperties;
        private PointSubController _positionSubController;

        public override Vector2 connectionPosition { get { return _positionSubController.position; } }
        public override List<ActorProperties> properties
        {
            get 
            { 
                return new List<ActorProperties>(new ActorProperties[]
                { 
                    _commonProperties,
                    _itemProperties
                });
            }
        }
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.Add(_itemProperties.data);
                d.SetAttributeValue("position", _positionSubController.position);
                return d;
            }
        }

        // Create new
        public ItemActorController(LevelController levelController, string itemUID)
            : base(levelController, levelController.getUnusedActorID())
        {
            _itemProperties = new ItemProperties(itemUID, 1);
            _commonProperties.depth = 0.1f;
            _type = ActorType.Item;
            initializeControls(levelController.getWorldMouse());
        }

        // Load from xml
        public ItemActorController(LevelController levelController, XElement data)
            : base(levelController, int.Parse(data.Attribute("id").Value))
        {
            _itemProperties = new ItemProperties(data);
            _type = ActorType.Item;
            _commonProperties = new CommonActorProperties(data);
            initializeControls(Loader.loadVector2(data.Attribute("position"), Vector2.Zero));
        }

        // Initialize controls
        private void initializeControls(Vector2 position)
        {
            _positionSubController = new PointSubController(position, this);
        }

        #region Input

        // Hit test
        public override List<ActorSubController> hitTest(Vector2 worldMouse)
        {
            List<ActorSubController> results = new List<ActorSubController>();

            // Hit test
            if (_positionSubController.hitTest(worldMouse))
                results.Add(_positionSubController);

            return results;
        }

        public override void globalKeyDown(Keys key)
        {
            // Delete test
            if (_positionSubController.selected && key == Keys.Delete)
                delete();

            base.globalKeyDown(key);
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
            _levelController.view.drawIcon(StasisCore.ActorType.Item, _positionSubController.position, _commonProperties.depth);
        }

        // Clone
        public override ActorController clone()
        {
            return new ItemActorController(_levelController, data);
        }
    }
}
