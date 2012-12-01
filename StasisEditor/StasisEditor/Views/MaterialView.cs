using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using StasisEditor.Controllers;

namespace StasisEditor.Views
{
    public partial class MaterialView : Form, IMaterialView
    {
        private EditorController _controller;

        public MaterialView()
        {
            InitializeComponent();
        }

        // setController
        public void setController(EditorController controller)
        {
            _controller = controller;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close the Materials View? Any unsaved changes will be lost.", "Close Materials", MessageBoxButtons.OKCancel) == DialogResult.OK)
                Close();
        }
    }
}
