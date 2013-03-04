using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisEditor.Models;
using StasisEditor.Views.Controls;
using StasisCore.Controllers;

namespace StasisEditor.Views
{
    public partial class BackgroundView : UserControl
    {
        private BackgroundController _controller;

        public BackgroundController controller { get { return _controller; } set { _controller = value; } }
        public BindingList<EditorBackground> backgrounds { set { backgroundList.DataSource = value; } }

        public BackgroundView()
        {
            InitializeComponent();
        }

        // Add new background
        private void addBackgroundButton_Click(object sender, EventArgs e)
        {
            CreateResourceView createResourceView = new CreateResourceView();
            if (createResourceView.ShowDialog() == DialogResult.OK)
            {
                EditorBackground background = new EditorBackground(createResourceView.uid);
                _controller.backgrounds.Add(background);
            }
        }
    }
}
