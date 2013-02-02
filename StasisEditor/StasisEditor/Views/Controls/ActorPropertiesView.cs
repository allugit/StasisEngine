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
        public ActorPropertiesView(EditorActor actor)
        {
            InitializeComponent();
            actorPropertyGrid.SelectedObject = actor;
        }
    }
}
