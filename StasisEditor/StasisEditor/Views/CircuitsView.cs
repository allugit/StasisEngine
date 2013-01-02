using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisCore.Models;

namespace StasisEditor.Views
{
    public partial class CircuitsView : UserControl
    {
        private CircuitController _controller;
        private bool _draw;
        private bool _keysEnabled;
        public bool active
        {
            get { return _draw && _keysEnabled; }
            set { _draw = value; _keysEnabled = value; }
        }
        public bool draw { get { return _draw; } }
        public bool keysEnabled { get { return _keysEnabled; } }
        public CircuitController controller { get { return _controller; } }
        public Circuit selectedCircuit { get { return circuitsList.SelectedItem as Circuit; } }

        public CircuitsView()
        {
            InitializeComponent();
        }

        // setController
        public void setController(CircuitController controller)
        {
            _controller = controller;
            circuitsList.DataSource = _controller.circuits;
        }

        // Selected circuit changed
        private void circuitsList_SelectedValueChanged(object sender, EventArgs e)
        {
            // Clear any selected gates
            circuitDisplay1.selectedGate = null;
        }
    }
}
