using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    using Keys = System.Windows.Forms.Keys;

    public class PlayerSpawnActorController : ActorController, IPointSubControllable
    {
        private PointSubController _positionSubController;

        public override Vector2 connectionPosition { get { return _positionSubController.position; } }
        public override List<ActorProperties> properties
        {
            get { return new List<ActorProperties>(); }
        }
        public override XElement data
        {
            get { throw new NotImplementedException(); }
        }

        // Create new
        public PlayerSpawnActorController(LevelController levelController)
            : base(levelController, levelController.getUnusedActorID())
        {
            _type = StasisCore.ActorType.PlayerSpawn;

            // Initialize controls
            initializeControls();
        }

        // Load from xml
        public PlayerSpawnActorController(LevelController levelController, XElement data)
            : base(levelController, int.Parse(data.Attribute("id").Value))
        {
            throw new NotImplementedException();
        }

        // Initialize controls
        private void initializeControls()
        {
            _positionSubController = new PointSubController(_levelController.getWorldMouse(), this, 12f);
        }

        #region Input

        // hitTest
        public override List<ActorSubController> hitTest(Vector2 worldMouse)
        {
            List<ActorSubController> results = new List<ActorSubController>();

            // Hit test
            if (_positionSubController.hitTest(worldMouse))
                results.Add(_positionSubController);

            return results;
        }

        // globalKeyDown
        public override void globalKeyDown(Keys key)
        {
            // Delete test
            if (_positionSubController.selected && key == Keys.Delete)
                delete();

            base.globalKeyDown(key);
        }

        #endregion

        #region Actor Resource Controller Methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            _levelController.selectSubController(_positionSubController);
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            _levelController.deselectSubController(_positionSubController);
        }

        // draw
        public override void draw()
        {
            _levelController.view.drawIcon(StasisCore.ActorType.PlayerSpawn, _positionSubController.position);
        }

        // clone
        public override ActorController clone()
        {
            return new PlayerSpawnActorController(_levelController);
        }

        #endregion
    }
}
