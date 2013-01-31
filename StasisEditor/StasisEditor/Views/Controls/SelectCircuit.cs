using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public partial class SelectCircuit : Form
    {
        public string circuitUID { get { return (circuitUIDs.SelectedItem as EditorCircuit).uid; } }

        public SelectCircuit(CircuitController controller)
        {
            InitializeComponent();

            // Initialize circuit uids
            circuitUIDs.DataSource = controller.circuits;
        }

        // Select button clicked
        private void selectButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        // Cancel button clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
