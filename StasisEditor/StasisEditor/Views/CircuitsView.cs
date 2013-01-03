using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;
using StasisEditor.Models;
using StasisEditor.Controllers;
using StasisEditor.Views.Controls;

namespace StasisEditor.Views
{
    public partial class CircuitsView : UserControl
    {
        private CircuitController _controller;
        private bool _draw;
        private bool _keysEnabled;
        private Gate _selectedGate;

        public bool active
        {
            get { return _draw && _keysEnabled; }
            set { _draw = value; _keysEnabled = value; }
        }
        public bool draw { get { return _draw; } }
        public bool keysEnabled { get { return _keysEnabled; } }
        public CircuitController controller { get { return _controller; } }
        public EditorCircuit selectedCircuit { get { return circuitsList.SelectedItem as EditorCircuit; } }
        public Gate selectedGate { get { return _selectedGate; } }

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

        // selectCircuit
        public void selectCircuit(EditorCircuit circuit)
        {
            circuitsList.SelectedIndex = _controller.circuits.IndexOf(circuit);
        }

        // Select gate
        public void selectGate(Gate gate)
        {
            _selectedGate = gate;
            gateAddButton.Enabled = false;
        }

        // Deselect gate
        public void deselectGate()
        {
            _selectedGate = null;
            gateAddButton.Enabled = true;
        }

        // Selected circuit changed
        private void circuitsList_SelectedValueChanged(object sender, EventArgs e)
        {
            // Clear any selected gates
            _selectedGate = null;

            // Enable buttons
            circuitDeleteButton.Enabled = selectedCircuit != null;
            gateAddButton.Enabled = selectedCircuit != null && _selectedGate == null;
        }

        // Create circuit button clicked
        private void circuitCreateButton_Click(object sender, EventArgs e)
        {
            CreateResourceView resourceView = new CreateResourceView();
            if (resourceView.ShowDialog() == DialogResult.OK)
            {
                EditorCircuit circuit = new EditorCircuit(resourceView.uid);
                _controller.circuits.Add(circuit);
                selectCircuit(circuit);
            }
        }

        // Delete circuit button clicked
        private void circuitDeleteButton_Click(object sender, EventArgs e)
        {
            int index = _controller.circuits.IndexOf(selectedCircuit);

            if (selectedCircuit != null)
                _controller.circuits.Remove(selectedCircuit);

            if (index > 0)
                selectCircuit(_controller.circuits[index - 1]);
        }

        // Add gate button clicked
        private void gateAddButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(selectedCircuit != null);

            SelectGateType selectGateTypeForm = new SelectGateType();
            if (selectGateTypeForm.ShowDialog() == DialogResult.OK)
            {
                Gate gate = new Gate(_controller.getUnusedGateID(selectedCircuit), selectGateTypeForm.gateType, _controller.getWorldMouse());
                selectedCircuit.gates.Add(gate);
                _selectedGate = gate;
            }
        }

        // Save circuits clicked
        private void saveCircuitsButton_Click(object sender, EventArgs e)
        {
            _controller.saveCircuits();
        }
    }
}
