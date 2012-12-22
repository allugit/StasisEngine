using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;

namespace StasisEditor.Views
{
    public partial class BlueprintView : UserControl
    {
        private BlueprintController _blueprintController;

        public BlueprintView()
        {
            InitializeComponent();
        }

        // Set controller
        public void setController(BlueprintController controller)
        {
            _blueprintController = controller;
        }
    }
}
