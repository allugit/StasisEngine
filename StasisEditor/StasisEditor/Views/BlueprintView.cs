using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisEditor.Views.Controls;

namespace StasisEditor.Views
{
    public partial class BlueprintView : UserControl
    {
        private BlueprintController _controller;

        public BlueprintView()
        {
            InitializeComponent();
        }

        // Set controller
        public void setController(BlueprintController controller)
        {
            _controller = controller;
        }

        // Add blueprint
        private void addBlueprintButton_Click(object sender, EventArgs e)
        {
            CreateResourceView createResourceView = new CreateResourceView();
            if (createResourceView.ShowDialog() == DialogResult.OK)
            {
                _controller.createBlueprint(createResourceView.uid);
            }
        }
    }
}
