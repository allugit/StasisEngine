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
    }
}
