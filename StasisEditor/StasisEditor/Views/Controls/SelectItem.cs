using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Controllers;
using StasisCore.Resources;
using StasisEditor.Controllers;

namespace StasisEditor.Views.Controls
{
    public partial class SelectItem : Form
    {
        public string uid { get { return itemUIDs.SelectedItem as string; } }

        public SelectItem(EditorController controller)
        {
            InitializeComponent();

            // Populate item uids
            List<ResourceObject> resources = ResourceController.loadItems();
            List<string> uids = new List<string>(from resource in resources select resource.uid);
            itemUIDs.DataSource = uids;
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
