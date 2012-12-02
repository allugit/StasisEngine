using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StasisEditor.Controls
{
    public partial class MaterialProperties : UserControl
    {
        public PropertyGrid PropertyGrid { get { return this.materialPropertyGrid; } }

        public MaterialProperties()
        {
            InitializeComponent();
            Dock = DockStyle.Top;
        }
    }
}
