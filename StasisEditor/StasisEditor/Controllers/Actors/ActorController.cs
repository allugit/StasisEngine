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
        protected int _id;
        protected ActorType _type;
        protected LevelController _levelController;
        protected List<CircuitConnectionSubController> _circuitConnections;

        public int id { get { return _id; } }
        public ActorType type { get { return _type; } }
        public bool shift { get { return _levelController.shift; } }
        public bool ctrl { get { return _levelController.ctrl; } }
        abstract public List<ActorProperties> properties { get; }
        abstract public XElement data { get; }
        abstract public Vector2 connectionPosition { get; }

        public ActorController(LevelController levelController, int id)
        {
            _levelController = levelController;
            _id = id;
            _circuitConnections = new List<CircuitConnectionSubController>();
            Application.Idle += new EventHandler(Application_Idle);
        }

        // Constantly update the position of circuit connection sub controllers
        void Application_Idle(object sender, EventArgs e)
        {
            foreach (CircuitConnectionSubController circuitConnectionSubController in _circuitConnections)
                circuitConnectionSubController.updatePosition(connectionPosition);
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

        // Connect circuit
        public void connectCircuit(CircuitConnectionSubController circuitConnection)
        {
            System.Diagnostics.Debug.Assert(!_circuitConnections.Contains(circuitConnection));
            _circuitConnections.Add(circuitConnection);
        }

        // Disconnect circuit
        public void disconnectCircuit(CircuitConnectionSubController circuitConnection)
        {
            System.Diagnostics.Debug.Assert(_circuitConnections.Contains(circuitConnection));
            _circuitConnections.Remove(circuitConnection);
        }

        // handleLeftMouseDown
        virtual public bool handleLeftMouseDown()
        {
            List<ActorSubController> results = hitTest(_levelController.getWorldMouse());
            foreach (ActorSubController subController in results)
                _levelController.selectSubController(subController);

            // Mouse has been handled if results exist
            return results.Count > 0;
        }

        // handleRightMouseDown
        virtual public bool handleRightMouseDown()
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

            return false;
        }

        // handleMouseDown
        virtual public bool handleMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            // Handle left and right clicks
            if (e.Button == MouseButtons.Left)
                return handleLeftMouseDown();
            else if (e.Button == MouseButtons.Right)
                return handleRightMouseDown();

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
