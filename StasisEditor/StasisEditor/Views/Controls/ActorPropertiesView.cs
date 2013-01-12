using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public partial class ActorPropertiesView : UserControl
    {
        public ActorPropertiesView(object properties)
        {
            InitializeComponent();
            Dock = DockStyle.Top;

            // Set properties and title
            propertiesGrid.SelectedObject = properties;
            propertiesTitle.Text = properties.ToString();

            // Calculate height
            if (properties is TreeProperties)
                Height = 360;
            else if (properties is BoxProperties)
                Height = 150;
            else if (properties is CircleProperties)
                Height = 150;
        }

        // refreshActorProperties
        public void refreshActorProperties()
        {
            propertiesGrid.Refresh();
        }
    }
}
