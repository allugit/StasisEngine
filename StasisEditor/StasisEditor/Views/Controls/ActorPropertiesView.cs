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
            groupBox.Text = properties.ToString();

            // Calculate height
            if (properties is CommonActorProperties)
                Height = 90;
            else if (properties is TreeProperties)
                Height = 330;
            else if (properties is BoxProperties)
                Height = 130;
            else if (properties is CircleProperties)
                Height = 90;
            else if (properties is BodyProperties)
                Height = 140;
        }

        // refreshActorProperties
        public void refreshActorProperties()
        {
            propertiesGrid.Refresh();
        }
    }
}
