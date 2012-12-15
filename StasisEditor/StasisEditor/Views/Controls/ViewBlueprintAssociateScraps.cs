using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;

namespace StasisEditor.Views.Controls
{
    public partial class ViewBlueprintAssociateScraps : UserControl
    {
        public ViewBlueprintAssociateScraps(List<BlueprintScrapItemResource> scraps)
        {
            InitializeComponent();
            Dock = DockStyle.Top;

            // Populate listbox
            scrapsListbox.DataSource = scraps;
        }
    }
}
