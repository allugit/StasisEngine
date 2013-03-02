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
    }
}
