using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    abstract public class ActorController
    {
        protected ActorType _type;
        protected LevelController _levelController;

        public ActorType type { get { return _type; } }
        public bool shift { get { return _levelController.shift; } }
        public bool ctrl { get { return _levelController.ctrl; } }
        abstract public List<ActorProperties> properties { get; }
        abstract public XElement data { get; }

        public ActorController(LevelController levelController)
        {
            _levelController = levelController;
        }

        // getLevelController
        public LevelController getLevelController()
        {
            return _levelController;
        }

        // selectAllSubControllers
        abstract public void selectAllSubControllers();

        // deselectAllSubControllers
        abstract public void deselectAllSubControllers();

        // selectSubController
        public void selectSubController(ActorSubController subController)
        {
            _levelController.selectSubController(subController);
        }

        // deselectSubController
        public void deselectSubController(ActorSubController subController)
        {
            _levelController.deselectSubController(subController);
        }

        // hitTest
        abstract public List<ActorSubController> hitTest(Vector2 worldMouse);

        // delete
        virtual public void delete()
        {
            deselectAllSubControllers();
            _levelController.removeActorController(this);
        }

        // handleMouseDown
        virtual public bool handleMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                List<ActorSubController> results = hitTest(_levelController.getWorldMouse());
                foreach (ActorSubController subController in results)
                    _levelController.selectSubController(subController);

                // Mouse has been handled
                if (results.Count > 0)
                    return true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                List<ActorSubController> results = hitTest(_levelController.getWorldMouse());

                if (results.Count > 0)
                {
                    // Show actor properties
                    _levelController.closeActorProperties();
                    _levelController.openActorProperties(properties);

                    // Mouse has been handled
                    return true;
                }
            }

            return false;
        }

        // globalKeyDown
        virtual public void globalKeyDown(Keys key) { }

        // globalKeyUp
        //virtual public void globalKeyUp(Keys key) { }

        // draw
        abstract public void draw();

        // clone
        abstract public ActorController clone();
    }
}
