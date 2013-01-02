using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StasisEditor.Views.Controls
{
    public partial class SelectGateType : Form
    {
        public string gateType { get { return gateTypes.SelectedItem.ToString(); } }

        public SelectGateType()
        {
            InitializeComponent();

            // Initialize gate types
            gateTypes.DataSource = new[] { "input", "output", "and", "or", "not" };
        }

        private void okayButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
