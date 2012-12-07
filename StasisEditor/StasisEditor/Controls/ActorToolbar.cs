using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisEditor.Controllers.Actors;
using StasisCore.Models;

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

        // setController
        public void setController(ILevelController controller)
        {
            _levelController = controller;
        }

        // Toolbar item clicked
        private void anchorToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _levelController.createActorControllerFromToolbar(e.ClickedItem.Name);
        }
    }
}
