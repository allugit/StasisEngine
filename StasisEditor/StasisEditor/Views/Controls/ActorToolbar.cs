using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisCore.Resources;

namespace StasisEditor.Views.Controls
{
    public partial class ActorToolbar : UserControl
    {
        private LevelController _levelController;

        public ActorToolbar()
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            AutoSize = true;
        }

        // setController
        public void setController(LevelController controller)
        {
            _levelController = controller;
        }

        // Toolbar item clicked
        private void anchorToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _levelController.createActorFromToolbar(e.ClickedItem.Name);
        }
    }
}
