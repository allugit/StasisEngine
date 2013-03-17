using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisCore;

namespace StasisEditor.Views.Controls
{
    public partial class SelectItem : Form
    {
        public string uid { get { return itemUIDs.SelectedItem as string; } }

        public SelectItem(EditorController controller)
        {
            InitializeComponent();

            // Populate item uids
            ResourceManager.loadAllItems(new FileStream(ResourceManager.itemPath, FileMode.Open));
            List<XElement> resources = ResourceManager.itemResources;
            resources.AddRange(ResourceManager.blueprintResources);
            List<string> uids = new List<string>(from resource in resources select resource.Attribute("uid").Value);
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
