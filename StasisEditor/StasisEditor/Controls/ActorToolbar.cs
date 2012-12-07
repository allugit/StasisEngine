using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;

namespace StasisEditor.Controls
{
    public partial class ActorToolbar : UserControl
    {
        private ILevelController _levelController;

        public ActorToolbar()
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            AutoSize = true;
        }

        public void setController(ILevelController controller)
        {
            _levelController = controller;
        }
    }
}
